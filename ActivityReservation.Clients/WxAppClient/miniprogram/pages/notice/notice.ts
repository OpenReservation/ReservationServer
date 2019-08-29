import { Notice } from '../../models/Notice';
import { PagedListData } from '../../models/PagedListData';
import { formatTime } from '../../utils/util';
import { NoticeService } from '../../services/NoticeService';

Page({
  data: {
    notices: [] as Array<Notice>,
    pageNum: 1,
    pageSize: 10,
    totalPage: 0,
    totalCount: 0    
  },
  onShow() {
    this.loadNotice(1, 10);
  },
  noticeService: new NoticeService(),
  navToDetails(event: any) {
    let path = event.currentTarget.dataset.path;
    console.log(path);
    wx.navigateTo({
      url: `./notice-detail?path=${path}`
    });
  },

  prevPage() {
    this.loadNotice(--this.data.pageNum, this.data.pageSize);
  },

  nextPage() {
    this.loadNotice(++this.data.pageNum, this.data.pageSize);
  },

  loadNotice(pageNum:number = 1, pageSize:number = 10) {
    let _this = this;
    wx.showLoading({
      title: "loading..."
    });
    this.noticeService.Get(result => {
        (<any>_this).setData({
          notices: result.Data,
          pageNum: result.PageNumber,
          pageSize: result.PageSize,
          totalPage: result.PageCount,
          totalCount: result.TotalCount
        });
    }, {
      PageNumber:pageNum,
      pageSize: pageSize
    });    
  }
})
