import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { IRecipe } from 'src/app/_models/recipe';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-recipe-card',
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent implements OnInit {
  @Input() recipe: IRecipe;
  constructor(private recipeService: RecipeService, private toastrService: ToastrService) { }

  ngOnInit(): void {
  }

  addToFav(id: number) {
    this.recipeService.addRecipeToFavourite(id).subscribe(data => {
    this.toastrService.success('Added ' + this.recipe.name + ' to your favourites');
    }, error => {
    this.toastrService.error(error);
    });
    }

}
