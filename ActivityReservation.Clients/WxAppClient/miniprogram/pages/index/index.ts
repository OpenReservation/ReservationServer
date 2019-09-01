//index.js
//获取应用实例
import { IMyApp } from '../../app';
import { Reservation } from '../../models/Reservation';
import { ReservationService } from '../../services/ReservationService';

const app = getApp<IMyApp>()
const reservationSvc = new ReservationService();

Page({
  data: {
    userInfo: {},
    hasUserInfo: false,
    canIUse: wx.canIUse('button.open-type.getUserInfo'),
    pageNum: 1,
    pageSize: 8,
    totalPage: 1,
    totalCount: -1,
    reservations: [] as Array<Reservation>
  },
  onLoad() {
    console.log(`onLoad`);
    // this.loadReservation(1, 10);
    // if (app.globalData.userInfo) {
    //   this.setData!({
    //     userInfo: app.globalData.userInfo,
    //     hasUserInfo: true,
    //   })
    // } else if (this.data.canIUse){
    //   // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
    //   // 所以此处加入 callback 以防止这种情况
    //   app.userInfoReadyCallback = (res) => {
    //     this.setData!({
    //       userInfo: res,
    //       hasUserInfo: true
    //     })
    //   }
    // } else {
    //   // 在没有 open-type=getUserInfo 版本的兼容处理
    //   wx.getUserInfo({
    //     success: res => {
    //       app.globalData.userInfo = res.userInfo
    //       this.setData!({
    //         userInfo: res.userInfo,
    //         hasUserInfo: true
    //       })
    //     }
    //   })
    // }
  },

  onShow(){
    console.log(`onShow`);
    this.loadReservation(1, this.data.pageSize);
  },

  prevPage() {
    this.loadReservation(--this.data.pageNum, this.data.pageSize);
  },

  nextPage() {
    this.loadReservation(++this.data.pageNum, this.data.pageSize);
  },

  loadReservation(pageNum:number, pageSize:number = 10){
    reservationSvc.Get((result)=>{
      console.log(result);
      (<any>this).setData({
        pageNum: result.PageNumber,
        pageSize: result.PageSize,
        totalPage: result.PageCount,
        totalCount: result.TotalCount,
        reservations: result.Data
      });
    }, {
      pageNumber: pageNum,
      pageSize: pageSize
    })
  },
  getUserInfo(e: any) {
    console.log(e)
    app.globalData.userInfo = e.detail.userInfo
    this.setData!({
      userInfo: e.detail.userInfo,
      hasUserInfo: true
    })
  }
})
