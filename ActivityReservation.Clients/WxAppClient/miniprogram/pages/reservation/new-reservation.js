"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ReservationService_1 = require("../../services/ReservationService");
var ReservationPlaceService_1 = require("../../services/ReservationPlaceService");
var Reservation_1 = require("../../models/Reservation");
var util = require("../../utils/util");
var reservationSvc = new ReservationService_1.ReservationService();
var reservationPlaceSvc = new ReservationPlaceService_1.ReservationPlaceService();
Page({
    data: {
        stepIndex: 0,
        places: [],
        placeNames: [],
        currentDate: new Date().getTime(),
        maxDate: util.addDays(new Date(), 7).getTime(),
        reservation: new Reservation_1.Reservation(),
        reservationPeriods: [],
        checkedPeriods: [],
        selectedPlaceIndex: 0,
        unitErr: "",
        acContentErr: "",
        pNameErr: "",
        pPhoneErr: ""
    },
    onLoad: function (params) {
        var _this = this;
        console.log(params);
        reservationPlaceSvc.GetAll(function (result) {
            _this.setData({
                places: result,
                placeNames: result.map(function (p) { return p.PlaceName; })
            });
        });
    },
    onStepChange: function () {
        var _this = this;
        console.log("stepIndex:" + this.data.stepIndex + ",reservationInfo: " + JSON.stringify(this.data.reservation));
        switch (this.data.stepIndex) {
            case 0:
                break;
            case 1:
                if (!this.data.reservation.ReservationPlaceId) {
                    this.data.reservation.ReservationPlaceId = this.data.places[0].PlaceId;
                    this.data.reservation.ReservationPlaceName = this.data.places[0].PlaceName;
                }
                break;
            case 2:
                if (!this.data.reservation.ReservationForDate) {
                    this.data.reservation.ReservationForDate = util.formatDate(new Date());
                }
                reservationPlaceSvc.getAvailablePeriods(function (result) {
                    console.log(result);
                    _this.setData({
                        reservationPeriods: result,
                        checkedPeriods: []
                    });
                }, this.data.reservation.ReservationPlaceId, this.data.reservation.ReservationForDate);
                break;
            case 3:
                if (this.data.checkedPeriods.length == 0) {
                    wx.showToast({
                        title: "请选择要预约的时间段或返回上一步选择其他预约日期",
                        duration: 2000,
                        icon: "none"
                    });
                    this.data.stepIndex--;
                    return false;
                }
                console.log(this.data.reservationPeriods);
                this.data.reservation.ReservationForTimeIds = this.data.reservationPeriods.filter(function (_) { return _.Checked; }).map(function (x) { return x.PeriodIndex; }).join(",");
                this.data.reservation.ReservationForTime = this.data.reservationPeriods.filter(function (_) { return _.Checked; }).map(function (x) { return x.PeriodTitle; }).join(",");
                break;
            case 4:
                if (this.validateInputParams()) {
                    this.setData({
                        reservation: this.data.reservation
                    });
                }
                else {
                    this.data.stepIndex--;
                    return false;
                }
                break;
        }
        return true;
    },
    prevStep: function (event) {
        this.data.stepIndex--;
        this.onStepChange();
        this.setData({
            stepIndex: this.data.stepIndex
        });
    },
    nextStep: function (event) {
        this.data.stepIndex++;
        this.onStepChange();
        this.setData({
            stepIndex: this.data.stepIndex
        });
    },
    onPlaceChange: function (event) {
        var _a = event.detail, picker = _a.picker, value = _a.value, index = _a.index;
        this.setData({
            selectedPlaceIndex: index
        });
        this.data.reservation.ReservationPlaceId = this.data.places[index].PlaceId;
        this.data.reservation.ReservationPlaceName = this.data.places[index].PlaceName;
    },
    onDateInput: function (event) {
        this.setData({
            currentDate: event.detail
        });
        var dateStr = util.formatDate(new Date(event.detail));
        console.log("date: " + dateStr);
        this.data.reservation.ReservationForDate = dateStr;
    },
    onPeriodsChange: function (event) {
        console.log(event);
        var idxs = new Array();
        for (var _i = 0, _a = event.detail; _i < _a.length; _i++) {
            var name_1 = _a[_i];
            var idx = Number.parseInt(name_1.substr(7));
            idxs.push(idx);
        }
        for (var _b = 0, _c = this.data.reservationPeriods; _b < _c.length; _b++) {
            var p = _c[_b];
            if (idxs.indexOf(p.PeriodIndex) > -1) {
                p.Checked = true;
            }
        }
        this.setData({
            checkedPeriods: event.detail
        });
    },
    onUnitChange: function (event) {
        console.log(event);
        this.data.reservation.ReservationUnit = event.detail;
    },
    onActivityContentChange: function (event) {
        console.log(event);
        this.data.reservation.ReservationActivityContent = event.detail;
    },
    onPersonNameChange: function (event) {
        console.log(event);
        this.data.reservation.ReservationPersonName = event.detail;
    },
    onPersonPhoneChange: function (event) {
        console.log(event);
        this.data.reservation.ReservationPersonPhone = event.detail;
    },
    validateInputParams: function () {
        if (!this.data.reservation.ReservationUnit) {
            this.setData({
                unitErr: "预约单位不能为空"
            });
            return false;
        }
        if (this.data.reservation.ReservationUnit.length < 2 || this.data.reservation.ReservationUnit.length > 16) {
            this.setData({
                unitErr: "预约单位长度需要在 2 与 16 之间"
            });
            return false;
        }
        if (this.data.unitErr) {
            this.setData({
                unitErr: ""
            });
        }
        if (!this.data.reservation.ReservationActivityContent) {
            this.setData({
                acContentErr: "活动内容不能为空"
            });
            return false;
        }
        if (this.data.reservation.ReservationActivityContent.length < 2 || this.data.reservation.ReservationActivityContent.length > 16) {
            this.setData({
                acContentErr: "活动内容长度需要在 2 与 16 之间"
            });
            return false;
        }
        if (this.data.acContentErr) {
            this.setData({
                acContentErr: ""
            });
        }
        if (!this.data.reservation.ReservationPersonName) {
            this.setData({
                pNameErr: "预约人名称不能为空"
            });
            return false;
        }
        if (this.data.reservation.ReservationPersonName.length < 2 || this.data.reservation.ReservationPersonName.length > 16) {
            this.setData({
                pNameErr: "预约人名称长度需要在 2 与 16 之间"
            });
            return false;
        }
        if (this.data.pNameErr) {
            this.setData({
                pNameErr: ""
            });
        }
        if (!this.data.reservation.ReservationPersonPhone) {
            this.setData({
                pPhoneErr: "预约人手机号不能为空"
            });
            return false;
        }
        if (!/^1[3-9]\d{9}$/.test(this.data.reservation.ReservationPersonPhone)) {
            this.setData({
                pPhoneErr: "预约人手机号不合法"
            });
            return false;
        }
        if (this.data.pPhoneErr) {
            this.setData({
                pPhoneErr: ""
            });
        }
        return true;
    },
    submit: function (event) {
        if (!this.validateInputParams()) {
            return;
        }
        reservationSvc.NewReservation(function (result) {
            console.log(result);
            if (result.Status == 200) {
                wx.reLaunch({
                    url: '/pages/index/index'
                });
            }
        }, this.data.reservation, 'None', '');
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmV3LXJlc2VydmF0aW9uLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibmV3LXJlc2VydmF0aW9uLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O0FBQUEsd0VBQXVFO0FBQ3ZFLGtGQUFpRjtBQUNqRix3REFBdUQ7QUFJdkQsdUNBQXlDO0FBRXpDLElBQU0sY0FBYyxHQUFHLElBQUksdUNBQWtCLEVBQUUsQ0FBQztBQUNoRCxJQUFNLG1CQUFtQixHQUFHLElBQUksaURBQXVCLEVBQUUsQ0FBQztBQUUxRCxJQUFJLENBQUM7SUFDSCxJQUFJLEVBQUU7UUFDSixTQUFTLEVBQUUsQ0FBQztRQUNaLE1BQU0sRUFBRSxFQUE2QjtRQUNyQyxVQUFVLEVBQUUsRUFBbUI7UUFDL0IsV0FBVyxFQUFFLElBQUksSUFBSSxFQUFFLENBQUMsT0FBTyxFQUFFO1FBQ2pDLE9BQU8sRUFBRSxJQUFJLENBQUMsT0FBTyxDQUFDLElBQUksSUFBSSxFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsT0FBTyxFQUFFO1FBQzlDLFdBQVcsRUFBRSxJQUFJLHlCQUFXLEVBQUU7UUFDOUIsa0JBQWtCLEVBQUUsRUFBOEI7UUFDbEQsY0FBYyxFQUFFLEVBQW1CO1FBRW5DLGtCQUFrQixFQUFFLENBQUM7UUFFckIsT0FBTyxFQUFFLEVBQUU7UUFDWCxZQUFZLEVBQUUsRUFBRTtRQUNoQixRQUFRLEVBQUUsRUFBRTtRQUNaLFNBQVMsRUFBRSxFQUFFO0tBQ2Q7SUFDRCxNQUFNLEVBQU4sVUFBTyxNQUFXO1FBQWxCLGlCQVNDO1FBUkMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxNQUFNLENBQUMsQ0FBQztRQUVwQixtQkFBbUIsQ0FBQyxNQUFNLENBQUMsVUFBQyxNQUFNO1lBQzFCLEtBQUssQ0FBQyxPQUFPLENBQUM7Z0JBQ2xCLE1BQU0sRUFBRSxNQUFNO2dCQUNkLFVBQVUsRUFBRSxNQUFNLENBQUMsR0FBRyxDQUFDLFVBQUEsQ0FBQyxJQUFJLE9BQUEsQ0FBQyxDQUFDLFNBQVMsRUFB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QUFHLENBQUMsSUFBSSxDQUFDLG1CQUFtQixFQUFFLEVBQUM7WUFDN0IsT0FBTztTQUNSO1FBRUQsY0FBYyxDQUFDLGNBQWMsQ0FBQyxVQUFBLE1BQU07WUFDbEMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxNQUFNLENBQUMsQ0FBQztZQUNwQixJQUFHLE1BQU0sQ0FBQyxNQUFNLElBQUksR0FBRyxFQUFDO2dCQUN0QixFQUFFLENBQUMsUUFBUSxDQUFDO29CQUNWLEdBQUcsRUFBRSxvQkFBb0I7aUJBQzFCLENBQUMsQ0FBQzthQUNKO1FBQ0gsQ0FBQyxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFLE1BQU0sRUFBRSxFQUFFLENBQUMsQ0FBQztJQUN4QyxDQUFDO0NBQ0YsQ0FBQyxDQUFBIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUmVzZXJ2YXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvUmVzZXJ2YXRpb25TZXJ2aWNlJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9SZXNlcnZhdGlvblBsYWNlU2VydmljZSc7XHJcbmltcG9ydCB7IFJlc2VydmF0aW9uIH0gZnJvbSAnLi4vLi4vbW9kZWxzL1Jlc2VydmF0aW9uJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb25QbGFjZSB9IGZyb20gJy4uLy4uL21vZGVscy9SZXNlcnZhdGlvblBsYWNlJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb25QZXJpb2QgfSBmcm9tICcuLi8uLi9tb2RlbHMvUmVzZXJ2YXRpb25QZXJpb2QnO1xyXG5cclxuaW1wb3J0ICogYXMgdXRpbCBmcm9tICcuLi8uLi91dGlscy91dGlsJztcclxuXHJcbmNvbnN0IHJlc2VydmF0aW9uU3ZjID0gbmV3IFJlc2VydmF0aW9uU2VydmljZSgpO1xyXG5jb25zdCByZXNlcnZhdGlvblBsYWNlU3ZjID0gbmV3IFJlc2VydmF0aW9uUGxhY2VTZXJ2aWNlKCk7XHJcblxyXG5QYWdlKHtcclxuICBkYXRhOiB7XHJcbiAgICBzdGVwSW5kZXg6IDAsXHJcbiAgICBwbGFjZXM6IFtdIGFzIEFycmF5PFJlc2VydmF0aW9uUGxhY2U+LFxyXG4gICAgcGxhY2VOYW1lczogW10gYXMgQXJyYXk8c3RyaW5nPixcclxuICAgIGN1cnJlbnREYXRlOiBuZXcgRGF0ZSgpLmdldFRpbWUoKSxcclxuICAgIG1heERhdGU6IHV0aWwuYWRkRGF5cyhuZXcgRGF0ZSgpLCA3KS5nZXRUaW1lKCksXHJcbiAgICByZXNlcnZhdGlvbjogbmV3IFJlc2VydmF0aW9uKCksXHJcbiAgICByZXNlcnZhdGlvblBlcmlvZHM6IFtdIGFzIEFycmF5PFJlc2VydmF0aW9uUGVyaW9kPixcclxuICAgIGNoZWNrZWRQZXJpb2RzOiBbXSBhcyBBcnJheTxzdHJpbmc+LFxyXG4gICAgXHJcbiAgICBzZWxlY3RlZFBsYWNlSW5kZXg6IDAsXHJcblxyXG4gICAgdW5pdEVycjogXCJcIixcclxuICAgIGFjQ29udGVudEVycjogXCJcIixcclxuICAgIHBOYW1lRXJyOiBcIlwiLFxyXG4gICAgcFBob25lRXJyOiBcIlwiXHJcbiAgfSxcclxuICBvbkxvYWQocGFyYW1zOiBhbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKHBhcmFtcyk7XHJcblxyXG4gICAgcmVzZXJ2YXRpb25QbGFjZVN2Yy5HZXRBbGwoKHJlc3VsdCkgPT4ge1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBwbGFjZXM6IHJlc3VsdCxcclxuICAgICAgICBwbGFjZU5hbWVzOiByZXN1bHQubWFwKHAgPT4gcC5QbGFjZU5hbWUpXHJcbiAgICAgIH0pO1xyXG4gICAgfSlcclxuICB9LFxyXG4gIG9uU3RlcENoYW5nZSgpOiBib29sZWFuIHtcclxuICAgIGNvbnNvbGUubG9nKGBzdGVwSW5kZXg6JHt0aGlzLmRhdGEuc3RlcEluZGV4fSxyZXNlcnZhdGlvbkluZm86ICR7SlNPTi5zdHJpbmdpZnkodGhpcy5kYXRhLnJlc2VydmF0aW9uKX1gKTtcclxuICAgIHN3aXRjaCAodGhpcy5kYXRhLnN0ZXBJbmRleCkge1xyXG4gICAgICBjYXNlIDA6XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICAgIGNhc2UgMTpcclxuICAgICAgICBpZiAoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBsYWNlSWQpIHtcclxuICAgICAgICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBsYWNlSWQgPSB0aGlzLmRhdGEucGxhY2VzWzBdLlBsYWNlSWQ7XHJcbiAgICAgICAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QbGFjZU5hbWUgPSB0aGlzLmRhdGEucGxhY2VzWzBdLlBsYWNlTmFtZTtcclxuICAgICAgICB9XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICAgIGNhc2UgMjpcclxuICAgICAgICBpZiAoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkZvckRhdGUpIHtcclxuICAgICAgICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkZvckRhdGUgPSB1dGlsLmZvcm1hdERhdGUobmV3IERhdGUoKSk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIC8vXHJcbiAgICAgICAgcmVzZXJ2YXRpb25QbGFjZVN2Yy5nZXRBdmFpbGFibGVQZXJpb2RzKHJlc3VsdCA9PiB7XHJcbiAgICAgICAgICBjb25zb2xlLmxvZyhyZXN1bHQpO1xyXG4gICAgICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgICAgIHJlc2VydmF0aW9uUGVyaW9kczogcmVzdWx0LFxyXG4gICAgICAgICAgICBjaGVja2VkUGVyaW9kczogW11cclxuICAgICAgICAgIH0pO1xyXG4gICAgICAgIH0sIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBsYWNlSWQsIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkZvckRhdGUpO1xyXG5cclxuICAgICAgICBicmVhaztcclxuXHJcbiAgICAgIGNhc2UgMzpcclxuICAgICAgICBpZih0aGlzLmRhdGEuY2hlY2tlZFBlcmlvZHMubGVuZ3RoID09IDApe1xyXG4gICAgICAgICAgd3guc2hvd1RvYXN0KHtcclxuICAgICAgICAgICAgdGl0bGU6IFwi6K+36YCJ5oup6KaB6aKE57qm55qE5pe26Ze05q615oiW6L+U5Zue5LiK5LiA5q2l6YCJ5oup5YW25LuW6aKE57qm5pel5pyfXCIsXHJcbiAgICAgICAgICAgIGR1cmF0aW9uOiAyMDAwLFxyXG4gICAgICAgICAgICBpY29uOiBcIm5vbmVcIlxyXG4gICAgICAgICAgfSk7XHJcbiAgICAgICAgICB0aGlzLmRhdGEuc3RlcEluZGV4LS07XHJcbiAgICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGNvbnNvbGUubG9nKHRoaXMuZGF0YS5yZXNlcnZhdGlvblBlcmlvZHMpO1xyXG4gICAgICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkZvclRpbWVJZHMgPSB0aGlzLmRhdGEucmVzZXJ2YXRpb25QZXJpb2RzLmZpbHRlcihfPT5fLkNoZWNrZWQpLm1hcCh4PT54LlBlcmlvZEluZGV4KS5qb2luKFwiLFwiKTtcclxuICAgICAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25Gb3JUaW1lID0gdGhpcy5kYXRhLnJlc2VydmF0aW9uUGVyaW9kcy5maWx0ZXIoXz0+Xy5DaGVja2VkKS5tYXAoeD0+IHguUGVyaW9kVGl0bGUpLmpvaW4oXCIsXCIpO1xyXG4gICAgXHJcbiAgICAgICAgYnJlYWs7XHJcblxyXG4gICAgICBjYXNlIDQ6XHJcbiAgICAgICAgaWYodGhpcy52YWxpZGF0ZUlucHV0UGFyYW1zKCkpe1xyXG4gICAgICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgICAgIHJlc2VydmF0aW9uOiB0aGlzLmRhdGEucmVzZXJ2YXRpb25cclxuICAgICAgICAgIH0pO1xyXG4gICAgICAgIH1lbHNle1xyXG4gICAgICAgICAgdGhpcy5kYXRhLnN0ZXBJbmRleC0tO1xyXG4gICAgICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgICAgIH1cclxuICAgICAgICBcclxuICAgICAgICBicmVhaztcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gdHJ1ZTtcclxuICB9LFxyXG5cclxuICBwcmV2U3RlcChldmVudDogYW55KSB7XHJcbiAgICB0aGlzLmRhdGEuc3RlcEluZGV4LS07XHJcbiAgICB0aGlzLm9uU3RlcENoYW5nZSgpO1xyXG4gICAgLy9cclxuICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICBzdGVwSW5kZXg6IHRoaXMuZGF0YS5zdGVwSW5kZXhcclxuICAgIH0pO1xyXG4gIH0sXHJcbiAgbmV4dFN0ZXAoZXZlbnQ6IGFueSkge1xyXG4gICAgdGhpcy5kYXRhLnN0ZXBJbmRleCsrO1xyXG4gICAgdGhpcy5vblN0ZXBDaGFuZ2UoKTtcclxuICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICBzdGVwSW5kZXg6IHRoaXMuZGF0YS5zdGVwSW5kZXhcclxuICAgIH0pO1xyXG4gIH0sXHJcblxyXG4gIG9uUGxhY2VDaGFuZ2UoZXZlbnQ6IGFueSkge1xyXG4gICAgY29uc3QgeyBwaWNrZXIsIHZhbHVlLCBpbmRleCB9ID0gZXZlbnQuZGV0YWlsO1xyXG4gICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgIHNlbGVjdGVkUGxhY2VJbmRleDogaW5kZXhcclxuICAgIH0pO1xyXG4gICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGxhY2VJZCA9IHRoaXMuZGF0YS5wbGFjZXNbaW5kZXhdLlBsYWNlSWQ7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QbGFjZU5hbWUgPSB0aGlzLmRhdGEucGxhY2VzW2luZGV4XS5QbGFjZU5hbWU7XHJcbiAgfSxcclxuXHJcbiAgb25EYXRlSW5wdXQoZXZlbnQ6IGFueSkge1xyXG4gICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgIGN1cnJlbnREYXRlOiBldmVudC5kZXRhaWxcclxuICAgIH0pO1xyXG4gICAgbGV0IGRhdGVTdHIgPSB1dGlsLmZvcm1hdERhdGUobmV3IERhdGUoZXZlbnQuZGV0YWlsKSk7XHJcbiAgICBjb25zb2xlLmxvZyhgZGF0ZTogJHtkYXRlU3RyfWApO1xyXG4gICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yRGF0ZSA9IGRhdGVTdHI7XHJcblxyXG4gIH0sXHJcblxyXG4gIG9uUGVyaW9kc0NoYW5nZShldmVudDogYW55KSB7XHJcbiAgICBjb25zb2xlLmxvZyhldmVudCk7XHJcblxyXG4gICAgbGV0IGlkeHMgPSBuZXcgQXJyYXk8bnVtYmVyPigpO1xyXG4gICAgZm9yIChsZXQgbmFtZSBvZiAoZXZlbnQuZGV0YWlsIGFzIEFycmF5PHN0cmluZz4pKSB7XHJcbiAgICAgIGxldCBpZHggPSBOdW1iZXIucGFyc2VJbnQobmFtZS5zdWJzdHIoNykpOyAgICAgIFxyXG4gICAgICBpZHhzLnB1c2goaWR4KTtcclxuICAgIH1cclxuICAgIGZvcihsZXQgcCBvZiB0aGlzLmRhdGEucmVzZXJ2YXRpb25QZXJpb2RzKXtcclxuICAgICAgaWYoaWR4cy5pbmRleE9mKHAuUGVyaW9kSW5kZXgpID4gLTEpe1xyXG4gICAgICAgIHAuQ2hlY2tlZCA9IHRydWU7XHJcbiAgICAgIH1cclxuICAgIH1cclxuXHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgY2hlY2tlZFBlcmlvZHM6IGV2ZW50LmRldGFpbFxyXG4gICAgfSk7XHJcbiAgfSxcclxuXHJcbiAgb25Vbml0Q2hhbmdlKGV2ZW50OiBhbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKGV2ZW50KTtcclxuICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblVuaXQgPSBldmVudC5kZXRhaWw7XHJcbiAgfSxcclxuICBvbkFjdGl2aXR5Q29udGVudENoYW5nZShldmVudDogYW55KSB7XHJcbiAgICBjb25zb2xlLmxvZyhldmVudCk7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25BY3Rpdml0eUNvbnRlbnQgPSBldmVudC5kZXRhaWw7XHJcbiAgfSxcclxuICBvblBlcnNvbk5hbWVDaGFuZ2UoZXZlbnQ6IGFueSkge1xyXG4gICAgY29uc29sZS5sb2coZXZlbnQpO1xyXG4gICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uTmFtZSA9IGV2ZW50LmRldGFpbDtcclxuICB9LFxyXG4gIG9uUGVyc29uUGhvbmVDaGFuZ2UoZXZlbnQ6IGFueSkge1xyXG4gICAgY29uc29sZS5sb2coZXZlbnQpO1xyXG4gICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uUGhvbmUgPSBldmVudC5kZXRhaWw7XHJcbiAgfSxcclxuXHJcblxyXG4gIHZhbGlkYXRlSW5wdXRQYXJhbXMoKTogYm9vbGVhbntcclxuICAgIGlmKCF0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25Vbml0KXtcclxuICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgdW5pdEVycjogXCLpooTnuqbljZXkvY3kuI3og73kuLrnqbpcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uVW5pdC5sZW5ndGggPCAyIHx8IHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblVuaXQubGVuZ3RoID4gMTYpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICB1bml0RXJyOiBcIumihOe6puWNleS9jemVv+W6pumcgOimgeWcqCAyIOS4jiAxNiDkuYvpl7RcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnVuaXRFcnIpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICB1bml0RXJyOiBcIlwiXHJcbiAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIGlmKCF0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25BY3Rpdml0eUNvbnRlbnQpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBhY0NvbnRlbnRFcnI6IFwi5rS75Yqo5YaF5a655LiN6IO95Li656m6XCJcclxuICAgICAgfSk7XHJcbiAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH1cclxuICAgIGlmKHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkFjdGl2aXR5Q29udGVudC5sZW5ndGggPCAyIHx8IHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkFjdGl2aXR5Q29udGVudC5sZW5ndGggPiAxNil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIGFjQ29udGVudEVycjogXCLmtLvliqjlhoXlrrnplb/luqbpnIDopoHlnKggMiDkuI4gMTYg5LmL6Ze0XCJcclxuICAgICAgfSk7XHJcbiAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH1cclxuICAgIGlmKHRoaXMuZGF0YS5hY0NvbnRlbnRFcnIpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBhY0NvbnRlbnRFcnI6IFwiXCJcclxuICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBlcnNvbk5hbWUpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBwTmFtZUVycjogXCLpooTnuqbkurrlkI3np7DkuI3og73kuLrnqbpcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uTmFtZS5sZW5ndGggPCAyIHx8IHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBlcnNvbk5hbWUubGVuZ3RoID4gMTYpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBwTmFtZUVycjogXCLpooTnuqbkurrlkI3np7Dplb/luqbpnIDopoHlnKggMiDkuI4gMTYg5LmL6Ze0XCJcclxuICAgICAgfSk7XHJcbiAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH1cclxuICAgIGlmKHRoaXMuZGF0YS5wTmFtZUVycil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBOYW1lRXJyOiBcIlwiXHJcbiAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIGlmKCF0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QZXJzb25QaG9uZSl7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBQaG9uZUVycjogXCLpooTnuqbkurrmiYvmnLrlj7fkuI3og73kuLrnqbpcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYoIS9eMVszLTldXFxkezl9JC8udGVzdCh0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QZXJzb25QaG9uZSkpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBwUGhvbmVFcnI6IFwi6aKE57qm5Lq65omL5py65Y+35LiN5ZCI5rOVXCJcclxuICAgICAgfSk7XHJcbiAgICAgIHJldHVybiBmYWxzZTtcclxuICAgIH1cclxuICAgIGlmKHRoaXMuZGF0YS5wUGhvbmVFcnIpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBwUGhvbmVFcnI6IFwiXCJcclxuICAgICAgfSk7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gdHJ1ZTtcclxuICB9LFxyXG5cclxuICBzdWJtaXQoZXZlbnQ6IGFueSkge1xyXG4gICAgLy8gdmFsaWRhdGUgcGFyYW0gbmFtZVxyXG4gICAgaWYoIXRoaXMudmFsaWRhdGVJbnB1dFBhcmFtcygpKXtcclxuICAgICAgcmV0dXJuO1xyXG4gICAgfVxyXG4gICAgLy9cclxuICAgIHJlc2VydmF0aW9uU3ZjLk5ld1Jlc2VydmF0aW9uKHJlc3VsdCA9PiB7XHJcbiAgICAgIGNvbnNvbGUubG9nKHJlc3VsdCk7XHJcbiAgICAgIGlmKHJlc3VsdC5TdGF0dXMgPT0gMjAwKXtcclxuICAgICAgICB3eC5yZUxhdW5jaCh7XHJcbiAgICAgICAgICB1cmw6ICcvcGFnZXMvaW5kZXgvaW5kZXgnXHJcbiAgICAgICAgfSk7XHJcbiAgICAgIH1cclxuICAgIH0sIHRoaXMuZGF0YS5yZXNlcnZhdGlvbiwgJ05vbmUnLCAnJyk7XHJcbiAgfVxyXG59KSJdfQ==