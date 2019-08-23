"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Notice_1 = require("../../models/Notice");
var WxParse = require('../../wxParse/wxParse.js');
var NoticeService_1 = require("../../services/NoticeService");
var noticeSvc = new NoticeService_1.NoticeService();
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
        noticeSvc.GetDetails(function (result) {
            WxParse.wxParse('NoticeContent', 'html', result.NoticeContent, _this);
            _this.setData({
                notice: result
            });
        }, path);
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibm90aWNlLWRldGFpbC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIm5vdGljZS1kZXRhaWwudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7QUFBQSw4Q0FBNkM7QUFFN0MsSUFBTSxPQUFPLEdBQUcsT0FBTyxDQUFDLDBCQUEwQixDQUFDLENBQUM7QUFDcEQsOERBQTZEO0FBQzdELElBQU0sU0FBUyxHQUFHLElBQUksNkJBQWEsRUFBRSxDQUFDO0FBRXRDLElBQUksQ0FBQztJQUNILElBQUksRUFBRTtRQUNKLE9BQU8sRUFBRSxJQUFJLGVBQU0sRUFBRTtLQUN0QjtJQUNELE1BQU0sRUFBTixVQUFPLE1BQVU7UUFDZixPQUFPLENBQUMsR0FBRyxDQUFDLE1BQU0sQ0FBQyxDQUFDO1FBQ3BCLElBQUksQ0FBQyxVQUFVLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxDQUFDO0lBQy9CLENBQUM7SUFHRCxVQUFVLEVBQVYsVUFBVyxJQUFXO1FBQXRCLGlCQU9DO1FBTkMsU0FBUyxDQUFDLFVBQVUsQ0FBQyxVQUFDLE1BQU07WUFDMUIsT0FBTyxDQUFDLE9BQU8sQ0FBQyxlQUFlLEVBQUUsTUFBTSxFQUFFLE1BQU0sQ0FBQyxhQUFhLEVBQUUsS0FBSSxDQUFDLENBQUM7WUFDL0QsS0FBSyxDQUFDLE9BQU8sQ0FBQztnQkFDbEIsTUFBTSxFQUFFLE1BQU07YUFDZixDQUFDLENBQUM7UUFDTCxDQUFDLEVBQUMsSUFBSSxDQUFDLENBQUM7SUFDVixDQUFDO0NBQ0YsQ0FBQyxDQUFBIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTm90aWNlIH0gZnJvbSAnLi4vLi4vbW9kZWxzL05vdGljZSc7XHJcbi8vIGltcG9ydCAqIGFzIHd4UGFyc2UgZnJvbSAnLi4vLi4vd3hQYXJzZS93eFBhcnNlLmpzJztcclxuY29uc3QgV3hQYXJzZSA9IHJlcXVpcmUoJy4uLy4uL3d4UGFyc2Uvd3hQYXJzZS5qcycpO1xyXG5pbXBvcnQgeyBOb3RpY2VTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvTm90aWNlU2VydmljZSc7XHJcbmNvbnN0IG5vdGljZVN2YyA9IG5ldyBOb3RpY2VTZXJ2aWNlKCk7XHJcblxyXG5QYWdlKHtcclxuICBkYXRhOiB7XHJcbiAgICBub3RpY2VzOiBuZXcgTm90aWNlKCksXHJcbiAgfSxcclxuICBvbkxvYWQocGFyYW1zOmFueSkge1xyXG4gICAgY29uc29sZS5sb2cocGFyYW1zKTtcclxuICAgIHRoaXMubG9hZE5vdGljZShwYXJhbXMucGF0aCk7XHJcbiAgfSxcclxuXHJcblxyXG4gIGxvYWROb3RpY2UocGF0aDpzdHJpbmcpIHtcclxuICAgIG5vdGljZVN2Yy5HZXREZXRhaWxzKChyZXN1bHQpPT57XHJcbiAgICAgIFd4UGFyc2Uud3hQYXJzZSgnTm90aWNlQ29udGVudCcsICdodG1sJywgcmVzdWx0Lk5vdGljZUNvbnRlbnQsIHRoaXMpO1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBub3RpY2U6IHJlc3VsdFxyXG4gICAgICB9KTtcclxuICAgIH0scGF0aCk7XHJcbiAgfVxyXG59KVxyXG4iXX0=