import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { getPaginatedResult, getPaginationHeaders } from '../_helpers/paginationHelper';
import { ICategory } from '../_models/category';
import { IKitchenOrigin } from '../_models/kitchenOrigin';
import { PaginatedResult } from '../_models/pagination';
import { IRecipe } from '../_models/recipe';
import { RecipeParams } from '../_models/recipeParams';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  
  recipes: IRecipe[] = [];
  categories: ICategory[] = [];
  paginatedResult: PaginatedResult<IRecipe[]> = new PaginatedResult<IRecipe[]>();


  currentRecipe: IRecipe;
  photoUrl = new BehaviorSubject<string> ('./assets/not-found.jpg');
  currentPhotoUrl = this.photoUrl.asObservable();
  baseUrl = environment.apiUrl;
  memberCache = new Map();
  constructor(private http: HttpClient, private recipeService: RecipeService) { }

  getRecipes(recipeParams: RecipeParams) {
    var response = this.memberCache.get(Object.values(recipeParams).join('-'))
    if(response) {
      return of(response);
    }
    let params = getPaginationHeaders(recipeParams.pageNumber, recipeParams.pageSize);

    if (recipeParams.categoryId !== 0) {
      params = params.append('categoryId', recipeParams.categoryId.toString());
     }
 
     if (recipeParams.kitchenOriginId !== 0) {
      params = params.append('kitchenOriginId', recipeParams.kitchenOriginId.toString());
     }
 
     if (recipeParams.orderBy) {
       params = params.append('orderBy', recipeParams.orderBy);
      }

     if (recipeParams.search) {
       params = params.append('search', recipeParams.search);
     }
    return getPaginatedResult<IRecipe[]>(this.baseUrl + 'recipes', params, this.http)
    .pipe(map(response => {
      this.memberCache.set(Object.values(recipeParams).join('-'), response);
      return response;
    }))
  }


  getRecipe(id: number) {
    const recipe = this.recipes.find(x => x.id === id);
    if (recipe !== undefined) return of(recipe);
    return this.http.get<IRecipe>(this.baseUrl + 'recipes/' + id);
  }

  addNewRecipe(recipe: IRecipe) {
    return this.http.post(this.baseUrl + 'users/' + 'add-recipe', recipe);
  }

  deleteRecipe(id: number) {
    return this.http.delete(this.baseUrl + 'recipes/' + id);
  }

  deleteIngredient(id: number) {
    return this.http.delete(this.baseUrl + 'recipes/ingredients/' + id);
  }


  editRecipe(id: number, recipe: IRecipe) {
    return this.http.put(this.baseUrl + 'recipes/' + id, recipe).pipe(
      map(() => {
        const index = this.recipes.indexOf(recipe);
        this.recipes[index] = recipe;
      })
    )
  }


  getCategories() {
    
    return this.http.get<ICategory[]>(this.baseUrl + 'types/' + 'categories').pipe(
      map(categories => {
        this.categories = categories;
        return categories;
      })
    )
  }

  getKitchenOrigins() {

    return this.http.get<IKitchenOrigin[]>(this.baseUrl + 'types/' + 'kitchen-origins').pipe(
      map(categories => {
        this.categories = categories;
        return categories;
      })
    )
  }

  deletePhoto(recipeId: number, id: number) {
    return this.http.delete(this.baseUrl + 'recipes/' + recipeId + '/delete-photo/' + id);
  }

  setMainPhoto(recipeId: number, id: number) {
    return this.http.put(this.baseUrl + 'recipes/' + recipeId + '/set-main-photo/'  + id, {});
  }

  changeRecipePhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }
  
}
