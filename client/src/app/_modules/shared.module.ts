import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ParticlesModule } from 'angular-particle';
import { ToastrModule } from 'ngx-toastr';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ParticlesModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    })
  ],

  exports: [
    BsDropdownModule,
    ParticlesModule,
    ToastrModule

  ]
})
export class SharedModule { }
