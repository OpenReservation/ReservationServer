import { Injectable } from '@angular/core';
import { BaseService } from './BaseService';
import { HttpClient } from '@angular/common/http';
import { ReservationPlace } from '../models/ReservationPlace';

@Injectable({
  providedIn: 'root'
})
export class ReservationPlaceService extends BaseService<ReservationPlace>{

  constructor(http: HttpClient){
    super(http, 'ReservationPlace');
  }

}
