import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NoticeService } from '../../services/NoticeService';
import { LoadingService } from '../../services/LoadingService';
import { Notice } from '../../models/Notice';


@Component({
  selector: 'app-notice-detail',
  templateUrl: './notice-detail.component.html'
})
export class NoticeDetailComponent implements OnInit {

  notice: Notice;
  constructor(
    private route: ActivatedRoute,
    private svc: NoticeService,
    public loadingSvc: LoadingService,
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      let noticePath = params.get('noticePath');
      this.svc.GetDetails(noticePath)
      .subscribe(data => {
        this.notice = data;
        this.loadingSvc.isLoading = false;
      });
    });
  }

}
