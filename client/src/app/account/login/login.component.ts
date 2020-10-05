import { Component, EventEmitter, OnInit, Output } from '@angular/core';

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
 

  constructor() { }

  ngOnInit(): void {
  }

}
