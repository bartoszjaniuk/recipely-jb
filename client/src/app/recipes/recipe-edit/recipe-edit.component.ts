import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IIngredient } from 'src/app/_models/ingredient';
import { IMember } from 'src/app/_models/member';
import { IPhoto } from 'src/app/_models/photo';
import { IRecipe } from 'src/app/_models/recipe';
import { environment } from 'src/environments/environment';
import { RecipeService } from '../recipe.service';

@Component({
  selector: 'app-recipe-edit',
  templateUrl: './recipe-edit.component.html',
  styleUrls: ['./recipe-edit.component.css'],
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
  currentUser;
  currentMain: IPhoto;
  constructor(
    private route: ActivatedRoute,
    private recipeService: RecipeService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    public accountService: AccountService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadRecipe();
    console.log(this.accountService.currentUser$);
    this.accountService.currentUser$.subscribe((data) => {
      (data) => data.username;
      this.currentUser = data.username;
    });
  }

  addIngredient(): void {
    this.recipe.ingredients.push({
      name: 'Ingredient',
      amount: 'Amount',
    });
  }

  createIngredient(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      amount: ['', Validators.required],
    });
  }
  loadRecipe() {
    this.recipeService
      .getRecipe(this.route.snapshot.params.id)
      .subscribe((recipe) => {
        this.recipe = recipe;
        this.initializeUploader();
      });
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'recipes/' + this.recipe.id + '/add-photo',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: IPhoto = JSON.parse(response);
        this.recipe.recipePhotos.push(photo);
        if (photo.isMain) {
          this.recipe.photoUrl = photo.url;
        }
      }
    };
  }

  deleteIngredient(id: number) {
    this.recipeService.deleteIngredient(id).subscribe(
      () => {
        this.ingredients.splice(
          this.ingredients.findIndex((p) => p.id === id),
          1
        );
        this.toastr.success('Ingredient has been deleted');
        this.loadRecipe();
      },
      (error) => {
        this.toastr.error('Deleting ingredient has failed');
      }
    );
  }

  updateRecipe(id: number) {
    this.recipeService.editRecipe(id, this.recipe).subscribe(
      (next) => {
        this.toastr.success('Recipe updated successfully');
        this.editForm.reset(this.recipe);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  setMainPhoto(photo: IPhoto) {
    this.recipeService.setMainPhoto(this.recipe.id, photo.id).subscribe(
      () => {
        this.currentMain = this.recipe.recipePhotos.filter(
          (p) => p.isMain === true
        )[0];
        if (this.currentMain) {
          this.currentMain.isMain = false;
          photo.isMain = true;
          this.toastr.success('Succesfully changed main photo');
        }
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  deletePhoto(id: number) {
    this.recipeService.deletePhoto(this.recipe.id, id).subscribe(() => {
      this.recipe.recipePhotos.splice(
        this.recipe.recipePhotos.findIndex((p) => p.id === id),
        1
      );
      this.toastr.success('Photo has been deleted');
    });
  }

  navigate() {
    this.router.navigate(['/not-found']);
  }
}
