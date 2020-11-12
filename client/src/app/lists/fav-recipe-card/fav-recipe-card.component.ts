import { Component, Input, OnInit } from '@angular/core';
import { IFavouriteRecipe } from 'src/app/_models/favouriteRecipe';

@Component({
  selector: 'app-fav-recipe-card',
  templateUrl: './fav-recipe-card.component.html',
  styleUrls: ['./fav-recipe-card.component.css']
})
export class FavRecipeCardComponent implements OnInit {
  @Input() recipe: IFavouriteRecipe;
  constructor() { }

  ngOnInit(): void {
  }

}
