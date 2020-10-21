import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { RecipeService } from 'src/app/recipes/recipe.service';
import { IRecipe } from 'src/app/_models/recipe';

@Component({
  selector: 'app-recipe-editor',
  templateUrl: './recipe-editor.component.html',
  styleUrls: ['./recipe-editor.component.css']
})
export class RecipeEditorComponent implements OnInit {
  @Input() recipes: IRecipe[];
  constructor(private toastr: ToastrService, private recipeService: RecipeService,
     private accountService: AccountService) { }

  ngOnInit(): void {
  }

  // deleteRecipe(id: number) {
  //   this.alertify.confirm('Are you sure you want to delete this recipe?', () => {
  //     this.recipeService.deleteRecipe(this.authService.decodedToken.nameid, id).subscribe(() => {
  //       this.recipes.splice(this.recipes.findIndex(p => p.id === id), 1);
  //       this.alertify.success('Recipe has been deleted');
  //     }, error => {
  //       this.alertify.error('Failed to delete the recipe');
  //     });
  //   });
  // }

  deleteRecipe(id: number) {
    this.recipeService.deleteRecipe(id).subscribe(() => {
      this.recipes.splice(this.recipes.findIndex(p => p.id === id), 1);
      this.toastr.success('Recipe has been deleted');
    }, error => {
      this.toastr.error('Deleting recipe has failed')
    })
  }

}
