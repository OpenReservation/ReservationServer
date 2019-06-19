import { Component, OnInit } from '@angular/core';
import { PagedListData } from '../../models/PagedListData';
import { Reservation } from '../../models/Reservation';
import { ReservationService } from '../../services/ReservationService';

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

  displayedColumns: string[] = [
    'ReservationForDate', 
    'ReservationForTime', 
    'ReservationPlaceName',
    'ReservationPersonName',
    'ReservationPersonPhone',
    'ReservationUnit',
    'ReservationActivityContent',
    'ReservationTime'
  ];

  constructor(private svc: ReservationService) { }

  ngOnInit() {
    
    this.svc.Get()
    .subscribe(data => {
      console.log(data);
      this.pageNumber = data.PageNumber;
      this.pageSize = data.PageSize;
      this.total = data.TotalCount;
      this.reservations = data.Data;
    });
  }

}
