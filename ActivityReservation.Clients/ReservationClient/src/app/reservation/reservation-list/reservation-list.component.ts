import { Component, OnInit } from '@angular/core';
import { PagedListData } from '../../models/PagedListData';
import { Reservation } from '../../models/Reservation';
import { ReservationService } from '../../services/ReservationService';
import { ColumnInfo } from 'src/app/models/ColumnInfo';

@Component({
  selector: 'app-reservation-list',
  templateUrl: './reservation-list.component.html',
  styleUrls: ['./reservation-list.component.less']
})
export class ReservationListComponent implements OnInit {

  public reservations: Array<Reservation>;
  public pageNumber = 1;
  public pageSize = 10;
  public total = 10;

  columns: Array<ColumnInfo> = [
    { 
      ColumnName: 'ReservationPlaceName',
      DisplayName: "活动室名称" 
    },
    { 
      ColumnName: 'ReservationForDate',
      DisplayName: "预约使用日期" 
    }, 
    { 
      ColumnName: 'ReservationForTime',
      DisplayName: "预约使用时间" 
    }, 
    { 
      ColumnName: 'ReservationPersonName',
      DisplayName: "预约人名称" 
    },
    { 
      ColumnName: 'ReservationPersonPhone',
      DisplayName: "预约人手机号" 
    },
    { 
      ColumnName: 'ReservationUnit',
      DisplayName: "预约单位" 
    },
    { 
      ColumnName: 'ReservationActivityContent',
      DisplayName: "活动内容" 
    },
    { 
      ColumnName:'ReservationTime',
      DisplayName: "预约时间" 
    }
  ];

  displayedColumns: Array<string>;

  constructor(private svc: ReservationService) {
    this.displayedColumns = this.columns.map(c=>c.ColumnName);
   }

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
