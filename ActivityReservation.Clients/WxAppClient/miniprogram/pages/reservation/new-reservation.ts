import { ReservationService } from '../../services/ReservationService';
import { ReservationPlaceService } from '../../services/ReservationPlaceService';
import { Reservation } from '../../models/Reservation';
import { ReservationPlace } from '../../models/ReservationPlace';
import { ReservationPeriod } from '../../models/ReservationPeriod';

import * as util from '../../utils/util';

const reservationSvc = new ReservationService();
const reservationPlaceSvc = new ReservationPlaceService();

Page({
  data: {
    stepIndex: 0,
    places: [] as Array<ReservationPlace>,
    placeNames: [] as Array<string>,
    currentDate: new Date().getTime(),
    maxDate: util.addDays(new Date(), 7).getTime(),
    reservation: new Reservation(),
    reservationPeriods: [] as Array<ReservationPeriod>,
    checkedPeriods: [] as Array<string>,
  },
  onLoad(params: any) {
    console.log(params);

    reservationPlaceSvc.GetAll((result) => {
      (<any>this).setData({
        places: result,
        placeNames: result.map(p => p.PlaceName)
      });
    })
  },
  onStepChange(): boolean {
    console.log(`stepIndex:${this.data.stepIndex},reservationInfo: ${JSON.stringify(this.data.reservation)}`);
    switch (this.data.stepIndex) {
      case 0:
        break;
      case 1:
        if (!this.data.reservation.ReservationPlaceId) {
          this.data.reservation.ReservationPlaceId = this.data.places[0].PlaceId;
          this.data.reservation.ReservationPlaceName = this.data.places[0].PlaceName;
        }
        break;
      case 2:
        if (!this.data.reservation.ReservationForDate) {
          this.data.reservation.ReservationForDate = util.formatDate(new Date());
        }
        //
        reservationPlaceSvc.getAvailablePeriods(result => {
          console.log(result);
          (<any>this).setData({
            reservationPeriods: result
          });
        }, this.data.reservation.ReservationPlaceId, this.data.reservation.ReservationForDate);

        break;

      case 3:
        console.log(this.data.reservationPeriods);
        this.data.reservation.ReservationForTimeIds = this.data.reservationPeriods.filter(_=>_.Checked).map(x=>x.PeriodIndex).join(",");
        this.data.reservation.ReservationForTime = this.data.reservationPeriods.filter(_=>_.Checked).map(x=> x.PeriodTitle).join(",");
    
        break;

      case 4:
        (<any>this).setData({
          reservation: this.data.reservation
        });
        break;
    }

    return false;
  },

  prevStep(event: any) {
    this.data.stepIndex--;
    if (this.onStepChange()) {

    }
    //
    (<any>this).setData({
      stepIndex: this.data.stepIndex
    });
  },
  nextStep(event: any) {
    this.data.stepIndex++;
    if (this.onStepChange()) {

    }
    (<any>this).setData({
      stepIndex: this.data.stepIndex
    });
  },

  onPlaceChange(event: any) {
    const { picker, value, index } = event.detail;
    this.data.reservation.ReservationPlaceId = this.data.places[index].PlaceId;
    this.data.reservation.ReservationPlaceName = this.data.places[index].PlaceName;
  },

  onDateInput(event: any) {
    let dateStr = util.formatDate(new Date(event.detail));
    console.log(`date: ${dateStr}`);
    this.data.reservation.ReservationForDate = dateStr;
  },

  onPeriodsChange(event: any) {
    console.log(event);

    let idxs = new Array<number>();
    for (let name of (event.detail as Array<string>)) {
      let idx = Number.parseInt(name.substr(7));      
      idxs.push(idx);
    }
    for(let p of this.data.reservationPeriods){
      if(idxs.indexOf(p.PeriodIndex) > -1){
        p.Checked = true;
      }
    }

    (<any>this).setData({
      checkedPeriods: event.detail
    });
  },

  onUnitChange(event: any) {
    console.log(event);
    this.data.reservation.ReservationUnit = event.detail;
  },
  onActivityContentChange(event: any) {
    console.log(event);
    this.data.reservation.ReservationActivityContent = event.detail;
  },
  onPersonNameChange(event: any) {
    console.log(event);
    this.data.reservation.ReservationPersonName = event.detail;
  },
  onPersonPhoneChange(event: any) {
    console.log(event);
    this.data.reservation.ReservationPersonPhone = event.detail;
  },

  submit(event: any) {
    reservationSvc.NewReservation(result => {
      console.log(result);
    }, this.data.reservation, 'None', '');
  }
})