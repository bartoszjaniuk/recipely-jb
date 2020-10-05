import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LikeListsComponent } from './lists/like-lists/like-lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages/messages.component';
import { RecipeAddComponent } from './recipes/recipe-add/recipe-add.component';
import { RecipeDetailComponent } from './recipes/recipe-detail/recipe-detail.component';
import { RecipeListComponent } from './recipes/recipe-list/recipe-list.component';
import { StartupComponent } from './startup/startup.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path: '', component: StartupComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MemberListComponent},
      {path: 'members/:id', component: MemberDetailComponent},
      {path: 'likes', component: LikeListsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'recipes', component: RecipeListComponent},
      {path: 'recipes/:id', component: RecipeDetailComponent},
      {path: 'recipes/add-recipe', component: RecipeAddComponent},
      // TODO
    ]
  },
  {path: '**', redirectTo: '', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
