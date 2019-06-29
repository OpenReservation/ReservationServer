import { Component, OnInit } from '@angular/core';
import { Reservation } from '../../models/Reservation';
import { ReservationService } from '../../services/ReservationService';
import { LoadingService } from '../../services/LoadingService';
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
      ColumnName: 'ReservationUnit',
      DisplayName: "预约单位" 
    },
    { 
      ColumnName: 'ReservationActivityContent',
      DisplayName: "活动内容" 
    },
    { 
      ColumnName: 'ReservationPersonName',
      DisplayName: "预约人名称" 
    },
    { 
      ColumnName:'ReservationTime',
      DisplayName: "预约时间" 
    }
  ];

  displayedColumns: Array<string>;

  constructor(private svc: ReservationService, private loadingSvc: LoadingService) {
    this.displayedColumns = this.columns.map(c=>c.ColumnName);
   }

  ngOnInit() {    
    this.loadData();
  }

  private loadData(params?:object): void{
    if(this.loadingSvc.isLoading === false){
      this.loadingSvc.isLoading = true;
    }
    this.svc.Get(params)
    .subscribe(data => {
      console.log(data);
      this.pageNumber = data.PageNumber;
      this.pageSize = data.PageSize;
      this.total = data.TotalCount;
      this.reservations = data.Data;
      
      // 修改 LoadingService 的 isLoading
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
