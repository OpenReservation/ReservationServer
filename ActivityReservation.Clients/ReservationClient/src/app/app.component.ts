import { Component } from '@angular/core';
import { MenuItem } from './models/MenuItem';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = '活动室预约系统';  

  menus: Array<MenuItem> = [
    { Title: "首页", Link: "/" },
    { Title: "预约", Link: "/reservation/new"},
    { Title: "公告", Link: "/notice" },
    { Title: "关于", Link: "/about" },
  ];
}
