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
var ReservationPlaceService = (function (_super) {
    __extends(ReservationPlaceService, _super);
    function ReservationPlaceService() {
        return _super.call(this, 'ReservationPlace') || this;
    }
    ReservationPlaceService.prototype.getAvailablePeriods = function (callback, placeId, date) {
        wx.showLoading({
            title: "loading..."
        });
        var url = this.apiBaseUrl + "/api/reservationPlace/" + placeId + "/periods?dt=" + date;
        wx.request({
            url: url,
            success: function (response) {
                wx.hideLoading();
                var result = response.data;
                callback(result);
            }
        });
    };
    return ReservationPlaceService;
}(BaseService_1.BaseService));
exports.ReservationPlaceService = ReservationPlaceService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJSZXNlcnZhdGlvblBsYWNlU2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7Ozs7Ozs7Ozs7QUFBQSw2Q0FBNEM7QUFJNUM7SUFBNkMsMkNBQTZCO0lBRXhFO2VBQ0Usa0JBQU0sa0JBQWtCLENBQUM7SUFDM0IsQ0FBQztJQUVNLHFEQUFtQixHQUExQixVQUEyQixRQUFnRCxFQUFFLE9BQWMsRUFBRSxJQUFZO1FBQ3ZHLEVBQUUsQ0FBQyxXQUFXLENBQUM7WUFDYixLQUFLLEVBQUUsWUFBWTtTQUNwQixDQUFDLENBQUM7UUFDSCxJQUFJLEdBQUcsR0FBTSxJQUFJLENBQUMsVUFBVSw4QkFBeUIsT0FBTyxvQkFBZSxJQUFNLENBQUM7UUFDbEYsRUFBRSxDQUFDLE9BQU8sQ0FBQztZQUNULEdBQUcsRUFBRSxHQUFHO1lBQ1IsT0FBTyxFQUFFLFVBQUMsUUFBUTtnQkFDaEIsRUFBRSxDQUFDLFdBQVcsRUFBRSxDQUFDO2dCQUNqQixJQUFJLE1BQU0sR0FBNkIsUUFBUSxDQUFDLElBQUksQ0FBQztnQkFDckQsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO1lBQ25CLENBQUM7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDO0lBRUgsOEJBQUM7QUFBRCxDQUFDLEFBckJELENBQTZDLHlCQUFXLEdBcUJ2RDtBQXJCWSwwREFBdUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBCYXNlU2VydmljZSB9IGZyb20gJy4vQmFzZVNlcnZpY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvblBsYWNlIH0gZnJvbSAnLi4vbW9kZWxzL1Jlc2VydmF0aW9uUGxhY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvblBlcmlvZCB9IGZyb20gJy4uL21vZGVscy9SZXNlcnZhdGlvblBlcmlvZCc7XHJcblxyXG5leHBvcnQgY2xhc3MgUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UgZXh0ZW5kcyBCYXNlU2VydmljZTxSZXNlcnZhdGlvblBsYWNlPntcclxuXHJcbiAgY29uc3RydWN0b3IoKXtcclxuICAgIHN1cGVyKCdSZXNlcnZhdGlvblBsYWNlJyk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgZ2V0QXZhaWxhYmxlUGVyaW9kcyhjYWxsYmFjazoocmVzdWx0OkFycmF5PFJlc2VydmF0aW9uUGVyaW9kPik9PnZvaWQsIHBsYWNlSWQ6c3RyaW5nLCBkYXRlOiBzdHJpbmcpIHtcclxuICAgIHd4LnNob3dMb2FkaW5nKHtcclxuICAgICAgdGl0bGU6IFwibG9hZGluZy4uLlwiXHJcbiAgICB9KTtcclxuICAgIGxldCB1cmwgPSBgJHt0aGlzLmFwaUJhc2VVcmx9L2FwaS9yZXNlcnZhdGlvblBsYWNlLyR7cGxhY2VJZH0vcGVyaW9kcz9kdD0ke2RhdGV9YDtcclxuICAgIHd4LnJlcXVlc3Qoe1xyXG4gICAgICB1cmw6IHVybCxcclxuICAgICAgc3VjY2VzczogKHJlc3BvbnNlKSA9PiB7XHJcbiAgICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPEFycmF5PFJlc2VydmF0aW9uUGVyaW9kPj5yZXNwb25zZS5kYXRhO1xyXG4gICAgICAgIGNhbGxiYWNrKHJlc3VsdCk7XHJcbiAgICAgIH1cclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbn1cclxuIl19