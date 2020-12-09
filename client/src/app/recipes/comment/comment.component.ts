import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IComment } from 'src/app/_models/comment';
import { CommentService } from './comment.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css'],
})
export class CommentComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  comments: IComment[] = [];
  commentForm: FormGroup;
  validationErrors: string[] = [];
  constructor(
    public commentService: CommentService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.intitializeRecipeForm();
    this.getComments();
  }

  getComments() {
    this.commentService
      .getComments(this.route.snapshot.params.id)
      .subscribe((response) => {
        this.comments = response;
      });
  }

  intitializeRecipeForm() {
    this.commentForm = this.fb.group({
      body: ['', Validators.required],
    });
  }

  deleteRecipe(id: number) {
    this.commentService.deleteComment(id).subscribe(
      () => {
        this.comments.splice(
          this.comments.findIndex((p) => p.id === id),
          1
        );
        this.toastr.success('Recipe has been deleted');
      },
      (error) => {
        this.toastr.error('Deleting recipe has failed');
      }
    );
  }

  addComment() {
    this.commentService
      .addComment(this.commentForm.value, this.route.snapshot.params.id)
      .subscribe(
        (response) => {
          this.getComments();
          this.commentForm.reset();
        },
        (error) => {
          this.validationErrors = error;
        }
      );
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
