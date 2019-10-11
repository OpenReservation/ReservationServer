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
            },
            fail: function (err) {
            }
        });
    };
    return ReservationService;
}(BaseService_1.BaseService));
exports.ReservationService = ReservationService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmVzZXJ2YXRpb25TZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiUmVzZXJ2YXRpb25TZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7Ozs7OztBQUNBLDZDQUE0QztBQUs1QztJQUF3QyxzQ0FBd0I7SUFFOUQ7ZUFDRSxrQkFBTSxhQUFhLENBQUM7SUFDdEIsQ0FBQztJQUVNLDJDQUFjLEdBQXJCLFVBQXNCLFFBQTJCLEVBQUMsV0FBd0IsRUFBRSxXQUFtQixFQUFFLE9BQWU7UUFDOUcsRUFBRSxDQUFDLFdBQVcsQ0FBQztZQUNiLEtBQUssRUFBRSxZQUFZO1NBQ3BCLENBQUMsQ0FBQztRQUNILElBQUksR0FBRyxHQUFNLElBQUksQ0FBQyxVQUFVLHFCQUFrQixDQUFDO1FBQy9DLEVBQUUsQ0FBQyxPQUFPLENBQUM7WUFDVCxHQUFHLEVBQUUsR0FBRztZQUNSLE1BQU0sRUFBRSxNQUFNO1lBQ2QsTUFBTSxFQUFDO2dCQUNMLGFBQWEsRUFBRSxXQUFXO2dCQUMxQixTQUFTLEVBQUUsT0FBTztnQkFDbEIsY0FBYyxFQUFFLGtCQUFrQjthQUNuQztZQUNELElBQUksRUFBRSxXQUFXO1lBQ2pCLFFBQVEsRUFBRSxNQUFNO1lBQ2hCLE9BQU8sRUFBRSxVQUFDLFFBQVE7Z0JBQ2hCLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztnQkFDakIsSUFBSSxNQUFNLEdBQVEsUUFBUSxDQUFDLElBQUksQ0FBQztnQkFDaEMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO1lBQ25CLENBQUM7WUFDRCxJQUFJLEVBQUUsVUFBQyxHQUFHO1lBRVYsQ0FBQztTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7SUFDSCx5QkFBQztBQUFELENBQUMsQUEvQkQsQ0FBd0MseUJBQVcsR0ErQmxEO0FBL0JZLGdEQUFrQiIsInNvdXJjZXNDb250ZW50IjpbIlxyXG5pbXBvcnQgeyBCYXNlU2VydmljZSB9IGZyb20gJy4vQmFzZVNlcnZpY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvbiB9IGZyb20gJy4uL21vZGVscy9SZXNlcnZhdGlvbic7XHJcbmltcG9ydCB7IFJldHJ5SGVscGVyIH0gZnJvbSAnLi4vdXRpbHMvUmV0cnlIZWxwZXInO1xyXG5cclxuXHJcbmV4cG9ydCBjbGFzcyBSZXNlcnZhdGlvblNlcnZpY2UgZXh0ZW5kcyBCYXNlU2VydmljZTxSZXNlcnZhdGlvbj57XHJcblxyXG4gIGNvbnN0cnVjdG9yKCl7XHJcbiAgICBzdXBlcignUmVzZXJ2YXRpb24nKTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBOZXdSZXNlcnZhdGlvbihjYWxsYmFjazoocmVzdWx0OmFueSk9PnZvaWQscmVzZXJ2YXRpb246IFJlc2VydmF0aW9uLCBjYXB0Y2hhVHlwZTogc3RyaW5nLCBjYXB0Y2hhOiBzdHJpbmcpe1xyXG4gICAgd3guc2hvd0xvYWRpbmcoe1xyXG4gICAgICB0aXRsZTogXCJsb2FkaW5nLi4uXCIgIFxyXG4gICAgfSk7XHJcbiAgICBsZXQgdXJsID0gYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvcmVzZXJ2YXRpb25gO1xyXG4gICAgd3gucmVxdWVzdCh7XHJcbiAgICAgIHVybDogdXJsLFxyXG4gICAgICBtZXRob2Q6IFwiUE9TVFwiLFxyXG4gICAgICBoZWFkZXI6e1xyXG4gICAgICAgIFwiY2FwdGNoYVR5cGVcIjogY2FwdGNoYVR5cGUsXHJcbiAgICAgICAgXCJjYXB0Y2hhXCI6IGNhcHRjaGEsXHJcbiAgICAgICAgXCJDb250ZW50LVR5cGVcIjogXCJhcHBsaWNhdGlvbi9qc29uXCJcclxuICAgICAgfSxcclxuICAgICAgZGF0YTogcmVzZXJ2YXRpb24sXHJcbiAgICAgIGRhdGFUeXBlOiBcImpzb25cIixcclxuICAgICAgc3VjY2VzczogKHJlc3BvbnNlKSA9PiB7XHJcbiAgICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPGFueT5yZXNwb25zZS5kYXRhO1xyXG4gICAgICAgIGNhbGxiYWNrKHJlc3VsdCk7XHJcbiAgICAgIH0sXHJcbiAgICAgIGZhaWw6IChlcnIpPT57XHJcbiAgICAgICAgXHJcbiAgICAgIH1cclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=