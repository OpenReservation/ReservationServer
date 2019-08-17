import { Notice } from '../../models/Notice';
// import * as wxParse from '../../wxParse/wxParse.js';
const WxParse = require('../../wxParse/wxParse.js');

Page({
  data: {
    notices: new Notice(),
  },
  onLoad(params:any) {
    console.log(params);
    this.loadNotice(params.path);
  },


  loadNotice(path:string) {
    let _this = this;
    wx.showLoading({
      title: "loading..."
    });
    wx.request({
      url: `https://reservation.weihanli.xyz/api/notice/${path}`,
      success: (res) => {
        wx.hideLoading();
        console.log(res.data)// 服务器回包信息 
        let result = <Notice>res.data;
        
        WxParse.wxParse('NoticeContent', 'html', result.NoticeContent, _this);

        console.log(`result:${JSON.stringify(result)}`);
        (<any>_this).setData({
          notice: result
        });
      }
    });
  }

})
