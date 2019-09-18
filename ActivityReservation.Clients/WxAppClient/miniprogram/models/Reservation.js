"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Reservation = (function () {
    function Reservation() {
    }
    Object.defineProperty(Reservation.prototype, "ReservationForDate", {
        get: function () {
            return this.reservationForDate;
        },
        set: function (v) {
            this.reservationForDate = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationForTime", {
        get: function () {
            return this.reservationForTime;
        },
        set: function (v) {
            this.reservationForTime = v.replace(',', '\n');
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationForTimeIds", {
        get: function () {
            return this.reservationForTimeIds;
        },
        set: function (v) {
            this.reservationForTimeIds = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationPersonPhone", {
        get: function () {
            return this.reservationPersonPhone;
        },
        set: function (v) {
            this.reservationPersonPhone = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationPersonName", {
        get: function () {
            return this.reservationPersonName;
        },
        set: function (v) {
            this.reservationPersonName = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationUnit", {
        get: function () {
            return this.reservationUnit;
        },
        set: function (v) {
            this.reservationUnit = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationPlaceName", {
        get: function () {
            return this.reservationPlaceName;
        },
        set: function (v) {
            this.reservationPlaceName = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationPlaceId", {
        get: function () {
            return this.reservationPlaceId;
        },
        set: function (v) {
            this.reservationPlaceId = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationActivityContent", {
        get: function () {
            return this.reservationActivityContent;
        },
        set: function (v) {
            this.reservationActivityContent = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationId", {
        get: function () {
            return this.reservationId;
        },
        set: function (v) {
            this.reservationId = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationTime", {
        get: function () {
            return this.reservationTime;
        },
        set: function (v) {
            this.reservationTime = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Reservation.prototype, "ReservationStatus", {
        get: function () {
            return this.reservationStatus;
        },
        set: function (v) {
            this.reservationStatus = v;
        },
        enumerable: true,
        configurable: true
    });
    return Reservation;
}());
exports.Reservation = Reservation;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmVzZXJ2YXRpb24uanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJSZXNlcnZhdGlvbi50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOztBQUFBO0lBQUE7SUEwR0EsQ0FBQztJQXZHRyxzQkFBVywyQ0FBa0I7YUFBN0I7WUFDSSxPQUFPLElBQUksQ0FBQyxrQkFBa0IsQ0FBQztRQUNuQyxDQUFDO2FBQ0QsVUFBOEIsQ0FBVTtZQUNwQyxJQUFJLENBQUMsa0JBQWtCLEdBQUcsQ0FBQyxDQUFDO1FBQ2hDLENBQUM7OztPQUhBO0lBTUQsc0JBQVcsMkNBQWtCO2FBQTdCO1lBQ0ksT0FBTyxJQUFJLENBQUMsa0JBQWtCLENBQUM7UUFDbkMsQ0FBQzthQUNELFVBQThCLENBQVU7WUFDdEMsSUFBSSxDQUFDLGtCQUFrQixHQUFHLENBQUMsQ0FBQyxPQUFPLENBQUMsR0FBRyxFQUFFLElBQUksQ0FBQyxDQUFDO1FBQ2pELENBQUM7OztPQUhBO0lBTUQsc0JBQVcsOENBQXFCO2FBQWhDO1lBQ0ksT0FBTyxJQUFJLENBQUMscUJBQXFCLENBQUM7UUFDdEMsQ0FBQzthQUNELFVBQWlDLENBQVU7WUFDdkMsSUFBSSxDQUFDLHFCQUFxQixHQUFHLENBQUMsQ0FBQztRQUNuQyxDQUFDOzs7T0FIQTtJQU1ELHNCQUFXLCtDQUFzQjthQUFqQztZQUNJLE9BQU8sSUFBSSxDQUFDLHNCQUFzQixDQUFDO1FBQ3ZDLENBQUM7YUFDRCxVQUFrQyxDQUFVO1lBQ3hDLElBQUksQ0FBQyxzQkFBc0IsR0FBRyxDQUFDLENBQUM7UUFDcEMsQ0FBQzs7O09BSEE7SUFNRCxzQkFBVyw4Q0FBcUI7YUFBaEM7WUFDSSxPQUFPLElBQUksQ0FBQyxxQkFBcUIsQ0FBQztRQUN0QyxDQUFDO2FBQ0QsVUFBaUMsQ0FBVTtZQUN2QyxJQUFJLENBQUMscUJBQXFCLEdBQUcsQ0FBQyxDQUFDO1FBQ25DLENBQUM7OztPQUhBO0lBT0Qsc0JBQVcsd0NBQWU7YUFBMUI7WUFDSSxPQUFPLElBQUksQ0FBQyxlQUFlLENBQUM7UUFDaEMsQ0FBQzthQUNELFVBQTJCLENBQVU7WUFDakMsSUFBSSxDQUFDLGVBQWUsR0FBRyxDQUFDLENBQUM7UUFDN0IsQ0FBQzs7O09BSEE7SUFPRCxzQkFBVyw2Q0FBb0I7YUFBL0I7WUFDSSxPQUFPLElBQUksQ0FBQyxvQkFBb0IsQ0FBQztRQUNyQyxDQUFDO2FBQ0QsVUFBZ0MsQ0FBVTtZQUN0QyxJQUFJLENBQUMsb0JBQW9CLEdBQUcsQ0FBQyxDQUFDO1FBQ2xDLENBQUM7OztPQUhBO0lBT0Qsc0JBQVcsMkNBQWtCO2FBQTdCO1lBQ0ksT0FBTyxJQUFJLENBQUMsa0JBQWtCLENBQUM7UUFDbkMsQ0FBQzthQUNELFVBQThCLENBQVU7WUFDcEMsSUFBSSxDQUFDLGtCQUFrQixHQUFHLENBQUMsQ0FBQztRQUNoQyxDQUFDOzs7T0FIQTtJQVFELHNCQUFXLG1EQUEwQjthQUFyQztZQUNJLE9BQU8sSUFBSSxDQUFDLDBCQUEwQixDQUFDO1FBQzNDLENBQUM7YUFDRCxVQUFzQyxDQUFVO1lBQzVDLElBQUksQ0FBQywwQkFBMEIsR0FBRyxDQUFDLENBQUM7UUFDeEMsQ0FBQzs7O09BSEE7SUFPRCxzQkFBVyxzQ0FBYTthQUF4QjtZQUNJLE9BQU8sSUFBSSxDQUFDLGFBQWEsQ0FBQztRQUM5QixDQUFDO2FBQ0QsVUFBeUIsQ0FBVTtZQUMvQixJQUFJLENBQUMsYUFBYSxHQUFHLENBQUMsQ0FBQztRQUMzQixDQUFDOzs7T0FIQTtJQU9ELHNCQUFXLHdDQUFlO2FBQTFCO1lBQ0ksT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDO1FBQ2hDLENBQUM7YUFDRCxVQUEyQixDQUFRO1lBQy9CLElBQUksQ0FBQyxlQUFlLEdBQUcsQ0FBQyxDQUFDO1FBQzdCLENBQUM7OztPQUhBO0lBT0Qsc0JBQVcsMENBQWlCO2FBQTVCO1lBQ0ksT0FBTyxJQUFJLENBQUMsaUJBQWlCLENBQUM7UUFDbEMsQ0FBQzthQUNELFVBQTZCLENBQVU7WUFDbkMsSUFBSSxDQUFDLGlCQUFpQixHQUFHLENBQUMsQ0FBQztRQUMvQixDQUFDOzs7T0FIQTtJQUtMLGtCQUFDO0FBQUQsQ0FBQyxBQTFHRCxJQTBHQztBQTFHWSxrQ0FBVyIsInNvdXJjZXNDb250ZW50IjpbImV4cG9ydCBjbGFzcyBSZXNlcnZhdGlvbntcclxuICAgIFxyXG4gICAgcHJpdmF0ZSByZXNlcnZhdGlvbkZvckRhdGUgOiBzdHJpbmc7XHJcbiAgICBwdWJsaWMgZ2V0IFJlc2VydmF0aW9uRm9yRGF0ZSgpIDogc3RyaW5nIHtcclxuICAgICAgICByZXR1cm4gdGhpcy5yZXNlcnZhdGlvbkZvckRhdGU7XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IFJlc2VydmF0aW9uRm9yRGF0ZSh2IDogc3RyaW5nKSB7XHJcbiAgICAgICAgdGhpcy5yZXNlcnZhdGlvbkZvckRhdGUgPSB2O1xyXG4gICAgfVxyXG4gICAgXHJcbiAgICBwcml2YXRlIHJlc2VydmF0aW9uRm9yVGltZSA6IHN0cmluZztcclxuICAgIHB1YmxpYyBnZXQgUmVzZXJ2YXRpb25Gb3JUaW1lKCkgOiBzdHJpbmcge1xyXG4gICAgICAgIHJldHVybiB0aGlzLnJlc2VydmF0aW9uRm9yVGltZTtcclxuICAgIH1cclxuICAgIHB1YmxpYyBzZXQgUmVzZXJ2YXRpb25Gb3JUaW1lKHYgOiBzdHJpbmcpIHtcclxuICAgICAgdGhpcy5yZXNlcnZhdGlvbkZvclRpbWUgPSB2LnJlcGxhY2UoJywnLCAnXFxuJyk7XHJcbiAgICB9XHJcbiAgICBcclxuICAgIHByaXZhdGUgcmVzZXJ2YXRpb25Gb3JUaW1lSWRzIDogc3RyaW5nO1xyXG4gICAgcHVibGljIGdldCBSZXNlcnZhdGlvbkZvclRpbWVJZHMoKSA6IHN0cmluZyB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMucmVzZXJ2YXRpb25Gb3JUaW1lSWRzO1xyXG4gICAgfVxyXG4gICAgcHVibGljIHNldCBSZXNlcnZhdGlvbkZvclRpbWVJZHModiA6IHN0cmluZykge1xyXG4gICAgICAgIHRoaXMucmVzZXJ2YXRpb25Gb3JUaW1lSWRzID0gdjtcclxuICAgIH1cclxuICAgIFxyXG4gICAgcHJpdmF0ZSByZXNlcnZhdGlvblBlcnNvblBob25lIDogc3RyaW5nO1xyXG4gICAgcHVibGljIGdldCBSZXNlcnZhdGlvblBlcnNvblBob25lKCkgOiBzdHJpbmcge1xyXG4gICAgICAgIHJldHVybiB0aGlzLnJlc2VydmF0aW9uUGVyc29uUGhvbmU7XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IFJlc2VydmF0aW9uUGVyc29uUGhvbmUodiA6IHN0cmluZykge1xyXG4gICAgICAgIHRoaXMucmVzZXJ2YXRpb25QZXJzb25QaG9uZSA9IHY7XHJcbiAgICB9XHJcbiAgICBcclxuICAgIHByaXZhdGUgcmVzZXJ2YXRpb25QZXJzb25OYW1lIDogc3RyaW5nO1xyXG4gICAgcHVibGljIGdldCBSZXNlcnZhdGlvblBlcnNvbk5hbWUoKSA6IHN0cmluZyB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMucmVzZXJ2YXRpb25QZXJzb25OYW1lO1xyXG4gICAgfVxyXG4gICAgcHVibGljIHNldCBSZXNlcnZhdGlvblBlcnNvbk5hbWUodiA6IHN0cmluZykge1xyXG4gICAgICAgIHRoaXMucmVzZXJ2YXRpb25QZXJzb25OYW1lID0gdjtcclxuICAgIH1cclxuICAgIFxyXG4gICAgXHJcbiAgICBwcml2YXRlIHJlc2VydmF0aW9uVW5pdCA6IHN0cmluZztcclxuICAgIHB1YmxpYyBnZXQgUmVzZXJ2YXRpb25Vbml0KCkgOiBzdHJpbmcge1xyXG4gICAgICAgIHJldHVybiB0aGlzLnJlc2VydmF0aW9uVW5pdDtcclxuICAgIH1cclxuICAgIHB1YmxpYyBzZXQgUmVzZXJ2YXRpb25Vbml0KHYgOiBzdHJpbmcpIHtcclxuICAgICAgICB0aGlzLnJlc2VydmF0aW9uVW5pdCA9IHY7XHJcbiAgICB9XHJcbiAgICBcclxuICAgIFxyXG4gICAgcHJpdmF0ZSByZXNlcnZhdGlvblBsYWNlTmFtZSA6IHN0cmluZztcclxuICAgIHB1YmxpYyBnZXQgUmVzZXJ2YXRpb25QbGFjZU5hbWUoKSA6IHN0cmluZyB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMucmVzZXJ2YXRpb25QbGFjZU5hbWU7XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IFJlc2VydmF0aW9uUGxhY2VOYW1lKHYgOiBzdHJpbmcpIHtcclxuICAgICAgICB0aGlzLnJlc2VydmF0aW9uUGxhY2VOYW1lID0gdjtcclxuICAgIH1cclxuXHJcbiAgICBcclxuICAgIHByaXZhdGUgcmVzZXJ2YXRpb25QbGFjZUlkIDogc3RyaW5nO1xyXG4gICAgcHVibGljIGdldCBSZXNlcnZhdGlvblBsYWNlSWQoKSA6IHN0cmluZyB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMucmVzZXJ2YXRpb25QbGFjZUlkO1xyXG4gICAgfVxyXG4gICAgcHVibGljIHNldCBSZXNlcnZhdGlvblBsYWNlSWQodiA6IHN0cmluZykge1xyXG4gICAgICAgIHRoaXMucmVzZXJ2YXRpb25QbGFjZUlkID0gdjtcclxuICAgIH1cclxuICAgIFxyXG4gICAgXHJcbiAgICBcclxuICAgIHByaXZhdGUgcmVzZXJ2YXRpb25BY3Rpdml0eUNvbnRlbnQgOiBzdHJpbmc7XHJcbiAgICBwdWJsaWMgZ2V0IFJlc2VydmF0aW9uQWN0aXZpdHlDb250ZW50KCkgOiBzdHJpbmcge1xyXG4gICAgICAgIHJldHVybiB0aGlzLnJlc2VydmF0aW9uQWN0aXZpdHlDb250ZW50O1xyXG4gICAgfVxyXG4gICAgcHVibGljIHNldCBSZXNlcnZhdGlvbkFjdGl2aXR5Q29udGVudCh2IDogc3RyaW5nKSB7XHJcbiAgICAgICAgdGhpcy5yZXNlcnZhdGlvbkFjdGl2aXR5Q29udGVudCA9IHY7XHJcbiAgICB9XHJcbiAgICBcclxuICAgIFxyXG4gICAgcHJpdmF0ZSByZXNlcnZhdGlvbklkIDogc3RyaW5nO1xyXG4gICAgcHVibGljIGdldCBSZXNlcnZhdGlvbklkKCkgOiBzdHJpbmcge1xyXG4gICAgICAgIHJldHVybiB0aGlzLnJlc2VydmF0aW9uSWQ7XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IFJlc2VydmF0aW9uSWQodiA6IHN0cmluZykge1xyXG4gICAgICAgIHRoaXMucmVzZXJ2YXRpb25JZCA9IHY7XHJcbiAgICB9XHJcbiAgICBcclxuICAgIFxyXG4gICAgcHJpdmF0ZSByZXNlcnZhdGlvblRpbWUgOiBEYXRlO1xyXG4gICAgcHVibGljIGdldCBSZXNlcnZhdGlvblRpbWUoKSA6IERhdGUge1xyXG4gICAgICAgIHJldHVybiB0aGlzLnJlc2VydmF0aW9uVGltZTtcclxuICAgIH1cclxuICAgIHB1YmxpYyBzZXQgUmVzZXJ2YXRpb25UaW1lKHYgOiBEYXRlKSB7XHJcbiAgICAgICAgdGhpcy5yZXNlcnZhdGlvblRpbWUgPSB2O1xyXG4gICAgfVxyXG4gICAgXHJcbiAgICBcclxuICAgIHByaXZhdGUgcmVzZXJ2YXRpb25TdGF0dXMgOiBudW1iZXI7XHJcbiAgICBwdWJsaWMgZ2V0IFJlc2VydmF0aW9uU3RhdHVzKCkgOiBudW1iZXIge1xyXG4gICAgICAgIHJldHVybiB0aGlzLnJlc2VydmF0aW9uU3RhdHVzO1xyXG4gICAgfVxyXG4gICAgcHVibGljIHNldCBSZXNlcnZhdGlvblN0YXR1cyh2IDogbnVtYmVyKSB7XHJcbiAgICAgICAgdGhpcy5yZXNlcnZhdGlvblN0YXR1cyA9IHY7XHJcbiAgICB9XHJcbiAgICBcclxufSJdfQ==