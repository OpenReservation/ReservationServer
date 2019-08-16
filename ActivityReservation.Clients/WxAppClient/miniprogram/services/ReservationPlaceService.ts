import { BaseService } from './BaseService';
import { ReservationPlace } from '../models/ReservationPlace';
import { ReservationPeriod } from '../models/ReservationPeriod';

export class ReservationPlaceService extends BaseService<ReservationPlace>{

  constructor(){
    super('ReservationPlace');
  }

  public getAvailablePeriods(placeId:string, date: string) {
    //return this.http.get<Array<ReservationPeriod>>(`${this.apiBaseUrl}/api/reservationPlace/${placeId}/periods?dt=${date}`);
  }

}
