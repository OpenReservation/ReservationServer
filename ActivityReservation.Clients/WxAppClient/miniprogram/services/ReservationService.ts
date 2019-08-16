
import { BaseService } from './BaseService';
import { Reservation } from '../models/Reservation';

export class ReservationService extends BaseService<Reservation>{

  constructor(){
    super('Reservation');
  }

  public NewReservation(reservation: Reservation, captchaType: string, captcha: string){
    // return this.http.post<any>(`${this.apiBaseUrl}/api/reservation`, reservation, {
    //   headers: {
    //     "captcha": captcha,
    //     "captchaType": captchaType
    //   }
    // });
  }
}
