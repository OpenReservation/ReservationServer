import { ReservationService } from '../../services/reservationService';
import { ReservationPlaceService } from '../../services/reservationPlaceService';
import { Reservation } from '../../models/reservation';
import { ReservationPlace } from '../../models/reservationPlace';

const reservationSvc = new ReservationService();
const reservationPlaceSvc = new ReservationPlaceService();

Page({
  data: {
    steps: [
      {
        text: '选择活动室',
        desc: '选择要预约的活动室'
      },
      {
        text: '选择日期',
        desc: '选择要预约的日期'
      },
      {
        text: '选择时间段',
        desc: '选择要预约的时间段'
      },
      {
        text: '预约信息',
        desc: '填写预约信息'
      },
      {
        text: '确认预约',
        desc: '确认要预约的信息'
      }
    ],
    stepIndex: 0,
    places: [] as Array<ReservationPlace>
  },
  onLoad(params: any) {
    console.log(params);
    reservationPlaceSvc.GetAll((result)=>{
      (<any>this).setData({
        places: result
      });
    })
  },
  prevStep(event: any) {
    (<any>this).setData({
      stepIndex: --this.data.stepIndex
    });
  },
  nextStep(event:any){
    (<any>this).setData({
      stepIndex: ++this.data.stepIndex
    });
  },
  submit(event: any){

  }
})