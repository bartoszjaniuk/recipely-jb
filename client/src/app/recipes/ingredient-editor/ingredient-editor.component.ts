import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { ControlContainer, FormArray, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { IIngredient } from 'src/app/_models/ingredient';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-ingredient-editor',
  templateUrl: './ingredient-editor.component.html',
  styleUrls: ['./ingredient-editor.component.css']
})
export class IngredientEditorComponent implements OnInit {
  @Input() ingredients: IIngredient[];
  constructor(private toastr: ToastrService, private recipeService: RecipeService,
    public controlContainer: ControlContainer) { }

  ngOnInit(): void {}

  deleteIngredient(id: number) {
    this.recipeService.deleteIngredient(id).subscribe(() => {
      this.ingredients.splice(this.ingredients.findIndex(p => p.id === id), 1);
      this.toastr.success('Ingredient has been deleted');
    }, error => {
      this.toastr.error('Deleting ingredient has failed')
    })
  }

}
