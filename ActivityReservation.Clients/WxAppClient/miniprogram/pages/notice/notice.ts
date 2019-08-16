import { Notice } from '../../models/Notice';
import { PagedListData } from '../../models/PagedListData';

Page({
  data: {
    notices: [] as Array<Notice>,
    pageNum: 1,
    pageSize: 10,
    totalPage: 0,    
  },
  onLoad() {
    this.loadNotice(this, 1, 10);
  },

  loadNotice(page: Page.PageInstance, pageNum:number = 1, pageSize:number = 10):void {
    wx.request({
      url: `https://reservation.weihanli.xyz/api/notice?pageNum=${pageNum}&pageSize=${pageSize}`,

      success: (res) => {
        console.log(res.data)// 服务器回包信息        
        let result = res.data as PagedListData<Notice>;
        if(null == result || undefined == result){
          console.log(`result as is null`);
          return;
        }
        console.log(`notice result: ${JSON.stringify(result)}`);
        page.setData({
          notices: result.Data,
          pageNum: result.PageNumber,
          pageSize: result.PageSize,
          totalPage: result.PageCount
        });        
      }
    });
  }
})
