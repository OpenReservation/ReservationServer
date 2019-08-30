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
                callback(result);
            }
        });
    };
    return ReservationService;
}(BaseService_1.BaseService));
exports.ReservationService = ReservationService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmVzZXJ2YXRpb25TZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiUmVzZXJ2YXRpb25TZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7Ozs7OztBQUNBLDZDQUE0QztBQUc1QztJQUF3QyxzQ0FBd0I7SUFFOUQ7ZUFDRSxrQkFBTSxhQUFhLENBQUM7SUFDdEIsQ0FBQztJQUVNLDJDQUFjLEdBQXJCLFVBQXNCLFFBQTJCLEVBQUMsV0FBd0IsRUFBRSxXQUFtQixFQUFFLE9BQWU7UUFDOUcsRUFBRSxDQUFDLFdBQVcsQ0FBQztZQUNiLEtBQUssRUFBRSxZQUFZO1NBQ3BCLENBQUMsQ0FBQztRQUNILElBQUksR0FBRyxHQUFNLElBQUksQ0FBQyxVQUFVLHFCQUFrQixDQUFDO1FBQy9DLEVBQUUsQ0FBQyxPQUFPLENBQUM7WUFDVCxHQUFHLEVBQUUsR0FBRztZQUNSLE1BQU0sRUFBRSxNQUFNO1lBQ2QsTUFBTSxFQUFDO2dCQUNMLGFBQWEsRUFBRSxXQUFXO2dCQUMxQixTQUFTLEVBQUUsT0FBTztnQkFDbEIsY0FBYyxFQUFFLGtCQUFrQjthQUNuQztZQUNELElBQUksRUFBRSxXQUFXO1lBQ2pCLFFBQVEsRUFBRSxNQUFNO1lBQ2hCLE9BQU8sRUFBRSxVQUFDLFFBQVE7Z0JBQ2hCLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztnQkFDakIsSUFBSSxNQUFNLEdBQVEsUUFBUSxDQUFDLElBQUksQ0FBQztnQkFDaEMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO1lBQ25CLENBQUM7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDO0lBQ0gseUJBQUM7QUFBRCxDQUFDLEFBNUJELENBQXdDLHlCQUFXLEdBNEJsRDtBQTVCWSxnREFBa0IiLCJzb3VyY2VzQ29udGVudCI6WyJcclxuaW1wb3J0IHsgQmFzZVNlcnZpY2UgfSBmcm9tICcuL0Jhc2VTZXJ2aWNlJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb24gfSBmcm9tICcuLi9tb2RlbHMvUmVzZXJ2YXRpb24nO1xyXG5cclxuZXhwb3J0IGNsYXNzIFJlc2VydmF0aW9uU2VydmljZSBleHRlbmRzIEJhc2VTZXJ2aWNlPFJlc2VydmF0aW9uPntcclxuXHJcbiAgY29uc3RydWN0b3IoKXtcclxuICAgIHN1cGVyKCdSZXNlcnZhdGlvbicpO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIE5ld1Jlc2VydmF0aW9uKGNhbGxiYWNrOihyZXN1bHQ6YW55KT0+dm9pZCxyZXNlcnZhdGlvbjogUmVzZXJ2YXRpb24sIGNhcHRjaGFUeXBlOiBzdHJpbmcsIGNhcHRjaGE6IHN0cmluZyl7XHJcbiAgICB3eC5zaG93TG9hZGluZyh7XHJcbiAgICAgIHRpdGxlOiBcImxvYWRpbmcuLi5cIiAgICAgIFxyXG4gICAgfSk7XHJcbiAgICBsZXQgdXJsID0gYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvcmVzZXJ2YXRpb25gO1xyXG4gICAgd3gucmVxdWVzdCh7XHJcbiAgICAgIHVybDogdXJsLFxyXG4gICAgICBtZXRob2Q6IFwiUE9TVFwiLFxyXG4gICAgICBoZWFkZXI6e1xyXG4gICAgICAgIFwiY2FwdGNoYVR5cGVcIjogY2FwdGNoYVR5cGUsXHJcbiAgICAgICAgXCJjYXB0Y2hhXCI6IGNhcHRjaGEsXHJcbiAgICAgICAgXCJDb250ZW50LVR5cGVcIjogXCJhcHBsaWNhdGlvbi9qc29uXCJcclxuICAgICAgfSxcclxuICAgICAgZGF0YTogcmVzZXJ2YXRpb24sXHJcbiAgICAgIGRhdGFUeXBlOiBcImpzb25cIixcclxuICAgICAgc3VjY2VzczogKHJlc3BvbnNlKSA9PiB7XHJcbiAgICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPGFueT5yZXNwb25zZS5kYXRhO1xyXG4gICAgICAgIGNhbGxiYWNrKHJlc3VsdCk7XHJcbiAgICAgIH1cclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=