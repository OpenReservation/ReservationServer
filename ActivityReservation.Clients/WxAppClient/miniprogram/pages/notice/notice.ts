import { Notice } from '../../models/Notice';
import { PagedListData } from '../../models/PagedListData';
import { formatTime } from '../../utils/util';

Page({
  data: {
    notices: [] as Array<Notice>,
    pageNum: 1,
    pageSize: 10,
    totalPage: 0,
    totalCount: 0    
  },
  onLoad() {
    this.loadNotice(1, 10);
  },

  loadNext(){
    this.data.pageNum++;
    this.loadNotice(this.data.pageNum, this.data.pageSize);
  },

  loadNotice(pageNum:number = 1, pageSize:number = 10) {
    let _this = this;
    wx.showLoading({
      title: "loading..."
    });
    wx.request({
      url: `https://reservation.weihanli.xyz/api/notice?pageNum=${pageNum}&pageSize=${pageSize}`,
      success: (res) => {
        wx.hideLoading();
        console.log(res.data)// 服务器回包信息 
        let result = <PagedListData<Notice>>res.data;
        console.log(`result:${JSON.stringify(result)}`);
        (<any>_this).setData({
          notices: result.Data,
          pageNum: result.PageNumber,
          pageSize: result.PageSize,
          totalPage: result.PageCount,
          totalCount: result.TotalCount
        }); 
      }
    });
  }

})
