"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Notice_1 = require("../../models/Notice");
var WxParse = require('../../wxParse/wxParse.js');
Page({
    data: {
        notices: new Notice_1.Notice(),
    },
    onLoad: function (params) {
        console.log(params);
        this.loadNotice(params.path);
    },
    loadNotice: function (path) {
        var _this = this;
        wx.showLoading({
            title: "loading..."
        });
        wx.request({
            url: "https://reservation.weihanli.xyz/api/notice/" + path,
            success: function (res) {
                wx.hideLoading();
                console.log(res.data);
                var result = res.data;
                WxParse.wxParse('NoticeContent', 'html', result.NoticeContent, _this);
                console.log("result:" + JSON.stringify(result));
                _this.setData({
                    notice: result
                });
            }
        });
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibm90aWNlLWRldGFpbC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIm5vdGljZS1kZXRhaWwudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7QUFBQSw4Q0FBNkM7QUFFN0MsSUFBTSxPQUFPLEdBQUcsT0FBTyxDQUFDLDBCQUEwQixDQUFDLENBQUM7QUFFcEQsSUFBSSxDQUFDO0lBQ0gsSUFBSSxFQUFFO1FBQ0osT0FBTyxFQUFFLElBQUksZUFBTSxFQUFFO0tBQ3RCO0lBQ0QsTUFBTSxFQUFOLFVBQU8sTUFBVTtRQUNmLE9BQU8sQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUM7UUFDcEIsSUFBSSxDQUFDLFVBQVUsQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDLENBQUM7SUFDL0IsQ0FBQztJQUdELFVBQVUsRUFBVixVQUFXLElBQVc7UUFDcEIsSUFBSSxLQUFLLEdBQUcsSUFBSSxDQUFDO1FBQ2pCLEVBQUUsQ0FBQyxXQUFXLENBQUM7WUFDYixLQUFLLEVBQUUsWUFBWTtTQUNwQixDQUFDLENBQUM7UUFDSCxFQUFFLENBQUMsT0FBTyxDQUFDO1lBQ1QsR0FBRyxFQUFFLGlEQUErQyxJQUFNO1lBQzFELE9BQU8sRUFBRSxVQUFDLEdBQUc7Z0JBQ1gsRUFBRSxDQUFDLFdBQVcsRUFBRSxDQUFDO2dCQUNqQixPQUFPLENBQUMsR0FBRyxDQUFDLEdBQUcsQ0FBQyxJQUFJLENBQUMsQ0FBQTtnQkFDckIsSUFBSSxNQUFNLEdBQVcsR0FBRyxDQUFDLElBQUksQ0FBQztnQkFFOUIsT0FBTyxDQUFDLE9BQU8sQ0FBQyxlQUFlLEVBQUUsTUFBTSxFQUFFLE1BQU0sQ0FBQyxhQUFhLEVBQUUsS0FBSyxDQUFDLENBQUM7Z0JBRXRFLE9BQU8sQ0FBQyxHQUFHLENBQUMsWUFBVSxJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU0sQ0FBRyxDQUFDLENBQUM7Z0JBQzFDLEtBQU0sQ0FBQyxPQUFPLENBQUM7b0JBQ25CLE1BQU0sRUFBRSxNQUFNO2lCQUNmLENBQUMsQ0FBQztZQUNMLENBQUM7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDO0NBRUYsQ0FBQyxDQUFBIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTm90aWNlIH0gZnJvbSAnLi4vLi4vbW9kZWxzL05vdGljZSc7XHJcbi8vIGltcG9ydCAqIGFzIHd4UGFyc2UgZnJvbSAnLi4vLi4vd3hQYXJzZS93eFBhcnNlLmpzJztcclxuY29uc3QgV3hQYXJzZSA9IHJlcXVpcmUoJy4uLy4uL3d4UGFyc2Uvd3hQYXJzZS5qcycpO1xyXG5cclxuUGFnZSh7XHJcbiAgZGF0YToge1xyXG4gICAgbm90aWNlczogbmV3IE5vdGljZSgpLFxyXG4gIH0sXHJcbiAgb25Mb2FkKHBhcmFtczphbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKHBhcmFtcyk7XHJcbiAgICB0aGlzLmxvYWROb3RpY2UocGFyYW1zLnBhdGgpO1xyXG4gIH0sXHJcblxyXG5cclxuICBsb2FkTm90aWNlKHBhdGg6c3RyaW5nKSB7XHJcbiAgICBsZXQgX3RoaXMgPSB0aGlzO1xyXG4gICAgd3guc2hvd0xvYWRpbmcoe1xyXG4gICAgICB0aXRsZTogXCJsb2FkaW5nLi4uXCJcclxuICAgIH0pO1xyXG4gICAgd3gucmVxdWVzdCh7XHJcbiAgICAgIHVybDogYGh0dHBzOi8vcmVzZXJ2YXRpb24ud2VpaGFubGkueHl6L2FwaS9ub3RpY2UvJHtwYXRofWAsXHJcbiAgICAgIHN1Y2Nlc3M6IChyZXMpID0+IHtcclxuICAgICAgICB3eC5oaWRlTG9hZGluZygpO1xyXG4gICAgICAgIGNvbnNvbGUubG9nKHJlcy5kYXRhKS8vIOacjeWKoeWZqOWbnuWMheS/oeaBryBcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPE5vdGljZT5yZXMuZGF0YTtcclxuICAgICAgICBcclxuICAgICAgICBXeFBhcnNlLnd4UGFyc2UoJ05vdGljZUNvbnRlbnQnLCAnaHRtbCcsIHJlc3VsdC5Ob3RpY2VDb250ZW50LCBfdGhpcyk7XHJcblxyXG4gICAgICAgIGNvbnNvbGUubG9nKGByZXN1bHQ6JHtKU09OLnN0cmluZ2lmeShyZXN1bHQpfWApO1xyXG4gICAgICAgICg8YW55Pl90aGlzKS5zZXREYXRhKHtcclxuICAgICAgICAgIG5vdGljZTogcmVzdWx0XHJcbiAgICAgICAgfSk7XHJcbiAgICAgIH1cclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbn0pXHJcbiJdfQ==