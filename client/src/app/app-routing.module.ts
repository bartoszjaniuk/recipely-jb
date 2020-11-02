import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { LikeListsComponent } from './lists/like-lists/like-lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages/messages.component';
import { RecipeAddComponent } from './recipes/recipe-add/recipe-add.component';
import { RecipeDetailComponent } from './recipes/recipe-detail/recipe-detail.component';
import { RecipeEditComponent } from './recipes/recipe-edit/recipe-edit.component';
import { RecipeListComponent } from './recipes/recipe-list/recipe-list.component';
import { StartupComponent } from './startup/startup.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailResolver } from './_resolvers/member-detailed.resolver';

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
  {path: 'member/edit/recipes/:id/edit', component: RecipeEditComponent},
  {path: 'members', component: MemberListComponent},
  {path: 'members/:username', component: MemberDetailComponent, resolve: {member: MemberDetailResolver}},
  {path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard]},
  {path: 'startup', component: StartupComponent},
  {path: 'errors', component: TestErrorsComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
