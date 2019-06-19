import { Component, OnInit } from '@angular/core';
import { NoticeService } from '../../services/NoticeService';

@Component({
  selector: 'app-notice-detail',
  templateUrl: './notice-detail.component.html'
})
export class NoticeDetailComponent implements OnInit {

  constructor(private svc: NoticeService) { }

  ngOnInit() {
  }

}
