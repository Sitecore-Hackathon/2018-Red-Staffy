import { Component, OnInit } from '@angular/core';
import { NgScService } from '@speak/ng-sc';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(
    private ngScService: NgScService
  ) {}

  ngOnInit() {
    this.ngScService.init();
  }
}
