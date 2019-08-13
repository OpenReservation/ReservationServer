Page({
  data: {
    logs: [] as string[]
  },
  onLoad() {
    this.loadNotice();
  },

  loadNotice(pageNum:number = 1, pageSize:number = 10):void {
    wx.request({
      url: `https://reservation.weihanli.xyz/api/notice?pageNum=${pageNum}&pageSize=${pageSize}`,

      success: function (res) {
        console.log(res)// 服务器回包信息        
      }
    })
  }
})
