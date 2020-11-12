import { Component, OnInit } from '@angular/core';
import { MembersService } from 'src/app/members/members.service';
import { IFavouriteRecipe } from 'src/app/_models/favouriteRecipe';
import { IRecipe } from 'src/app/_models/recipe';

@Component({
  selector: 'app-favourite-recipes-list',
  templateUrl: './favourite-recipes-list.component.html',
  styleUrls: ['./favourite-recipes-list.component.css']
})
export class FavouriteRecipesListComponent implements OnInit {
  recipes: IFavouriteRecipe[];
  constructor(private membersService: MembersService) { }

  ngOnInit(): void {
    this.loadFavRecipes();
  }

  loadFavRecipes() {
    this.membersService.getFavouriteRecipes().subscribe(response => {
      this.recipes = response;
    }
    )}
}
