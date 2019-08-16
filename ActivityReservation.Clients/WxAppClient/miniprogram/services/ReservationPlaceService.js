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
    ReservationPlaceService.prototype.getAvailablePeriods = function (placeId, date) {
    };
    return ReservationPlaceService;
}(BaseService_1.BaseService));
exports.ReservationPlaceService = ReservationPlaceService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJSZXNlcnZhdGlvblBsYWNlU2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7Ozs7Ozs7Ozs7QUFBQSw2Q0FBNEM7QUFJNUM7SUFBNkMsMkNBQTZCO0lBRXhFO2VBQ0Usa0JBQU0sa0JBQWtCLENBQUM7SUFDM0IsQ0FBQztJQUVNLHFEQUFtQixHQUExQixVQUEyQixPQUFjLEVBQUUsSUFBWTtJQUV2RCxDQUFDO0lBRUgsOEJBQUM7QUFBRCxDQUFDLEFBVkQsQ0FBNkMseUJBQVcsR0FVdkQ7QUFWWSwwREFBdUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBCYXNlU2VydmljZSB9IGZyb20gJy4vQmFzZVNlcnZpY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvblBsYWNlIH0gZnJvbSAnLi4vbW9kZWxzL1Jlc2VydmF0aW9uUGxhY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvblBlcmlvZCB9IGZyb20gJy4uL21vZGVscy9SZXNlcnZhdGlvblBlcmlvZCc7XHJcblxyXG5leHBvcnQgY2xhc3MgUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UgZXh0ZW5kcyBCYXNlU2VydmljZTxSZXNlcnZhdGlvblBsYWNlPntcclxuXHJcbiAgY29uc3RydWN0b3IoKXtcclxuICAgIHN1cGVyKCdSZXNlcnZhdGlvblBsYWNlJyk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgZ2V0QXZhaWxhYmxlUGVyaW9kcyhwbGFjZUlkOnN0cmluZywgZGF0ZTogc3RyaW5nKSB7XHJcbiAgICAvL3JldHVybiB0aGlzLmh0dHAuZ2V0PEFycmF5PFJlc2VydmF0aW9uUGVyaW9kPj4oYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvcmVzZXJ2YXRpb25QbGFjZS8ke3BsYWNlSWR9L3BlcmlvZHM/ZHQ9JHtkYXRlfWApO1xyXG4gIH1cclxuXHJcbn1cclxuIl19