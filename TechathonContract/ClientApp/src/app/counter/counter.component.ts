import { Component } from '@angular/core';
import { Contract } from './Contract';
import { User } from './User';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public currentCount = 0;
  public document;
  public user;
  public access;
  public documentList:Contract;
  public userList:User;
  public 

  public incrementCounter() {
    this.currentCount++;
  }

  public updateControl(){
    console.log( this.document );
  }
}
