import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { MessageService } from 'src/app/messages/message.service';
import { PresenceService } from 'src/app/signalr/presence.service';
import { IMember } from 'src/app/_models/member';
import { IMessage } from 'src/app/_models/message';
import { IUser } from 'src/app/_models/user';
import { MembersService } from '../members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  activeTab: TabDirective;
  member: IMember;
  likers: Partial<IMember[]>;
  messages: IMessage[] = [];
  user: IUser;
  constructor(public presence: PresenceService, private route: ActivatedRoute,
     private messageService: MessageService, private accountService: AccountService, private router: Router, private memberService: MembersService) { 
       this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
       this.router.routeReuseStrategy.shouldReuseRoute = () => false;
     }
 

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.member = data.member;
    })
    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })
    this.messageService.createHubConnection(this.user, this.member.username);
    console.log(this.member.lastActive)
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.messageService.createHubConnection(this.user, this.member.username);
    } else {
      this.messageService.stopHubConnection();
    }
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }


}

