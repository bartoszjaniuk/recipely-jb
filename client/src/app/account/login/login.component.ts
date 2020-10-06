import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  registerMode = false;
  @Output() registerModeOutput = new EventEmitter<boolean>();

  registerToggle() {
    this.registerModeOutput.emit(true);
  }


  model: any = {};
  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model)
      .subscribe(response => {
          this.router.navigateByUrl('/members');
          this.toastr.success('Success!')
      }, error => {
        console.log(error);
        

      })
  }
}




