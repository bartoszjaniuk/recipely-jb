import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  model: any = {};
  isCollapsed = true;
  constructor(public accountService: AccountService, private router: Router) {}

  ngOnInit(): void {}

  logOut() {
    this.accountService.logOut();
    this.router.navigate(['/']);
  }
}
