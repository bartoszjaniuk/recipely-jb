import { Component, OnInit } from '@angular/core';
import { MembersService } from 'src/app/members/members.service';
import { IMember } from 'src/app/_models/member';
import { IPagination } from 'src/app/_models/pagination';

@Component({
  selector: 'app-like-lists',
  templateUrl: './like-lists.component.html',
  styleUrls: ['./like-lists.component.css']
})
export class LikeListsComponent implements OnInit {
  members: Partial<IMember[]>;
  predicate = 'liked'
  pageNumber = 1;
  pageSize = 6;
  pagination: IPagination;
  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }
  loadLikes() {
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe(response =>{
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadLikes();
  }

}
