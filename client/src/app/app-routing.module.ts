import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
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
  {path: '', component: RecipeListComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'likes', component: LikeListsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'recipe-add', component: RecipeAddComponent}    // TODO
    ]
  },
  {path: 'recipes', component: RecipeListComponent},
  {path: 'recipes/:id', component: RecipeDetailComponent},
  {path: 'members', component: MemberListComponent},
  {path: 'members/:id', component: MemberDetailComponent},
  {path: 'startup', component: StartupComponent},
  {path: '**', redirectTo: ''}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
