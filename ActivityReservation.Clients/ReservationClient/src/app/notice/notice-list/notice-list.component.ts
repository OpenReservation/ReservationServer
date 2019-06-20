import { OnInit, Component } from '@angular/core';
import { Notice } from '../../models/Notice';
import { NoticeService } from '../../services/NoticeService';

@Component({
  selector: 'app-notice-list',
  templateUrl: './notice-list.component.html'
})
export class NoticeListComponent implements OnInit {

  public noticeList: Array<Notice>;
  public pageNumber = 1;
  public pageSize = 10;
  public total = 10;

  constructor(private svc: NoticeService) { }

  ngOnInit() {
        
    this.loadData();
  }

  private loadData(params?:object): void{
    this.svc.Get(params)
    .subscribe(data => {
      console.log(data);
      this.pageNumber = data.PageNumber;
      this.pageSize = data.PageSize;
      this.total = data.TotalCount;
      this.noticeList = data.Data;
    });
  }

  onPageEvent(pageParams){
    this.loadData({
      pageNumber: pageParams.pageIndex + 1,
      pageSize: pageParams.pageSize
    });
  }
}

