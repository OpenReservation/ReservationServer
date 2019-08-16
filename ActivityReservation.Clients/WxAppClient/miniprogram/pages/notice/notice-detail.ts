import { Notice } from '../../models/Notice';
import { PagedListData } from '../../models/PagedListData';

Page({
  data: {
    notices: new Notice(),
    pageNum: 1,
    pageSize: 10,
    totalPage: 0,
    totalCount: 0
  },
  onLoad() {
    this.loadNotice('aa');
  },

  loadNotice(pageNum: number = 1, pageSize: number = 10) {
    let _this = this;
    wx.showLoading({
      title: "loading..."
    });
    let path ='';
    wx.request({
      url: `https://reservation.weihanli.xyz/api/notice/${path}`,
      success: (res) => {
        wx.hideLoading();
        console.log(res.data)// 服务器回包信息 
        let result = <Notice>res.data;
        console.log(`result:${JSON.stringify(result)}`);
        (<any>_this).setData({
          notice: result
        });
      }
    });
  }

})
