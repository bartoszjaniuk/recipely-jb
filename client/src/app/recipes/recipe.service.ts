import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICategory } from '../_models/category';
import { IKitchenOrigin } from '../_models/kitchenOrigin';
import { IRecipe } from '../_models/recipe';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  
  recipes: IRecipe[] = [];
  categories: ICategory[] = [];


  currentRecipe: IRecipe;
  photoUrl = new BehaviorSubject<string> ('./assets/not-found.jpg');
  currentPhotoUrl = this.photoUrl.asObservable();
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient, private recipeService: RecipeService) { }

  getRecipes() {
    if (this.recipes.length > 0) return of(this.recipes);
    return this.http.get<IRecipe[]>(this.baseUrl + 'recipes').pipe(
      map(recipes => {
        this.recipes = recipes;
        return recipes;
      })
    )
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
    if (this.categories.length > 0) return of(this.categories);
    return this.http.get<ICategory[]>(this.baseUrl + 'types/' + 'categories').pipe(
      map(categories => {
        this.categories = categories;
        return categories;
      })
    )
  }

  getKitchenOrigins() {
    if (this.categories.length > 0) return of(this.categories);
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
