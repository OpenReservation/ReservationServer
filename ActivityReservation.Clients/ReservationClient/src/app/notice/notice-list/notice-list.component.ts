import { OnInit, Component } from '@angular/core';
import { Notice } from '../../models/Notice';
import { NoticeService } from '../../services/NoticeService';
import { LoadingService } from '../../services/LoadingService';

@Component({
  selector: 'app-notice-list',
  templateUrl: './notice-list.component.html'
})
export class NoticeListComponent implements OnInit {

  public noticeList: Array<Notice>;
  public pageNumber = 1;
  public pageSize = 10;
  public total = 10;

  constructor(private svc: NoticeService, private loadingSvc: LoadingService) { }

  ngOnInit() {        
    this.loadData();
  }

  private loadData(params?:object): void{
    if(this.loadingSvc.isLoading === false){
      this.loadingSvc.isLoading = true;
    }
    this.svc.Get(params)
    .subscribe(data => {
      this.pageNumber = data.PageNumber;
      this.pageSize = data.PageSize;
      this.total = data.TotalCount;
      this.noticeList = data.Data;
      this.loadingSvc.isLoading = false;
    });
  }

  onPageEvent(pageParams){
    this.loadData({
      pageNumber: pageParams.pageIndex + 1,
      pageSize: pageParams.pageSize
    });
  }
}

