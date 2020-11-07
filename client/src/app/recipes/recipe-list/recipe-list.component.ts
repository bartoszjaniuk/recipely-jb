import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ICategory } from 'src/app/_models/category';
import { IKitchenOrigin } from 'src/app/_models/kitchenOrigin';
import { IPagination } from 'src/app/_models/pagination';
import { IRecipe } from 'src/app/_models/recipe';
import { RecipeParams } from 'src/app/_models/recipeParams';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.css']
})
export class RecipeListComponent implements OnInit {
  @ViewChild('search') searchTerm: ElementRef;
  recipeParams = new RecipeParams();
  recipes: IRecipe[];
  pagination: IPagination;
  categories: ICategory[];
  kitchenOrigins: IKitchenOrigin[];
  totalCount: number;
  orderByOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Preparation Time: Short to Long', value: 'minTime'},
    {name: 'Preparation Time: Long to Short', value: 'maxTime'}
  ];
  constructor(private recipeService: RecipeService) { 
  }

  ngOnInit(): void {
    this.loadRecipes();
    this.getCategories();
    this.getKitchenOrigins();
  }


  loadRecipes() {
    this.recipeService.getRecipes(this.recipeParams)
    .subscribe(response => {
      this.recipes = response.result;
      this.pagination = response.pagination;
    })
  }

  onCategorySelected(categoryId: any) {
    this.recipeParams.categoryId = categoryId;
    this.loadRecipes();
  }

  onSortSelected(orderBy: any) {
    this.recipeParams.orderBy = orderBy;
    this.loadRecipes();
  }

  onKitchenOriginSelected(kitchenOriginId: any) {
    this.recipeParams.kitchenOriginId = kitchenOriginId;
    this.loadRecipes();
  }

  pageChanged(event: any) {
    this.recipeParams.pageNumber = event.page;
    this.loadRecipes();
  }

  getCategories() {
    this.recipeService.getCategories().subscribe(response => {
      this.categories = [{id: 0, name: 'All'}, ...response];
    });
  }

  getKitchenOrigins() {
    this.recipeService.getKitchenOriginsWithRecipesOnly().subscribe(response => {
      this.kitchenOrigins = [{id: 0, name: 'All'}, ...response];
    });
  }

  onSearch() {
    this.recipeParams.search = this.searchTerm.nativeElement.value;
    this.recipeParams.pageNumber = 1;
    this.loadRecipes();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.recipeParams = new RecipeParams();
    this.loadRecipes();
  }

}
