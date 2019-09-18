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
        minDate: new Date().getTime(),
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
            else {
                p.Checked = false;
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
                wx.showToast({
                    title: "预约成功",
                    icon: "success",
                    duration: 2000,
                    complete: function (res) {
                        wx.reLaunch({
                            url: '/pages/index/index'
                        });
                    }
                });
            }
            else {
                wx.showToast({
                    title: "\u9884\u7EA6\u5931\u8D25," + result.ErrorMsg,
                    icon: "none",
                    duration: 2000
                });
            }
        }, this.data.reservation, 'None', '');
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmV3LXJlc2VydmF0aW9uLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibmV3LXJlc2VydmF0aW9uLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O0FBQUEsd0VBQXVFO0FBQ3ZFLGtGQUFpRjtBQUNqRix3REFBdUQ7QUFJdkQsdUNBQXlDO0FBRXpDLElBQU0sY0FBYyxHQUFHLElBQUksdUNBQWtCLEVBQUUsQ0FBQztBQUNoRCxJQUFNLG1CQUFtQixHQUFHLElBQUksaURBQXVCLEVBQUUsQ0FBQztBQUUxRCxJQUFJLENBQUM7SUFDSCxJQUFJLEVBQUU7UUFDSixTQUFTLEVBQUUsQ0FBQztRQUNaLE1BQU0sRUFBRSxFQUE2QjtRQUNyQyxVQUFVLEVBQUUsRUFBbUI7UUFDL0IsV0FBVyxFQUFFLElBQUksSUFBSSxFQUFFLENBQUMsT0FBTyxFQUFFO1FBQ2pDLE9BQU8sRUFBRSxJQUFJLElBQUksRUFBRSxDQUFDLE9BQU8sRUFBRTtRQUM3QixPQUFPLEVBQUUsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLElBQUksRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLE9BQU8sRUFBRTtRQUM5QyxXQUFXLEVBQUUsSUFBSSx5QkFBVyxFQUFFO1FBQzlCLGtCQUFrQixFQUFFLEVBQThCO1FBQ2xELGNBQWMsRUFBRSxFQUFtQjtRQUVuQyxrQkFBa0IsRUFBRSxDQUFDO1FBRXJCLE9BQU8sRUFBRSxFQUFFO1FBQ1gsWUFBWSxFQUFFLEVBQUU7UUFDaEIsUUFBUSxFQUFFLEVBQUU7UUFDWixTQUFTLEVBQUUsRUFBRTtLQUNkO0lBQ0QsTUFBTSxFQUFOLFVBQU8sTUFBVztRQUFsQixpQkFTQztRQVJDLE9BQU8sQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUM7UUFFcEIsbUJBQW1CLENBQUMsTUFBTSxDQUFDLFVBQUMsTUFBTTtZQUMxQixLQUFLLENBQUMsT0FBTyxDQUFDO2dCQUNsQixNQUFNLEVBQUUsTUFBTTtnQkFDZCxVQUFVLEVBQUUsTUFBTSxDQUFDLEdB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QVMsRUFBRSxFQUFFO2FBQ2QsQ0FBQyxDQUFDO1NBQ0o7UUFDRCxPQUFPLElBQUksQ0FBQztJQUNkLENBQUM7SUFFRCxNQUFNLEVBQU4sVUFBTyxLQUFVO1FBRWYsSUFBRyxDQUFDLElBQUksQ0FBQyxtQkFBbUIsRUFBRSxFQUFDO1lBQzdCLE9BQU87U0FDUjtRQUVELGNBQWMsQ0FBQyxjQUFjLENBQUMsVUFBQSxNQUFNO1lBQ2xDLE9BQU8sQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUM7WUFDcEIsSUFBRyxNQUFNLENBQUMsTUFBTSxJQUFJLEdBQUcsRUFBQztnQkFDdEIsRUFBRSxDQUFDLFNBQVMsQ0FBQztvQkFDWCxLQUFLLEVBQUUsTUFBTTtvQkFDYixJQUFJLEVBQUUsU0FBUztvQkFDZixRQUFRLEVBQUUsSUFBSTtvQkFDZCxRQUFRLEVBQUUsVUFBQSxHQUFHO3dCQUNYLEVBQUUsQ0FBQyxRQUFRLENBQUM7NEJBQ1YsR0FBRyxFQUFFLG9CQUFvQjt5QkFDMUIsQ0FBQyxDQUFDO29CQUNMLENBQUM7aUJBQ0YsQ0FBQyxDQUFDO2FBQ0o7aUJBQU07Z0JBQ0wsRUFBRSxDQUFDLFNBQVMsQ0FBQztvQkFDWCxLQUFLLEVBQUUsOEJBQVEsTUFBTSxDQUFDLFFBQVU7b0JBQ2hDLElBQUksRUFBRSxNQUFNO29CQUNaLFFBQVEsRUFBRSxJQUFJO2lCQUNmLENBQUMsQ0FBQzthQUNKO1FBQ0gsQ0FBQyxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFLE1BQU0sRUFBRSxFQUFFLENBQUMsQ0FBQztJQUN4QyxDQUFDO0NBQ0YsQ0FBQyxDQUFBIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUmVzZXJ2YXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvUmVzZXJ2YXRpb25TZXJ2aWNlJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9SZXNlcnZhdGlvblBsYWNlU2VydmljZSc7XHJcbmltcG9ydCB7IFJlc2VydmF0aW9uIH0gZnJvbSAnLi4vLi4vbW9kZWxzL1Jlc2VydmF0aW9uJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb25QbGFjZSB9IGZyb20gJy4uLy4uL21vZGVscy9SZXNlcnZhdGlvblBsYWNlJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb25QZXJpb2QgfSBmcm9tICcuLi8uLi9tb2RlbHMvUmVzZXJ2YXRpb25QZXJpb2QnO1xyXG5cclxuaW1wb3J0ICogYXMgdXRpbCBmcm9tICcuLi8uLi91dGlscy91dGlsJztcclxuXHJcbmNvbnN0IHJlc2VydmF0aW9uU3ZjID0gbmV3IFJlc2VydmF0aW9uU2VydmljZSgpO1xyXG5jb25zdCByZXNlcnZhdGlvblBsYWNlU3ZjID0gbmV3IFJlc2VydmF0aW9uUGxhY2VTZXJ2aWNlKCk7XHJcblxyXG5QYWdlKHtcclxuICBkYXRhOiB7XHJcbiAgICBzdGVwSW5kZXg6IDAsXHJcbiAgICBwbGFjZXM6IFtdIGFzIEFycmF5PFJlc2VydmF0aW9uUGxhY2U+LFxyXG4gICAgcGxhY2VOYW1lczogW10gYXMgQXJyYXk8c3RyaW5nPixcclxuICAgIGN1cnJlbnREYXRlOiBuZXcgRGF0ZSgpLmdldFRpbWUoKSxcclxuICAgIG1pbkRhdGU6IG5ldyBEYXRlKCkuZ2V0VGltZSgpLFxyXG4gICAgbWF4RGF0ZTogdXRpbC5hZGREYXlzKG5ldyBEYXRlKCksIDcpLmdldFRpbWUoKSxcclxuICAgIHJlc2VydmF0aW9uOiBuZXcgUmVzZXJ2YXRpb24oKSxcclxuICAgIHJlc2VydmF0aW9uUGVyaW9kczogW10gYXMgQXJyYXk8UmVzZXJ2YXRpb25QZXJpb2Q+LFxyXG4gICAgY2hlY2tlZFBlcmlvZHM6IFtdIGFzIEFycmF5PHN0cmluZz4sXHJcbiAgICBcclxuICAgIHNlbGVjdGVkUGxhY2VJbmRleDogMCxcclxuXHJcbiAgICB1bml0RXJyOiBcIlwiLFxyXG4gICAgYWNDb250ZW50RXJyOiBcIlwiLFxyXG4gICAgcE5hbWVFcnI6IFwiXCIsXHJcbiAgICBwUGhvbmVFcnI6IFwiXCJcclxuICB9LFxyXG4gIG9uTG9hZChwYXJhbXM6IGFueSkge1xyXG4gICAgY29uc29sZS5sb2cocGFyYW1zKTtcclxuXHJcbiAgICByZXNlcnZhdGlvblBsYWNlU3ZjLkdldEFsbCgocmVzdWx0KSA9PiB7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBsYWNlczogcmVzdWx0LFxyXG4gICAgICAgIHBsYWNlTmFtZXM6IHJlc3VsdC5tYXAocCA9PiBwLlBsYWNlTmFtZSlcclxuICAgICAgfSk7XHJcbiAgICB9KVxyXG4gIH0sXHJcbiAgb25TdGVwQ2hhbmdlKCk6IGJvb2xlYW4ge1xyXG4gICAgY29uc29sZS5sb2coYHN0ZXBJbmRleDoke3RoaXMuZGF0YS5zdGVwSW5kZXh9LHJlc2VydmF0aW9uSW5mbzogJHtKU09OLnN0cmluZ2lmeSh0aGlzLmRhdGEucmVzZXJ2YXRpb24pfWApO1xyXG4gICAgc3dpdGNoICh0aGlzLmRhdGEuc3RlcEluZGV4KSB7XHJcbiAgICAgIGNhc2UgMDpcclxuICAgICAgICBicmVhaztcclxuICAgICAgY2FzZSAxOlxyXG4gICAgICAgIGlmICghdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGxhY2VJZCkge1xyXG4gICAgICAgICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGxhY2VJZCA9IHRoaXMuZGF0YS5wbGFjZXNbMF0uUGxhY2VJZDtcclxuICAgICAgICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBsYWNlTmFtZSA9IHRoaXMuZGF0YS5wbGFjZXNbMF0uUGxhY2VOYW1lO1xyXG4gICAgICAgIH1cclxuICAgICAgICBicmVhaztcclxuICAgICAgY2FzZSAyOlxyXG4gICAgICAgIGlmICghdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yRGF0ZSkge1xyXG4gICAgICAgICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yRGF0ZSA9IHV0aWwuZm9ybWF0RGF0ZShuZXcgRGF0ZSgpKTtcclxuICAgICAgICB9XHJcbiAgICAgICAgLy9cclxuICAgICAgICByZXNlcnZhdGlvblBsYWNlU3ZjLmdldEF2YWlsYWJsZVBlcmlvZHMocmVzdWx0ID0+IHtcclxuICAgICAgICAgIGNvbnNvbGUubG9nKHJlc3VsdCk7XHJcbiAgICAgICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICAgICAgcmVzZXJ2YXRpb25QZXJpb2RzOiByZXN1bHQsXHJcbiAgICAgICAgICAgIGNoZWNrZWRQZXJpb2RzOiBbXVxyXG4gICAgICAgICAgfSk7XHJcbiAgICAgICAgfSwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGxhY2VJZCwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yRGF0ZSk7XHJcblxyXG4gICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgY2FzZSAzOlxyXG4gICAgICAgIGlmKHRoaXMuZGF0YS5jaGVja2VkUGVyaW9kcy5sZW5ndGggPT0gMCl7XHJcbiAgICAgICAgICB3eC5zaG93VG9hc3Qoe1xyXG4gICAgICAgICAgICB0aXRsZTogXCLor7fpgInmi6nopoHpooTnuqbnmoTml7bpl7TmrrXmiJbov5Tlm57kuIrkuIDmraXpgInmi6nlhbbku5bpooTnuqbml6XmnJ9cIixcclxuICAgICAgICAgICAgZHVyYXRpb246IDIwMDAsXHJcbiAgICAgICAgICAgIGljb246IFwibm9uZVwiXHJcbiAgICAgICAgICB9KTtcclxuICAgICAgICAgIHRoaXMuZGF0YS5zdGVwSW5kZXgtLTtcclxuICAgICAgICAgIHJldHVybiBmYWxzZTtcclxuICAgICAgICB9XHJcbiAgICAgICAgY29uc29sZS5sb2codGhpcy5kYXRhLnJlc2VydmF0aW9uUGVyaW9kcyk7XHJcbiAgICAgICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yVGltZUlkcyA9IHRoaXMuZGF0YS5yZXNlcnZhdGlvblBlcmlvZHMuZmlsdGVyKF89Pl8uQ2hlY2tlZCkubWFwKHg9PnguUGVyaW9kSW5kZXgpLmpvaW4oXCIsXCIpO1xyXG4gICAgICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkZvclRpbWUgPSB0aGlzLmRhdGEucmVzZXJ2YXRpb25QZXJpb2RzLmZpbHRlcihfPT5fLkNoZWNrZWQpLm1hcCh4PT4geC5QZXJpb2RUaXRsZSkuam9pbihcIixcIik7XHJcbiAgICBcclxuICAgICAgICBicmVhaztcclxuXHJcbiAgICAgIGNhc2UgNDpcclxuICAgICAgICBpZih0aGlzLnZhbGlkYXRlSW5wdXRQYXJhbXMoKSl7XHJcbiAgICAgICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICAgICAgcmVzZXJ2YXRpb246IHRoaXMuZGF0YS5yZXNlcnZhdGlvblxyXG4gICAgICAgICAgfSk7XHJcbiAgICAgICAgfWVsc2V7XHJcbiAgICAgICAgICB0aGlzLmRhdGEuc3RlcEluZGV4LS07XHJcbiAgICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIFxyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiB0cnVlO1xyXG4gIH0sXHJcblxyXG4gIHByZXZTdGVwKGV2ZW50OiBhbnkpIHtcclxuICAgIHRoaXMuZGF0YS5zdGVwSW5kZXgtLTtcclxuICAgIHRoaXMub25TdGVwQ2hhbmdlKCk7XHJcbiAgICAvL1xyXG4gICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgIHN0ZXBJbmRleDogdGhpcy5kYXRhLnN0ZXBJbmRleFxyXG4gICAgfSk7XHJcbiAgfSxcclxuICBuZXh0U3RlcChldmVudDogYW55KSB7XHJcbiAgICB0aGlzLmRhdGEuc3RlcEluZGV4Kys7XHJcbiAgICB0aGlzLm9uU3RlcENoYW5nZSgpO1xyXG4gICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgIHN0ZXBJbmRleDogdGhpcy5kYXRhLnN0ZXBJbmRleFxyXG4gICAgfSk7XHJcbiAgfSxcclxuXHJcbiAgb25QbGFjZUNoYW5nZShldmVudDogYW55KSB7XHJcbiAgICBjb25zdCB7IHBpY2tlciwgdmFsdWUsIGluZGV4IH0gPSBldmVudC5kZXRhaWw7XHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgc2VsZWN0ZWRQbGFjZUluZGV4OiBpbmRleFxyXG4gICAgfSk7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QbGFjZUlkID0gdGhpcy5kYXRhLnBsYWNlc1tpbmRleF0uUGxhY2VJZDtcclxuICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBsYWNlTmFtZSA9IHRoaXMuZGF0YS5wbGFjZXNbaW5kZXhdLlBsYWNlTmFtZTtcclxuICB9LFxyXG5cclxuICBvbkRhdGVJbnB1dChldmVudDogYW55KSB7XHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgY3VycmVudERhdGU6IGV2ZW50LmRldGFpbFxyXG4gICAgfSk7XHJcbiAgICBsZXQgZGF0ZVN0ciA9IHV0aWwuZm9ybWF0RGF0ZShuZXcgRGF0ZShldmVudC5kZXRhaWwpKTtcclxuICAgIGNvbnNvbGUubG9nKGBkYXRlOiAke2RhdGVTdHJ9YCk7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25Gb3JEYXRlID0gZGF0ZVN0cjtcclxuXHJcbiAgfSxcclxuXHJcbiAgb25QZXJpb2RzQ2hhbmdlKGV2ZW50OiBhbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKGV2ZW50KTtcclxuXHJcbiAgICBsZXQgaWR4cyA9IG5ldyBBcnJheTxudW1iZXI+KCk7XHJcbiAgICBmb3IgKGxldCBuYW1lIG9mIChldmVudC5kZXRhaWwgYXMgQXJyYXk8c3RyaW5nPikpIHtcclxuICAgICAgbGV0IGlkeCA9IE51bWJlci5wYXJzZUludChuYW1lLnN1YnN0cig3KSk7ICAgICAgXHJcbiAgICAgIGlkeHMucHVzaChpZHgpO1xyXG4gICAgfVxyXG4gICAgZm9yKGxldCBwIG9mIHRoaXMuZGF0YS5yZXNlcnZhdGlvblBlcmlvZHMpe1xyXG4gICAgICBpZihpZHhzLmluZGV4T2YocC5QZXJpb2RJbmRleCkgPiAtMSl7XHJcbiAgICAgICAgcC5DaGVja2VkID0gdHJ1ZTtcclxuICAgICAgfWVsc2V7XHJcbiAgICAgICAgcC5DaGVja2VkID0gZmFsc2U7XHJcbiAgICAgIH1cclxuICAgIH1cclxuXHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgY2hlY2tlZFBlcmlvZHM6IGV2ZW50LmRldGFpbFxyXG4gICAgfSk7XHJcbiAgfSxcclxuXHJcbiAgb25Vbml0Q2hhbmdlKGV2ZW50OiBhbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKGV2ZW50KTtcclxuICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblVuaXQgPSBldmVudC5kZXRhaWw7XHJcbiAgfSxcclxuICBvbkFjdGl2aXR5Q29udGVudENoYW5nZShldmVudDogYW55KSB7XHJcbiAgICBjb25zb2xlLmxvZyhldmVudCk7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25BY3Rpdml0eUNvbnRlbnQgPSBldmVudC5kZXRhaWw7XHJcbiAgfSxcclxuICBvblBlcnNvbk5hbWVDaGFuZ2UoZXZlbnQ6IGFueSkge1xyXG4gICAgY29uc29sZS5sb2coZXZlbnQpO1xyXG4gICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uTmFtZSA9IGV2ZW50LmRldGFpbDtcclxuICB9LFxyXG4gIG9uUGVyc29uUGhvbmVDaGFuZ2UoZXZlbnQ6IGFueSkge1xyXG4gICAgY29uc29sZS5sb2coZXZlbnQpO1xyXG4gICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uUGhvbmUgPSBldmVudC5kZXRhaWw7XHJcbiAgfSxcclxuXHJcbiAgdmFsaWRhdGVJbnB1dFBhcmFtcygpOiBib29sZWFue1xyXG4gICAgaWYoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblVuaXQpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICB1bml0RXJyOiBcIumihOe6puWNleS9jeS4jeiDveS4uuepulwiXHJcbiAgICAgIH0pO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25Vbml0Lmxlbmd0aCA8IDIgfHwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uVW5pdC5sZW5ndGggPiAxNil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHVuaXRFcnI6IFwi6aKE57qm5Y2V5L2N6ZW/5bqm6ZyA6KaB5ZyoIDIg5LiOIDE2IOS5i+mXtFwiXHJcbiAgICAgIH0pO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLmRhdGEudW5pdEVycil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHVuaXRFcnI6IFwiXCJcclxuICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkFjdGl2aXR5Q29udGVudCl7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIGFjQ29udGVudEVycjogXCLmtLvliqjlhoXlrrnkuI3og73kuLrnqbpcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uQWN0aXZpdHlDb250ZW50Lmxlbmd0aCA8IDIgfHwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uQWN0aXZpdHlDb250ZW50Lmxlbmd0aCA+IDE2KXtcclxuICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgYWNDb250ZW50RXJyOiBcIua0u+WKqOWGheWuuemVv+W6pumcgOimgeWcqCAyIOS4jiAxNiDkuYvpl7RcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLmFjQ29udGVudEVycil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIGFjQ29udGVudEVycjogXCJcIlxyXG4gICAgICB9KTtcclxuICAgIH1cclxuXHJcbiAgICBpZighdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uTmFtZSl7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBOYW1lRXJyOiBcIumihOe6puS6uuWQjeensOS4jeiDveS4uuepulwiXHJcbiAgICAgIH0pO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QZXJzb25OYW1lLmxlbmd0aCA8IDIgfHwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uTmFtZS5sZW5ndGggPiAxNil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBOYW1lRXJyOiBcIumihOe6puS6uuWQjeensOmVv+W6pumcgOimgeWcqCAyIOS4jiAxNiDkuYvpl7RcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnBOYW1lRXJyKXtcclxuICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgcE5hbWVFcnI6IFwiXCJcclxuICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBlcnNvblBob25lKXtcclxuICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgcFBob25lRXJyOiBcIumihOe6puS6uuaJi+acuuWPt+S4jeiDveS4uuepulwiXHJcbiAgICAgIH0pO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcbiAgICBpZighL14xWzMtOV1cXGR7OX0kLy50ZXN0KHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBlcnNvblBob25lKSl7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBQaG9uZUVycjogXCLpooTnuqbkurrmiYvmnLrlj7fkuI3lkIjms5VcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnBQaG9uZUVycil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBQaG9uZUVycjogXCJcIlxyXG4gICAgICB9KTtcclxuICAgIH1cclxuICAgIHJldHVybiB0cnVlO1xyXG4gIH0sXHJcblxyXG4gIHN1Ym1pdChldmVudDogYW55KSB7XHJcbiAgICAvLyB2YWxpZGF0ZSBwYXJhbSBuYW1lXHJcbiAgICBpZighdGhpcy52YWxpZGF0ZUlucHV0UGFyYW1zKCkpe1xyXG4gICAgICByZXR1cm47XHJcbiAgICB9XHJcbiAgICAvL1xyXG4gICAgcmVzZXJ2YXRpb25TdmMuTmV3UmVzZXJ2YXRpb24ocmVzdWx0ID0+IHtcclxuICAgICAgY29uc29sZS5sb2cocmVzdWx0KTtcclxuICAgICAgaWYocmVzdWx0LlN0YXR1cyA9PSAyMDApe1xyXG4gICAgICAgIHd4LnNob3dUb2FzdCh7XHJcbiAgICAgICAgICB0aXRsZTogXCLpooTnuqbmiJDlip9cIixcclxuICAgICAgICAgIGljb246IFwic3VjY2Vzc1wiLFxyXG4gICAgICAgICAgZHVyYXRpb246IDIwMDAsXHJcbiAgICAgICAgICBjb21wbGV0ZTogcmVzID0+IHtcclxuICAgICAgICAgICAgd3gucmVMYXVuY2goe1xyXG4gICAgICAgICAgICAgIHVybDogJy9wYWdlcy9pbmRleC9pbmRleCdcclxuICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfSk7ICAgICAgICBcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICB3eC5zaG93VG9hc3Qoe1xyXG4gICAgICAgICAgdGl0bGU6IGDpooTnuqblpLHotKUsJHtyZXN1bHQuRXJyb3JNc2d9YCxcclxuICAgICAgICAgIGljb246IFwibm9uZVwiLFxyXG4gICAgICAgICAgZHVyYXRpb246IDIwMDBcclxuICAgICAgICB9KTtcclxuICAgICAgfVxyXG4gICAgfSwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLCAnTm9uZScsICcnKTtcclxuICB9XHJcbn0pIl19