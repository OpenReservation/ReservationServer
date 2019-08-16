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
    ReservationService.prototype.NewReservation = function (reservation, captchaType, captcha) {
    };
    return ReservationService;
}(BaseService_1.BaseService));
exports.ReservationService = ReservationService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmVzZXJ2YXRpb25TZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiUmVzZXJ2YXRpb25TZXJ2aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7Ozs7OztBQUNBLDZDQUE0QztBQUc1QztJQUF3QyxzQ0FBd0I7SUFFOUQ7ZUFDRSxrQkFBTSxhQUFhLENBQUM7SUFDdEIsQ0FBQztJQUVNLDJDQUFjLEdBQXJCLFVBQXNCLFdBQXdCLEVBQUUsV0FBbUIsRUFBRSxPQUFlO0lBT3BGLENBQUM7SUFDSCx5QkFBQztBQUFELENBQUMsQUFkRCxDQUF3Qyx5QkFBVyxHQWNsRDtBQWRZLGdEQUFrQiIsInNvdXJjZXNDb250ZW50IjpbIlxyXG5pbXBvcnQgeyBCYXNlU2VydmljZSB9IGZyb20gJy4vQmFzZVNlcnZpY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvbiB9IGZyb20gJy4uL21vZGVscy9SZXNlcnZhdGlvbic7XHJcblxyXG5leHBvcnQgY2xhc3MgUmVzZXJ2YXRpb25TZXJ2aWNlIGV4dGVuZHMgQmFzZVNlcnZpY2U8UmVzZXJ2YXRpb24+e1xyXG5cclxuICBjb25zdHJ1Y3Rvcigpe1xyXG4gICAgc3VwZXIoJ1Jlc2VydmF0aW9uJyk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgTmV3UmVzZXJ2YXRpb24ocmVzZXJ2YXRpb246IFJlc2VydmF0aW9uLCBjYXB0Y2hhVHlwZTogc3RyaW5nLCBjYXB0Y2hhOiBzdHJpbmcpe1xyXG4gICAgLy8gcmV0dXJuIHRoaXMuaHR0cC5wb3N0PGFueT4oYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvcmVzZXJ2YXRpb25gLCByZXNlcnZhdGlvbiwge1xyXG4gICAgLy8gICBoZWFkZXJzOiB7XHJcbiAgICAvLyAgICAgXCJjYXB0Y2hhXCI6IGNhcHRjaGEsXHJcbiAgICAvLyAgICAgXCJjYXB0Y2hhVHlwZVwiOiBjYXB0Y2hhVHlwZVxyXG4gICAgLy8gICB9XHJcbiAgICAvLyB9KTtcclxuICB9XHJcbn1cclxuIl19