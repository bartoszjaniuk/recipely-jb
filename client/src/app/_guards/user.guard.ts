import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../account/account.service';
import { IRecipe } from '../_models/recipe';

@Injectable({
  providedIn: 'root',
})
export class UserGuard implements CanActivate {
  recipe: IRecipe;
  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map((user) => {
        if (user.username === this.recipe.authorUserName) {
          return true;
        }
        this.toastr.error('You cannot enter this area');
      })
    );
  }
}
