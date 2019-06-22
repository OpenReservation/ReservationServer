import { Injectable } from '@angular/core';
import { BaseService } from './BaseService';
import { HttpClient } from '@angular/common/http';
import { ReservationPlace } from '../models/ReservationPlace';
import { Observable } from 'rxjs';
import { ReservationPeriod } from '../models/ReservationPeriod';

@Injectable({
  providedIn: 'root'
})
export class ReservationPlaceService extends BaseService<ReservationPlace>{

  constructor(http: HttpClient){
    super(http, 'ReservationPlace');
  }

  public getAvailablePeriods(placeId:string, date: string) : Observable<Array<ReservationPeriod>>{
    return this.http.get<Array<ReservationPeriod>>(`${this.apiBaseUrl}/api/reservationPlace/${placeId}/periods?dt=${date}`);
  }

}
