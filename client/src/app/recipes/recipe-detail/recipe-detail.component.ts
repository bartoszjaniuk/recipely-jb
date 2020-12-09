import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  NgxGalleryAnimation,
  NgxGalleryImage,
  NgxGalleryOptions,
} from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IRecipe } from 'src/app/_models/recipe';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-recipe-detail',
  templateUrl: './recipe-detail.component.html',
  styleUrls: ['./recipe-detail.component.css'],
})
export class RecipeDetailComponent implements OnInit {
  recipe: IRecipe;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  backgroundImage = './assets/background.png';

  constructor(
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    public accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.loadRecipe();

    this.galleryOptions = [
      {
        width: '1000px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: true,
      },
    ];
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.recipe.recipePhotos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url,
      });
    }
    return imageUrls;
  }

  loadRecipe() {
    this.recipeService
      .getRecipe(this.route.snapshot.params.id)
      .subscribe((recipe) => {
        this.recipe = recipe;
        this.galleryImages = this.getImages();
      });
  }
  addToFav(id: number) {
    this.recipeService.addRecipeToFavourite(id).subscribe((data) => {
      this.toastr.success('Added ' + this.recipe.name + ' to your favourites');
    });
  }
}
