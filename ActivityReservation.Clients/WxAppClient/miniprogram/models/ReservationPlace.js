"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ReservationPlace = (function () {
    function ReservationPlace() {
    }
    Object.defineProperty(ReservationPlace.prototype, "PlaceId", {
        get: function () {
            return this._PlaceId;
        },
        set: function (v) {
            this._PlaceId = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReservationPlace.prototype, "PlaceName", {
        get: function () {
            return this._PlaceName;
        },
        set: function (v) {
            this._PlaceName = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReservationPlace.prototype, "PlaceIndex", {
        get: function () {
            return this._PlaceIndex;
        },
        set: function (v) {
            this._PlaceIndex = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReservationPlace.prototype, "MaxReservationPeriodNum", {
        get: function () {
            return this._MaxReservationPeriodNum;
        },
        set: function (v) {
            this._MaxReservationPeriodNum = v;
        },
        enumerable: true,
        configurable: true
    });
    return ReservationPlace;
}());
exports.ReservationPlace = ReservationPlace;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmVzZXJ2YXRpb25QbGFjZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIlJlc2VydmF0aW9uUGxhY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7QUFBQTtJQUFBO0lBcUNBLENBQUM7SUFsQ0csc0JBQVcscUNBQU87YUFBbEI7WUFDSSxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUM7UUFDekIsQ0FBQzthQUNELFVBQW1CLENBQVU7WUFDekIsSUFBSSxDQUFDLFFBQVEsR0FBRyxDQUFDLENBQUM7UUFDdEIsQ0FBQzs7O09BSEE7SUFPRCxzQkFBVyx1Q0FBUzthQUFwQjtZQUNJLE9BQU8sSUFBSSxDQUFDLFVBQVUsQ0FBQztRQUMzQixDQUFDO2FBQ0QsVUFBcUIsQ0FBVTtZQUMzQixJQUFJLENBQUMsVUFBVSxHQUFHLENBQUMsQ0FBQztRQUN4QixDQUFDOzs7T0FIQTtJQU9ELHNCQUFXLHdDQUFVO2FBQXJCO1lBQ0ksT0FBTyxJQUFJLENBQUMsV0FBVyxDQUFDO1FBQzVCLENBQUM7YUFDRCxVQUFzQixDQUFVO1lBQzVCLElBQUksQ0FBQyxXQUFXLEdBQUcsQ0FBQyxDQUFDO1FBQ3pCLENBQUM7OztPQUhBO0lBT0Qsc0JBQVcscURBQXVCO2FBQWxDO1lBQ0ksT0FBTyxJQUFJLENBQUMsd0JBQXdCLENBQUM7UUFDekMsQ0FBQzthQUNELFVBQW1DLENBQVU7WUFDekMsSUFBSSxDQUFDLHdCQUF3QixHQUFHLENBQUMsQ0FBQztRQUN0QyxDQUFDOzs7T0FIQTtJQUtMLHVCQUFDO0FBQUQsQ0FBQyxBQXJDRCxJQXFDQztBQXJDWSw0Q0FBZ0IiLCJzb3VyY2VzQ29udGVudCI6WyJleHBvcnQgY2xhc3MgUmVzZXJ2YXRpb25QbGFjZXtcclxuICAgIFxyXG4gICAgcHJpdmF0ZSBfUGxhY2VJZCA6IHN0cmluZztcclxuICAgIHB1YmxpYyBnZXQgUGxhY2VJZCgpIDogc3RyaW5nIHtcclxuICAgICAgICByZXR1cm4gdGhpcy5fUGxhY2VJZDtcclxuICAgIH1cclxuICAgIHB1YmxpYyBzZXQgUGxhY2VJZCh2IDogc3RyaW5nKSB7XHJcbiAgICAgICAgdGhpcy5fUGxhY2VJZCA9IHY7XHJcbiAgICB9XHJcbiAgICBcclxuICAgIFxyXG4gICAgcHJpdmF0ZSBfUGxhY2VOYW1lIDogc3RyaW5nO1xyXG4gICAgcHVibGljIGdldCBQbGFjZU5hbWUoKSA6IHN0cmluZyB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMuX1BsYWNlTmFtZTtcclxuICAgIH1cclxuICAgIHB1YmxpYyBzZXQgUGxhY2VOYW1lKHYgOiBzdHJpbmcpIHtcclxuICAgICAgICB0aGlzLl9QbGFjZU5hbWUgPSB2O1xyXG4gICAgfVxyXG4gICAgXHJcbiAgICBcclxuICAgIHByaXZhdGUgX1BsYWNlSW5kZXggOiBudW1iZXI7XHJcbiAgICBwdWJsaWMgZ2V0IFBsYWNlSW5kZXgoKSA6IG51bWJlciB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMuX1BsYWNlSW5kZXg7XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IFBsYWNlSW5kZXgodiA6IG51bWJlcikge1xyXG4gICAgICAgIHRoaXMuX1BsYWNlSW5kZXggPSB2O1xyXG4gICAgfVxyXG4gICAgXHJcbiAgICBcclxuICAgIHByaXZhdGUgX01heFJlc2VydmF0aW9uUGVyaW9kTnVtIDogbnVtYmVyO1xyXG4gICAgcHVibGljIGdldCBNYXhSZXNlcnZhdGlvblBlcmlvZE51bSgpIDogbnVtYmVyIHtcclxuICAgICAgICByZXR1cm4gdGhpcy5fTWF4UmVzZXJ2YXRpb25QZXJpb2ROdW07XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IE1heFJlc2VydmF0aW9uUGVyaW9kTnVtKHYgOiBudW1iZXIpIHtcclxuICAgICAgICB0aGlzLl9NYXhSZXNlcnZhdGlvblBlcmlvZE51bSA9IHY7XHJcbiAgICB9XHJcbiAgICBcclxufSJdfQ==