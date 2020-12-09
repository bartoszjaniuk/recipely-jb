import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IRecipe } from 'src/app/_models/recipe';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-recipe-card',
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css'],
})
export class RecipeCardComponent implements OnInit {
  @Input() recipe: IRecipe;
  constructor(
    private recipeService: RecipeService,
    private toastrService: ToastrService,
    public accountService: AccountService
  ) {}

  ngOnInit(): void {}

  addToFav(id: number) {
    this.recipeService.addRecipeToFavourite(id).subscribe((data) => {
      this.toastrService.success(
        'Added ' + this.recipe.name + ' to your favourites'
      );
    });
  }
}
