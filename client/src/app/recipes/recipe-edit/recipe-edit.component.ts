import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { IPhoto } from 'src/app/_models/photo';
import { IRecipe } from 'src/app/_models/recipe';
import { environment } from 'src/environments/environment';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-recipe-edit',
  templateUrl: './recipe-edit.component.html',
  styleUrls: ['./recipe-edit.component.css']
})
export class RecipeEditComponent implements OnInit {
  @ViewChild('editForm', {static: true}) editForm: NgForm;
  recipe: IRecipe;
  photos: IPhoto[] = [];
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: IPhoto;
  constructor(private route: ActivatedRoute, private recipeService: RecipeService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'recipes/' + this.recipe.id + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
  

  this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };


  //   this.uploader.onSuccessItem = (item, response, status, headers) => {
  //     if (response) {
  //       const res: IPhoto = JSON.parse(response);
  //       const photo = {
  //         id: res.id,
  //         url: res.url,
  //         isMain: res.isMain
  //       };
  //       this.photos.push(photo);
  //       this.recipe.recipePhotos.push(photo);

  //       if (photo.isMain) {
  //         this.recipeService.changeRecipePhoto(photo.url);
  //         this.recipeService.currentRecipe.photoUrl = photo.url;
  //         this.recipe = this.recipeService.currentRecipe;
  //       }
  //     }
  //   };
  }

}
