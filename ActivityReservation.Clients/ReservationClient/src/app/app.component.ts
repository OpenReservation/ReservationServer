import { Component } from '@angular/core';
import { MenuItem } from './models/MenuItem';
import { Router, NavigationStart } from '@angular/router'; 
import { LoadingService } from './services/LoadingService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  constructor(public loadingSvc: LoadingService, private router:Router) { 
    this.loadingSvc.isLoading = true;
    this.router.events.subscribe((e) => { if(e instanceof NavigationStart) {this.loadingSvc.isLoading = true;} });
  }

  title = 'OpenReservation';  
  year = new Date().getFullYear();

  menus: Array<MenuItem> = [
    { Title: "首页", Link: "/" },
    { Title: "预约", Link: "/reservation/new"},
    { Title: "公告", Link: "/notice" },
    { Title: "关于", Link: "/about" },
  ];
}
