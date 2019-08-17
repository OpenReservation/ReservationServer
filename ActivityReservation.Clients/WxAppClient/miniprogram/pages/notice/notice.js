"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
Page({
    data: {
        notices: [],
        pageNum: 1,
        pageSize: 10,
        totalPage: 0,
        totalCount: 0
    },
    onLoad: function () {
        this.loadNotice(1, 10);
    },
    navToDetails: function (event) {
        var path = event.currentTarget.dataset.path;
        console.log(path);
        wx.navigateTo({
            url: "./notice-detail?path=" + path
        });
    },
    prevPage: function () {
        this.loadNotice(--this.data.pageNum, this.data.pageSize);
    },
    nextPage: function () {
        this.loadNotice(++this.data.pageNum, this.data.pageSize);
    },
    loadNotice: function (pageNum, pageSize) {
        if (pageNum === void 0) { pageNum = 1; }
        if (pageSize === void 0) { pageSize = 10; }
        var _this = this;
        wx.showLoading({
            title: "loading..."
        });
        wx.request({
            url: "https://reservation.weihanli.xyz/api/notice?pageNum=" + pageNum + "&pageSize=" + pageSize,
            success: function (res) {
                wx.hideLoading();
                console.log(res.data);
                var result = res.data;
                console.log("result:" + JSON.stringify(result));
                _this.setData({
                    notices: result.Data,
                    pageNum: result.PageNumber,
                    pageSize: result.PageSize,
                    totalPage: result.PageCount,
                    totalCount: result.TotalCount
                });
            }
        });
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibm90aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibm90aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O0FBSUEsSUFBSSxDQUFDO0lBQ0gsSUFBSSxFQUFFO1FBQ0osT0FBTyxFQUFFLEVBQW1CO1FBQzVCLE9BQU8sRUFBRSxDQUFDO1FBQ1YsUUFBUSxFQUFFLEVBQUU7UUFDWixTQUFTLEVBQUUsQ0FBQztRQUNaLFVBQVUsRUFBRSxDQUFDO0tBQ2Q7SUFDRCxNQUFNO1FBQ0osSUFBSSxDQUFDLFVBQVUsQ0FBQyxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUM7SUFDekIsQ0FBQztJQUVELFlBQVksRUFBWixVQUFhLEtBQVU7UUFDckIsSUFBSSxJQUFJLEdBQUcsS0FBSyxDQUFDLGFBQWEsQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDO1FBQzVDLE9BQU8sQ0FBQyxHQUFHLENBQUMsSUFBSSxDQUFDLENBQUM7UUFDbEIsRUFBRSxDQUFDLFVBQVUsQ0FBQztZQUNaLEdBQUcsRUFBRSwwQkFBd0IsSUFBTTtTQUNwQyxDQUFDLENBQUM7SUFDTCxDQUFDO0lBRUQsUUFBUTtRQUNOLElBQUksQ0FBQyxVQUFVLENBQUMsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO0lBQzNELENBQUM7SUFFRCxRQUFRO1FBQ04sSUFBSSxDQUFDLFVBQVUsQ0FBQyxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUM7SUFDM0QsQ0FBQztJQUVELFVBQVUsRUFBVixVQUFXLE9BQWtCLEVBQUUsUUFBb0I7UUFBeEMsd0JBQUEsRUFBQSxXQUFrQjtRQUFFLHlCQUFBLEVBQUEsYUFBb0I7UUFDakQsSUFBSSxLQUFLLEdBQUcsSUFBSSxDQUFDO1FBQ2pCLEVBQUUsQ0FBQyxXQUFXLENBQUM7WUFDYixLQUFLLEVBQUUsWUFBWTtTQUNwQixDQUFDLENBQUM7UUFDSCxFQUFFLENBQUMsT0FBTyxDQUFDO1lBQ1QsR0FBRyxFQUFFLHlEQUF1RCxPQUFPLGtCQUFhLFFBQVU7WUFDMUYsT0FBTyxFQUFFLFVBQUMsR0FBRztnQkFDWCxFQUFFLENBQUMsV0FBVyxFQUFFLENBQUM7Z0JBQ2pCLE9BQU8sQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLElBQUksQ0FBQyxDQUFBO2dCQUNyQixJQUFJLE1BQU0sR0FBMEIsR0FBRyxDQUFDLElBQUksQ0FBQztnQkFDN0MsT0FBTyxDQUFDLEdBQUcsQ0FBQyxZQUFVLElBQUksQ0FBQyxTQUFTLENBQUMsTUFBTSxDQUFHLENBQUMsQ0FBQztnQkFDMUMsS0FBTSxDQUFDLE9BQU8sQ0FBQztvQkFDbkIsT0FBTyxFQUFFLE1BQU0sQ0FBQyxJQUFJO29CQUNwQixPQUFPLEVBQUUsTUFBTSxDQUFDLFVBQVU7b0JBQzFCLFFBQVEsRUFBRSxNQUFNLENBQUMsUUFBUTtvQkFDekIsU0FBUyxFQUFFLE1BQU0sQ0FBQyxTQUFTO29CQUMzQixVQUFVLEVBQUUsTUFBTSxDQUFDLFVBQVU7aUJBQzlCLENBQUMsQ0FBQztZQUNMLENBQUM7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDO0NBRUYsQ0FBQyxDQUFBIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTm90aWNlIH0gZnJvbSAnLi4vLi4vbW9kZWxzL05vdGljZSc7XHJcbmltcG9ydCB7IFBhZ2VkTGlzdERhdGEgfSBmcm9tICcuLi8uLi9tb2RlbHMvUGFnZWRMaXN0RGF0YSc7XHJcbmltcG9ydCB7IGZvcm1hdFRpbWUgfSBmcm9tICcuLi8uLi91dGlscy91dGlsJztcclxuXHJcblBhZ2Uoe1xyXG4gIGRhdGE6IHtcclxuICAgIG5vdGljZXM6IFtdIGFzIEFycmF5PE5vdGljZT4sXHJcbiAgICBwYWdlTnVtOiAxLFxyXG4gICAgcGFnZVNpemU6IDEwLFxyXG4gICAgdG90YWxQYWdlOiAwLFxyXG4gICAgdG90YWxDb3VudDogMCAgICBcclxuICB9LFxyXG4gIG9uTG9hZCgpIHtcclxuICAgIHRoaXMubG9hZE5vdGljZSgxLCAxMCk7XHJcbiAgfSxcclxuXHJcbiAgbmF2VG9EZXRhaWxzKGV2ZW50OiBhbnkpIHtcclxuICAgIGxldCBwYXRoID0gZXZlbnQuY3VycmVudFRhcmdldC5kYXRhc2V0LnBhdGg7XHJcbiAgICBjb25zb2xlLmxvZyhwYXRoKTtcclxuICAgIHd4Lm5hdmlnYXRlVG8oe1xyXG4gICAgICB1cmw6IGAuL25vdGljZS1kZXRhaWw/cGF0aD0ke3BhdGh9YFxyXG4gICAgfSk7XHJcbiAgfSxcclxuXHJcbiAgcHJldlBhZ2UoKSB7XHJcbiAgICB0aGlzLmxvYWROb3RpY2UoLS10aGlzLmRhdGEucGFnZU51bSwgdGhpcy5kYXRhLnBhZ2VTaXplKTtcclxuICB9LFxyXG5cclxuICBuZXh0UGFnZSgpIHtcclxuICAgIHRoaXMubG9hZE5vdGljZSgrK3RoaXMuZGF0YS5wYWdlTnVtLCB0aGlzLmRhdGEucGFnZVNpemUpO1xyXG4gIH0sXHJcblxyXG4gIGxvYWROb3RpY2UocGFnZU51bTpudW1iZXIgPSAxLCBwYWdlU2l6ZTpudW1iZXIgPSAxMCkge1xyXG4gICAgbGV0IF90aGlzID0gdGhpcztcclxuICAgIHd4LnNob3dMb2FkaW5nKHtcclxuICAgICAgdGl0bGU6IFwibG9hZGluZy4uLlwiXHJcbiAgICB9KTtcclxuICAgIHd4LnJlcXVlc3Qoe1xyXG4gICAgICB1cmw6IGBodHRwczovL3Jlc2VydmF0aW9uLndlaWhhbmxpLnh5ei9hcGkvbm90aWNlP3BhZ2VOdW09JHtwYWdlTnVtfSZwYWdlU2l6ZT0ke3BhZ2VTaXplfWAsXHJcbiAgICAgIHN1Y2Nlc3M6IChyZXMpID0+IHtcclxuICAgICAgICB3eC5oaWRlTG9hZGluZygpO1xyXG4gICAgICAgIGNvbnNvbGUubG9nKHJlcy5kYXRhKS8vIOacjeWKoeWZqOWbnuWMheS/oeaBryBcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPFBhZ2VkTGlzdERhdGE8Tm90aWNlPj5yZXMuZGF0YTtcclxuICAgICAgICBjb25zb2xlLmxvZyhgcmVzdWx0OiR7SlNPTi5zdHJpbmdpZnkocmVzdWx0KX1gKTtcclxuICAgICAgICAoPGFueT5fdGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgICBub3RpY2VzOiByZXN1bHQuRGF0YSxcclxuICAgICAgICAgIHBhZ2VOdW06IHJlc3VsdC5QYWdlTnVtYmVyLFxyXG4gICAgICAgICAgcGFnZVNpemU6IHJlc3VsdC5QYWdlU2l6ZSxcclxuICAgICAgICAgIHRvdGFsUGFnZTogcmVzdWx0LlBhZ2VDb3VudCxcclxuICAgICAgICAgIHRvdGFsQ291bnQ6IHJlc3VsdC5Ub3RhbENvdW50XHJcbiAgICAgICAgfSk7IFxyXG4gICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG59KVxyXG4iXX0=