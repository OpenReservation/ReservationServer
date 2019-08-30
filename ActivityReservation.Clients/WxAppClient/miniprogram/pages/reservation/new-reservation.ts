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
    minDate: new Date().getTime(),
    maxDate: util.addDays(new Date(), 7).getTime(),
    reservation: new Reservation(),
    reservationPeriods: [] as Array<ReservationPeriod>,
    checkedPeriods: [] as Array<string>,
    
    selectedPlaceIndex: 0,

    unitErr: "",
    acContentErr: "",
    pNameErr: "",
    pPhoneErr: ""
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
            reservationPeriods: result,
            checkedPeriods: []
          });
        }, this.data.reservation.ReservationPlaceId, this.data.reservation.ReservationForDate);

        break;

      case 3:
        if(this.data.checkedPeriods.length == 0){
          wx.showToast({
            title: "请选择要预约的时间段或返回上一步选择其他预约日期",
            duration: 2000,
            icon: "none"
          });
          this.data.stepIndex--;
          return false;
        }
        console.log(this.data.reservationPeriods);
        this.data.reservation.ReservationForTimeIds = this.data.reservationPeriods.filter(_=>_.Checked).map(x=>x.PeriodIndex).join(",");
        this.data.reservation.ReservationForTime = this.data.reservationPeriods.filter(_=>_.Checked).map(x=> x.PeriodTitle).join(",");
    
        break;

      case 4:
        if(this.validateInputParams()){
          (<any>this).setData({
            reservation: this.data.reservation
          });
        }else{
          this.data.stepIndex--;
          return false;
        }
        
        break;
    }

    return true;
  },

  prevStep(event: any) {
    this.data.stepIndex--;
    this.onStepChange();
    //
    (<any>this).setData({
      stepIndex: this.data.stepIndex
    });
  },
  nextStep(event: any) {
    this.data.stepIndex++;
    this.onStepChange();
    (<any>this).setData({
      stepIndex: this.data.stepIndex
    });
  },

  onPlaceChange(event: any) {
    const { picker, value, index } = event.detail;
    (<any>this).setData({
      selectedPlaceIndex: index
    });
    this.data.reservation.ReservationPlaceId = this.data.places[index].PlaceId;
    this.data.reservation.ReservationPlaceName = this.data.places[index].PlaceName;
  },

  onDateInput(event: any) {
    (<any>this).setData({
      currentDate: event.detail
    });
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
      }else{
        p.Checked = false;
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

  validateInputParams(): boolean{
    if(!this.data.reservation.ReservationUnit){
      (<any>this).setData({
        unitErr: "预约单位不能为空"
      });
      return false;
    }
    if(this.data.reservation.ReservationUnit.length < 2 || this.data.reservation.ReservationUnit.length > 16){
      (<any>this).setData({
        unitErr: "预约单位长度需要在 2 与 16 之间"
      });
      return false;
    }
    if(this.data.unitErr){
      (<any>this).setData({
        unitErr: ""
      });
    }

    if(!this.data.reservation.ReservationActivityContent){
      (<any>this).setData({
        acContentErr: "活动内容不能为空"
      });
      return false;
    }
    if(this.data.reservation.ReservationActivityContent.length < 2 || this.data.reservation.ReservationActivityContent.length > 16){
      (<any>this).setData({
        acContentErr: "活动内容长度需要在 2 与 16 之间"
      });
      return false;
    }
    if(this.data.acContentErr){
      (<any>this).setData({
        acContentErr: ""
      });
    }

    if(!this.data.reservation.ReservationPersonName){
      (<any>this).setData({
        pNameErr: "预约人名称不能为空"
      });
      return false;
    }
    if(this.data.reservation.ReservationPersonName.length < 2 || this.data.reservation.ReservationPersonName.length > 16){
      (<any>this).setData({
        pNameErr: "预约人名称长度需要在 2 与 16 之间"
      });
      return false;
    }
    if(this.data.pNameErr){
      (<any>this).setData({
        pNameErr: ""
      });
    }

    if(!this.data.reservation.ReservationPersonPhone){
      (<any>this).setData({
        pPhoneErr: "预约人手机号不能为空"
      });
      return false;
    }
    if(!/^1[3-9]\d{9}$/.test(this.data.reservation.ReservationPersonPhone)){
      (<any>this).setData({
        pPhoneErr: "预约人手机号不合法"
      });
      return false;
    }
    if(this.data.pPhoneErr){
      (<any>this).setData({
        pPhoneErr: ""
      });
    }
    return true;
  },

  submit(event: any) {
    // validate param name
    if(!this.validateInputParams()){
      return;
    }
    //
    reservationSvc.NewReservation(result => {
      console.log(result);
      if(result.Status == 200){
        wx.showToast({
          title: "预约成功",
          icon: "success",
          duration: 2000,
          complete: res => {
            wx.reLaunch({
              url: '/pages/index/index'
            });
          }
        });        
      } else {
        wx.showToast({
          title: `预约失败,${result.ErrorMsg}`,
          icon: "none",
          duration: 2000
        });
      }
    }, this.data.reservation, 'None', '');
  }
})