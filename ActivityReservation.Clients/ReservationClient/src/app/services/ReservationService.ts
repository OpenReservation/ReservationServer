import { Injectable } from '@angular/core';
import { BaseService } from './BaseService';
import { Reservation } from '../models/Reservation';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ReservationService extends BaseService<Reservation>{

  constructor(http: HttpClient){
    super(http, 'Reservation');
  }

}
