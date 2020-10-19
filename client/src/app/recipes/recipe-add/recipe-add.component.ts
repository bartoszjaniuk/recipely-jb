import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Form, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ICategory } from 'src/app/_models/category';
import { IKitchenOrigin } from 'src/app/_models/kitchenOrigin';
import { IMember } from 'src/app/_models/member';
import { IRecipe } from 'src/app/_models/recipe';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-recipe-add',
  templateUrl: './recipe-add.component.html',
  styleUrls: ['./recipe-add.component.css']
})
export class RecipeAddComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  categories: ICategory[];
  kitchenOrigins: IKitchenOrigin[];
  member: IMember;
  recipe: IRecipe;

  recipeForm: FormGroup;
  ingredients: FormArray;



  ingredientForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private recipeService: RecipeService,
    private toastr: ToastrService, private fb: FormBuilder, private router: Router) {

  }

  ngOnInit(): void {
    this.intitializeRecipeForm();
    this.getCategories();
    this.getKitchenOrigins();
  }

  intitializeRecipeForm() {
    this.recipeForm = this.fb.group({
      name: ['', Validators.required],
      preparationTime: ['', Validators.required],
      description: ['', Validators.required],
      numberOfCalories: ['', Validators.required],
      categoryId: ['', Validators.required],
      kitchenOriginId: ['', Validators.required],
      ingredients: this.fb.array([this.createIngredient()])
    });
  }


  createIngredient(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      amount: ['', Validators.required],
    });
  }

  addIngredient(): void {
    this.ingredients = this.recipeForm.get('ingredients') as FormArray;
    this.ingredients.push(this.createIngredient());
  }

  
  onSubmit() {
    console.log(this.recipeForm.value);
  }





 removeIngredient(i:number) {
  this.ingredients.removeAt(i);
}


  addRecipe() {
    this.recipeService.addNewRecipe(this.recipeForm.value).subscribe(response => {
      this.router.navigateByUrl('/recipes');
    }, error => {
      this.validationErrors = error;
    })
  }


  getCategories() {
    this.recipeService.getCategories().subscribe(response => {
      this.categories = response;
    }, error => {
      this.toastr.error(error);
    });
  }

  getKitchenOrigins() {
    this.recipeService.getKitchenOrigins().subscribe(response => {
      this.kitchenOrigins = response;
    }, error => {
      this.toastr.error(error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }



}
