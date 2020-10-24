import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { IIngredient } from 'src/app/_models/ingredient';
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
  @ViewChild('editForm') editForm;
  recipe: IRecipe;
  photos: IPhoto[] = [];
  ingredients: IIngredient[] = [];
  ingredient: IIngredient;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  
  currentMain: IPhoto;
  constructor(private route: ActivatedRoute, private recipeService: RecipeService,
    private toastr: ToastrService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.loadRecipe(); 

  }

  addIngredient(): void {
    this.recipe.ingredients.push({
      name: 'Ingredient',
      amount: 1
    });
  }

  createIngredient(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      amount: ['', Validators.required],
    });
  }
  loadRecipe() {
    this.recipeService.getRecipe(this.route.snapshot.params.id).subscribe(recipe => {
      this.recipe = recipe;
      this.initializeUploader();
    })
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader () {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'recipes/' + this.recipe.id + '/add-photo',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo : IPhoto = JSON.parse(response);
        this.recipe.recipePhotos.push(photo);
        if (photo.isMain) {
          this.recipe.photoUrl = photo.url;
        }
      }
    }
  }



  deleteIngredient(id: number) {
    this.recipeService.deleteIngredient(id).subscribe(() => {
      this.ingredients.splice(this.ingredients.findIndex(p => p.id === id), 1);
      this.toastr.success('Ingredient has been deleted');
      this.loadRecipe(); 
    }, error => {
      this.toastr.error('Deleting ingredient has failed')
    })
  }

  updateRecipe(id: number) {
    this.recipeService.editRecipe(id, this.recipe).subscribe(next => {
      this.toastr.success('Recipe updated successfully');
    }, error => {
      this.toastr.error(error);
    });
  }

  setMainPhoto(photo: IPhoto) {
    this.recipeService.setMainPhoto(this.recipe.id, photo.id)
      .subscribe(() => {
        this.currentMain = this.recipe.recipePhotos.filter(p => p.isMain === true)[0];
        if (this.currentMain) {
          this.currentMain.isMain = false;
          photo.isMain = true;
          this.toastr.success('Succesfully changed main photo');
        }
      }, error => {
        this.toastr.error(error);
      });
  }

  deletePhoto(id: number) {

    this.recipeService.deletePhoto(this.recipe.id, id).subscribe(() => {
      this.recipe.recipePhotos.splice(this.recipe.recipePhotos.findIndex(p => p.id === id), 1);
      this.toastr.success('Photo has been deleted');
    }, error => {
      this.toastr.error('Failed to delete the photo');
    });
  }

}
