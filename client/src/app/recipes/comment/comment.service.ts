import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map} from 'rxjs/operators';
import { IComment } from 'src/app/_models/comment';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  baseUrl = environment.apiUrl;
  comments: IComment[] = [];

  constructor(private http: HttpClient) { }

  getComments(recipeId: number) {
    return this.http.get<IComment[]>(this.baseUrl + 'recipes/' + recipeId + '/comments').pipe(
      map(comments => {
        this.comments = comments;
        return comments;
      })
    )
  }
  deleteComment(id: number) {
    return this.http.delete(this.baseUrl + 'comments/' + id);
  }
  ///api/Recipes/{recipeId}/Comments
  addComment(comment: IComment, recipeId: number) {
    return this.http.post(this.baseUrl + 'recipes/' + recipeId + '/comments', comment);
  }
}


