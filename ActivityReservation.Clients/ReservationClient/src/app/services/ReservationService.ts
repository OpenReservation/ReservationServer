import { Injectable } from '@angular/core';
import { BaseService } from './BaseService';
import { Reservation } from '../models/Reservation';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfigService } from './ConfigService';

@Injectable({
  providedIn: 'root'
})
export class ReservationService extends BaseService<Reservation>{

  constructor(http: HttpClient, config: ConfigService){
    super(http, config, 'Reservation');
  }

  public NewReservation(reservation: Reservation, captchaType: string, captcha: string): Observable<any>{
    return this.http.post<any>(`${this.apiBaseUrl}/api/reservation`, reservation, {
      headers: {
        "captcha": captcha,
        "captchaType": captchaType
      }
    });
  }
}
