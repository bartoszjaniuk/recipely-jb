import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ICategory } from 'src/app/_models/category';
import { IRecipe } from 'src/app/_models/recipe';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.css']
})
export class RecipeListComponent implements OnInit {
  recipes$: Observable<IRecipe[]>;
  categories$: Observable<ICategory[]>;
  constructor(private recipeService: RecipeService) { }

  ngOnInit(): void {
    this.recipes$ = this.recipeService.getRecipes();
    this.categories$ = this.recipeService.getCategories();
  }


  // loadRecipes() {
  //   this.recipeService.getRecipes()
  //   .subscribe(()=> {
  //     this.recipes = res.result;
  //   }, error => {
  //     this.alertify.error(error);
  //   });
  // }
  // getCategories() {
  //   this.recipeService.getCategories().subscribe(response => {
  //     this.categories = response;
  //   }, error => {
  //     this.alertify.error(error);
  //   });
  // }

}
