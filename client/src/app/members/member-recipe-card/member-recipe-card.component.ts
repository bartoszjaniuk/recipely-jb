import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IMember } from 'src/app/_models/member';
import { IRecipe } from 'src/app/_models/recipe';
import { MembersService } from '../members.service';

@Component({
  selector: 'app-member-recipe-card',
  templateUrl: './member-recipe-card.component.html',
  styleUrls: ['./member-recipe-card.component.css']
})
export class MemberRecipeCardComponent implements OnInit {
  @Input() recipe: IRecipe;
  member: IMember;
  constructor(private accountService: AccountService, private toastrService: ToastrService,
              private memberService: MembersService) { }

  ngOnInit() {
  }

  // addToFav(recipe: IRecipe) {
  //   this.memberService.addRecipeToFavourite(recipe.id, this.member.id).subscribe(data => {
  //     this.toastrService.success('Added to fav ' + this.recipe.name);
  //   }, error => {
  //     this.toastrService.error(error);
  //   })
    
    
    // this.userService.addToFav(this.authService.decodedToken.nameid, id).subscribe(data => {
    // this.alertify.success('Added to fav ' + this.recipe.name);
    // }, error => {
    // this.alertify.error(error);
    // });
    // }

}