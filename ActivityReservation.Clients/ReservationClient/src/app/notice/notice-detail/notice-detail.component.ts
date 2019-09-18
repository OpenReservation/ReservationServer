import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NoticeService } from '../../services/NoticeService';
import { LoadingService } from '../../services/LoadingService';
import { Notice } from '../../models/Notice';
import { Router } from '@angular/router';

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
    private router: Router
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const noticePath = params.get('noticePath');
      this.svc.GetDetails(noticePath)
        .subscribe(data => {
          this.notice = data;
          this.loadingSvc.isLoading = false;
        }, err => {
          console.error(err);
          this.router.navigateByUrl('/notice');
        });
    });
  }

}
