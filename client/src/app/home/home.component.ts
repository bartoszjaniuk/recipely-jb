import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;
  constructor(public accountService: AccountService) { }

  ngOnInit() {
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
    // przełącznik
  }
  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}