import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICategory } from '../_models/category';
import { IRecipe } from '../_models/recipe';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  baseUrl = environment.apiUrl;
  recipes: IRecipe[] = [];
  categories: ICategory[] = [];
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
    return this.http.get<IRecipe>(this.baseUrl + 'recipes/' + id);
  }


  // getMember(id: number) {
  //   const recipe = this.recipes.find(x => x.id === id);
  //   if (recipe !== undefined) return of(recipe);
  //   return this.http.get<IRecipe>(this.baseUrl + 'recipes/' + id);
  // }


  updateMember(recipe: IRecipe) {
    return this.http.put(this.baseUrl + 'recipes', recipe).pipe(
      map(() => {
        const index = this.recipes.indexOf(recipe);
        this.recipes[index] = recipe;
      })
    )
  }

  getCategories() {
    if (this.categories.length > 0) return of(this.categories);
    return this.http.get<ICategory[]>(this.baseUrl + 'recipes' + '/categories').pipe(
      map(categories => {
        this.categories = categories;
        return categories;
      })
    )
  }


  // TODO delete recipe
}
