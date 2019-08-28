
import { BaseService } from './BaseService';
import { Reservation } from '../models/Reservation';

export class ReservationService extends BaseService<Reservation>{

  constructor(){
    super('Reservation');
  }

  public NewReservation(callback:(result:any)=>void,reservation: Reservation, captchaType: string, captcha: string){
    wx.showLoading({
      title: "loading..."      
    });
    let url = `${this.apiBaseUrl}/api/reservation`;
    wx.request({
      url: url,
      method: "POST",
      header:{
        "captchaType": captchaType,
        "captcha": captcha,
        "Content-Type": "application/json"
      },
      data: reservation,
      dataType: "json",
      success: (response) => {
        wx.hideLoading();
        let result = <any>response.data;        
        if(result.Status == 200){
          wx.showToast({
            title: "预约成功",
            icon: "success",
            duration: 2000
          });
        }else{
          wx.showToast({
            title: `预约失败,${result.ErrorMsg}`,
            icon: "none",
            duration: 2000,
          });
        }
        callback(result);
      }
    });
  }
}
