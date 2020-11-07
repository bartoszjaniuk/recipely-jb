import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MessageService } from 'src/app/messages/message.service';
import { IMessage } from 'src/app/_models/message';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() username: string;
  messages: IMessage[];
  messageContent: string;
  constructor(public messageService: MessageService) { }

  ngOnInit(): void {}

  loadMessages() {
    this.messageService.getMessageThread(this.username).subscribe(messages => {
      this.messages = messages;
    })
  }

  // sendMessage() {
  //   this.messageService.sendMessage(this.username, this.messageContent).subscribe(message => {
  //     this.messages.push(message);
  //     this.messageForm.reset();
  //   })
  // }

  sendMessage() {
    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
      this.messageForm.reset();
    })
  }

}
