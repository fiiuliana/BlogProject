import { Component, Input, OnInit } from '@angular/core';
import { BlogCommentViewModel } from 'src/app/models/blog-comment/blog-comment-view-model.model';
import { BlogComment } from 'src/app/models/blog-comment/blog-comment.model';
import { AccountService } from 'src/app/services/account.service';
import { BlogCommentService } from 'src/app/services/blog-comment.service';

@Component({
  selector: 'app-comment-system',
  templateUrl: './comment-system.component.html',
  styleUrls: ['./comment-system.component.css']
})
export class CommentSystemComponent implements OnInit {

  @Input() blogId: number;

  standAloneComment: BlogCommentViewModel;
  blogComments: BlogComment[];
  blogCommentViewModels: BlogCommentViewModel[];

  constructor(
    private blogCommentService: BlogCommentService,
    public accountService: AccountService) { }

  ngOnInit(): void {
    this.blogCommentService.getAll(this.blogId).subscribe(blogComments => {

      if (this.accountService.isLoggedIn()) {
        this.initComment(this.accountService.currentUserValue.username);
      }

      this.blogComments = blogComments;
      this.blogCommentViewModels = [];

      for (let i=0; i<this.blogComments.length; i++) {
        if (!this.blogComments[i].parentBlogCommentId) {
          this.findCommentReplies(this.blogCommentViewModels, i);
        }
      }

    });
  }

  findCommentReplies(blogCommentViewModels: BlogCommentViewModel[], i: number) {
    throw new Error('Method not implemented.');
  }

  
  initComment(username: string) {
    throw new Error('Method not implemented.');
  }

}
