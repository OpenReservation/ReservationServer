import { Notice } from '../../models/Notice';
// import * as wxParse from '../../wxParse/wxParse.js';
const WxParse = require('../../wxParse/wxParse.js');
import { NoticeService } from '../../services/NoticeService';
const noticeSvc = new NoticeService();

Page({
  data: {
    notices: new Notice(),
  },
  onLoad(params:any) {
    console.log(params);
    this.loadNotice(params.path);
  },


  loadNotice(path:string) {
    noticeSvc.GetDetails((result)=>{
      WxParse.wxParse('NoticeContent', 'html', result.NoticeContent, this);
      (<any>this).setData({
        notice: result
      });
    },path);
  }
})
