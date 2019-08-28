"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var BaseService_1 = require("./BaseService");
var ReservationService = (function (_super) {
    __extends(ReservationService, _super);
    function ReservationService() {
        return _super.call(this, 'Reservation') || this;
    }
    ReservationService.prototype.NewReservation = function (callback, reservation, captchaType, captcha) {
        wx.showLoading({
            title: "loading..."
        });
        var url = this.apiBaseUrl + "/api/reservation";
        wx.request({
            url: url,
            method: "POST",
            header: {
                "captchaType": captchaType,
                "captcha": captcha,
                "Content-Type": "application/json"
            },
            data: reservation,
            dataType: "json",
            success: function (response) {
                wx.hideLoading();
                var result = response.data;
                if (result.Status == 200) {
                    wx.showToast({
                        title: "预约成功",
                        icon: "success",
                        duration: 2000
                    });
                }
                else {
                    wx.showToast({
                        title: "\u9884\u7EA6\u5931\u8D25," + result.ErrorMsg,
                        icon: "none",
                        duration: 2000,
                    });
                }
                callback(result);
            }
        });
    };
    return ReservationService;
}(BaseService_1.BaseService));
exports.ReservationService = ReservationService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmVzZXJ2YXRpb25TZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiUmVzZXJ2YXRpb25TZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7Ozs7OztBQUNBLDZDQUE0QztBQUc1QztJQUF3QyxzQ0FBd0I7SUFFOUQ7ZUFDRSxrQkFBTSxhQUFhLENBQUM7SUFDdEIsQ0FBQztJQUVNLDJDQUFjLEdBQXJCLFVBQXNCLFFBQTJCLEVBQUMsV0FBd0IsRUFBRSxXQUFtQixFQUFFLE9BQWU7UUFDOUcsRUFBRSxDQUFDLFdBQVcsQ0FBQztZQUNiLEtBQUssRUFBRSxZQUFZO1NBQ3BCLENBQUMsQ0FBQztRQUNILElBQUksR0FBRyxHQUFNLElBQUksQ0FBQyxVQUFVLHFCQUFrQixDQUFDO1FBQy9DLEVBQUUsQ0FBQyxPQUFPLENBQUM7WUFDVCxHQUFHLEVBQUUsR0FBRztZQUNSLE1BQU0sRUFBRSxNQUFNO1lBQ2QsTUFBTSxFQUFDO2dCQUNMLGFBQWEsRUFBRSxXQUFXO2dCQUMxQixTQUFTLEVBQUUsT0FBTztnQkFDbEIsY0FBYyxFQUFFLGtCQUFrQjthQUNuQztZQUNELElBQUksRUFBRSxXQUFXO1lBQ2pCLFFBQVEsRUFBRSxNQUFNO1lBQ2hCLE9BQU8sRUFBRSxVQUFDLFFBQVE7Z0JBQ2hCLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztnQkFDakIsSUFBSSxNQUFNLEdBQVEsUUFBUSxDQUFDLElBQUksQ0FBQztnQkFDaEMsSUFBRyxNQUFNLENBQUMsTUFBTSxJQUFJLEdBQUcsRUFBQztvQkFDdEIsRUFBRSxDQUFDLFNBQVMsQ0FBQzt3QkFDWCxLQUFLLEVBQUUsTUFBTTt3QkFDYixJQUFJLEVBQUUsU0FBUzt3QkFDZixRQUFRLEVBQUUsSUFBSTtxQkFDZixDQUFDLENBQUM7aUJBQ0o7cUJBQUk7b0JBQ0gsRUFBRSxDQUFDLFNBQVMsQ0FBQzt3QkFDWCxLQUFLLEVBQUUsOEJBQVEsTUFBTSxDQUFDLFFBQVU7d0JBQ2hDLElBQUksRUFBRSxNQUFNO3dCQUNaLFFBQVEsRUFBRSxJQUFJO3FCQUNmLENBQUMsQ0FBQztpQkFDSjtnQkFDRCxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7WUFDbkIsQ0FBQztTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7SUFDSCx5QkFBQztBQUFELENBQUMsQUF6Q0QsQ0FBd0MseUJBQVcsR0F5Q2xEO0FBekNZLGdEQUFrQiIsInNvdXJjZXNDb250ZW50IjpbIlxyXG5pbXBvcnQgeyBCYXNlU2VydmljZSB9IGZyb20gJy4vQmFzZVNlcnZpY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvbiB9IGZyb20gJy4uL21vZGVscy9SZXNlcnZhdGlvbic7XHJcblxyXG5leHBvcnQgY2xhc3MgUmVzZXJ2YXRpb25TZXJ2aWNlIGV4dGVuZHMgQmFzZVNlcnZpY2U8UmVzZXJ2YXRpb24+e1xyXG5cclxuICBjb25zdHJ1Y3Rvcigpe1xyXG4gICAgc3VwZXIoJ1Jlc2VydmF0aW9uJyk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgTmV3UmVzZXJ2YXRpb24oY2FsbGJhY2s6KHJlc3VsdDphbnkpPT52b2lkLHJlc2VydmF0aW9uOiBSZXNlcnZhdGlvbiwgY2FwdGNoYVR5cGU6IHN0cmluZywgY2FwdGNoYTogc3RyaW5nKXtcclxuICAgIHd4LnNob3dMb2FkaW5nKHtcclxuICAgICAgdGl0bGU6IFwibG9hZGluZy4uLlwiICAgICAgXHJcbiAgICB9KTtcclxuICAgIGxldCB1cmwgPSBgJHt0aGlzLmFwaUJhc2VVcmx9L2FwaS9yZXNlcnZhdGlvbmA7XHJcbiAgICB3eC5yZXF1ZXN0KHtcclxuICAgICAgdXJsOiB1cmwsXHJcbiAgICAgIG1ldGhvZDogXCJQT1NUXCIsXHJcbiAgICAgIGhlYWRlcjp7XHJcbiAgICAgICAgXCJjYXB0Y2hhVHlwZVwiOiBjYXB0Y2hhVHlwZSxcclxuICAgICAgICBcImNhcHRjaGFcIjogY2FwdGNoYSxcclxuICAgICAgICBcIkNvbnRlbnQtVHlwZVwiOiBcImFwcGxpY2F0aW9uL2pzb25cIlxyXG4gICAgICB9LFxyXG4gICAgICBkYXRhOiByZXNlcnZhdGlvbixcclxuICAgICAgZGF0YVR5cGU6IFwianNvblwiLFxyXG4gICAgICBzdWNjZXNzOiAocmVzcG9uc2UpID0+IHtcclxuICAgICAgICB3eC5oaWRlTG9hZGluZygpO1xyXG4gICAgICAgIGxldCByZXN1bHQgPSA8YW55PnJlc3BvbnNlLmRhdGE7ICAgICAgICBcclxuICAgICAgICBpZihyZXN1bHQuU3RhdHVzID09IDIwMCl7XHJcbiAgICAgICAgICB3eC5zaG93VG9hc3Qoe1xyXG4gICAgICAgICAgICB0aXRsZTogXCLpooTnuqbmiJDlip9cIixcclxuICAgICAgICAgICAgaWNvbjogXCJzdWNjZXNzXCIsXHJcbiAgICAgICAgICAgIGR1cmF0aW9uOiAyMDAwXHJcbiAgICAgICAgICB9KTtcclxuICAgICAgICB9ZWxzZXtcclxuICAgICAgICAgIHd4LnNob3dUb2FzdCh7XHJcbiAgICAgICAgICAgIHRpdGxlOiBg6aKE57qm5aSx6LSlLCR7cmVzdWx0LkVycm9yTXNnfWAsXHJcbiAgICAgICAgICAgIGljb246IFwibm9uZVwiLFxyXG4gICAgICAgICAgICBkdXJhdGlvbjogMjAwMCxcclxuICAgICAgICAgIH0pO1xyXG4gICAgICAgIH1cclxuICAgICAgICBjYWxsYmFjayhyZXN1bHQpO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcbn1cclxuIl19