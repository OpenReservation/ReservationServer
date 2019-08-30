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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmV3LXJlc2VydmF0aW9uLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibmV3LXJlc2VydmF0aW9uLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O0FBQUEsd0VBQXVFO0FBQ3ZFLGtGQUFpRjtBQUNqRix3REFBdUQ7QUFJdkQsdUNBQXlDO0FBRXpDLElBQU0sY0FBYyxHQUFHLElBQUksdUNBQWtCLEVBQUUsQ0FBQztBQUNoRCxJQUFNLG1CQUFtQixHQUFHLElBQUksaURBQXVCLEVBQUUsQ0FBQztBQUUxRCxJQUFJLENBQUM7SUFDSCxJQUFJLEVBQUU7UUFDSixTQUFTLEVBQUUsQ0FBQztRQUNaLE1BQU0sRUFBRSxFQUE2QjtRQUNyQyxVQUFVLEVBQUUsRUFBbUI7UUFDL0IsV0FBVyxFQUFFLElBQUksSUFBSSxFQUFFLENBQUMsT0FBTyxFQUFFO1FBQ2pDLE9BQU8sRUFBRSxJQUFJLENBQUMsT0FBTyxDQUFDLElBQUksSUFBSSxFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsT0FBTyxFQUFFO1FBQzlDLFdBQVcsRUFBRSxJQUFJLHlCQUFXLEVBQUU7UUFDOUIsa0JBQWtCLEVBQUUsRUFBOEI7UUFDbEQsY0FBYyxFQUFFLEVBQW1CO1FBRW5DLGtCQUFrQixFQUFFLENBQUM7UUFFckIsT0FBTyxFQUFFLEVBQUU7UUFDWCxZQUFZLEVBQUUsRUFBRTtRQUNoQixRQUFRLEVBQUUsRUFBRTtRQUNaLFNBQVMsRUFBRSxFQUFFO0tBQ2Q7SUFDRCxNQUFNLEVBQU4sVUFBTyxNQUFXO1FBQWxCLGlCQVNDO1FBUkMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxNQUFNLENBQUMsQ0FBQztRQUVwQixtQkFBbUIsQ0FBQyxNQUFNLENBQUMsVUFBQyxNQUFNO1lBQzFCLEtBQUssQ0FBQyxPQUFPLENBQUM7Z0JBQ2xCLE1BQU0sRUFBRSxNQUFNO2dCQUNkLFVBQVUsRUFBRSxNQUFNLENBQUMsR0FBRyxDQUFDLFVBQUEsQ0FBQyxJQUFJLE9BQUEsQ0FBQyxDQUFDLFNBQVMsRUFB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QUFHLENBQUMsSUFBSSxDQUFDLG1CQUFtQixFQUFFLEVBQUM7WUFDN0IsT0FBTztTQUNSO1FBRUQsY0FBYyxDQUFDLGNBQWMsQ0FBQyxVQUFBLE1BQU07WUFDbEMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxNQUFNLENBQUMsQ0FBQztZQUNwQixJQUFHLE1BQU0sQ0FBQyxNQUFNLElBQUksR0FBRyxFQUFDO2dCQUN0QixFQUFFLENBQUMsU0FBUyxDQUFDO29CQUNYLEtBQUssRUFBRSxNQUFNO29CQUNiLElBQUksRUFBRSxTQUFTO29CQUNmLFFBQVEsRUFBRSxJQUFJO29CQUNkLFFBQVEsRUFBRSxVQUFBLEdBQUc7d0JBQ1gsRUFBRSxDQUFDLFFBQVEsQ0FBQzs0QkFDVixHQUFHLEVBQUUsb0JBQW9CO3lCQUMxQixDQUFDLENBQUM7b0JBQ0wsQ0FBQztpQkFDRixDQUFDLENBQUM7YUFDSjtpQkFBTTtnQkFDTCxFQUFFLENBQUMsU0FBUyxDQUFDO29CQUNYLEtBQUssRUFBRSw4QkFBUSxNQUFNLENBQUMsUUFBVTtvQkFDaEMsSUFBSSxFQUFFLE1BQU07b0JBQ1osUUFBUSxFQUFFLElBQUk7aUJBQ2YsQ0FBQyxDQUFDO2FBQ0o7UUFDSCxDQUFDLEVBQUUsSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUUsTUFBTSxFQUFFLEVBQUUsQ0FBQyxDQUFDO0lBQ3hDLENBQUM7Q0FDRixDQUFDLENBQUEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBSZXNlcnZhdGlvblNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9SZXNlcnZhdGlvblNlcnZpY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvblBsYWNlU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL1Jlc2VydmF0aW9uUGxhY2VTZXJ2aWNlJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb24gfSBmcm9tICcuLi8uLi9tb2RlbHMvUmVzZXJ2YXRpb24nO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvblBsYWNlIH0gZnJvbSAnLi4vLi4vbW9kZWxzL1Jlc2VydmF0aW9uUGxhY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvblBlcmlvZCB9IGZyb20gJy4uLy4uL21vZGVscy9SZXNlcnZhdGlvblBlcmlvZCc7XHJcblxyXG5pbXBvcnQgKiBhcyB1dGlsIGZyb20gJy4uLy4uL3V0aWxzL3V0aWwnO1xyXG5cclxuY29uc3QgcmVzZXJ2YXRpb25TdmMgPSBuZXcgUmVzZXJ2YXRpb25TZXJ2aWNlKCk7XHJcbmNvbnN0IHJlc2VydmF0aW9uUGxhY2VTdmMgPSBuZXcgUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UoKTtcclxuXHJcblBhZ2Uoe1xyXG4gIGRhdGE6IHtcclxuICAgIHN0ZXBJbmRleDogMCxcclxuICAgIHBsYWNlczogW10gYXMgQXJyYXk8UmVzZXJ2YXRpb25QbGFjZT4sXHJcbiAgICBwbGFjZU5hbWVzOiBbXSBhcyBBcnJheTxzdHJpbmc+LFxyXG4gICAgY3VycmVudERhdGU6IG5ldyBEYXRlKCkuZ2V0VGltZSgpLFxyXG4gICAgbWF4RGF0ZTogdXRpbC5hZGREYXlzKG5ldyBEYXRlKCksIDcpLmdldFRpbWUoKSxcclxuICAgIHJlc2VydmF0aW9uOiBuZXcgUmVzZXJ2YXRpb24oKSxcclxuICAgIHJlc2VydmF0aW9uUGVyaW9kczogW10gYXMgQXJyYXk8UmVzZXJ2YXRpb25QZXJpb2Q+LFxyXG4gICAgY2hlY2tlZFBlcmlvZHM6IFtdIGFzIEFycmF5PHN0cmluZz4sXHJcbiAgICBcclxuICAgIHNlbGVjdGVkUGxhY2VJbmRleDogMCxcclxuXHJcbiAgICB1bml0RXJyOiBcIlwiLFxyXG4gICAgYWNDb250ZW50RXJyOiBcIlwiLFxyXG4gICAgcE5hbWVFcnI6IFwiXCIsXHJcbiAgICBwUGhvbmVFcnI6IFwiXCJcclxuICB9LFxyXG4gIG9uTG9hZChwYXJhbXM6IGFueSkge1xyXG4gICAgY29uc29sZS5sb2cocGFyYW1zKTtcclxuXHJcbiAgICByZXNlcnZhdGlvblBsYWNlU3ZjLkdldEFsbCgocmVzdWx0KSA9PiB7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBsYWNlczogcmVzdWx0LFxyXG4gICAgICAgIHBsYWNlTmFtZXM6IHJlc3VsdC5tYXAocCA9PiBwLlBsYWNlTmFtZSlcclxuICAgICAgfSk7XHJcbiAgICB9KVxyXG4gIH0sXHJcbiAgb25TdGVwQ2hhbmdlKCk6IGJvb2xlYW4ge1xyXG4gICAgY29uc29sZS5sb2coYHN0ZXBJbmRleDoke3RoaXMuZGF0YS5zdGVwSW5kZXh9LHJlc2VydmF0aW9uSW5mbzogJHtKU09OLnN0cmluZ2lmeSh0aGlzLmRhdGEucmVzZXJ2YXRpb24pfWApO1xyXG4gICAgc3dpdGNoICh0aGlzLmRhdGEuc3RlcEluZGV4KSB7XHJcbiAgICAgIGNhc2UgMDpcclxuICAgICAgICBicmVhaztcclxuICAgICAgY2FzZSAxOlxyXG4gICAgICAgIGlmICghdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGxhY2VJZCkge1xyXG4gICAgICAgICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGxhY2VJZCA9IHRoaXMuZGF0YS5wbGFjZXNbMF0uUGxhY2VJZDtcclxuICAgICAgICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBsYWNlTmFtZSA9IHRoaXMuZGF0YS5wbGFjZXNbMF0uUGxhY2VOYW1lO1xyXG4gICAgICAgIH1cclxuICAgICAgICBicmVhaztcclxuICAgICAgY2FzZSAyOlxyXG4gICAgICAgIGlmICghdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yRGF0ZSkge1xyXG4gICAgICAgICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yRGF0ZSA9IHV0aWwuZm9ybWF0RGF0ZShuZXcgRGF0ZSgpKTtcclxuICAgICAgICB9XHJcbiAgICAgICAgLy9cclxuICAgICAgICByZXNlcnZhdGlvblBsYWNlU3ZjLmdldEF2YWlsYWJsZVBlcmlvZHMocmVzdWx0ID0+IHtcclxuICAgICAgICAgIGNvbnNvbGUubG9nKHJlc3VsdCk7XHJcbiAgICAgICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICAgICAgcmVzZXJ2YXRpb25QZXJpb2RzOiByZXN1bHQsXHJcbiAgICAgICAgICAgIGNoZWNrZWRQZXJpb2RzOiBbXVxyXG4gICAgICAgICAgfSk7XHJcbiAgICAgICAgfSwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGxhY2VJZCwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yRGF0ZSk7XHJcblxyXG4gICAgICAgIGJyZWFrO1xyXG5cclxuICAgICAgY2FzZSAzOlxyXG4gICAgICAgIGlmKHRoaXMuZGF0YS5jaGVja2VkUGVyaW9kcy5sZW5ndGggPT0gMCl7XHJcbiAgICAgICAgICB3eC5zaG93VG9hc3Qoe1xyXG4gICAgICAgICAgICB0aXRsZTogXCLor7fpgInmi6nopoHpooTnuqbnmoTml7bpl7TmrrXmiJbov5Tlm57kuIrkuIDmraXpgInmi6nlhbbku5bpooTnuqbml6XmnJ9cIixcclxuICAgICAgICAgICAgZHVyYXRpb246IDIwMDAsXHJcbiAgICAgICAgICAgIGljb246IFwibm9uZVwiXHJcbiAgICAgICAgICB9KTtcclxuICAgICAgICAgIHRoaXMuZGF0YS5zdGVwSW5kZXgtLTtcclxuICAgICAgICAgIHJldHVybiBmYWxzZTtcclxuICAgICAgICB9XHJcbiAgICAgICAgY29uc29sZS5sb2codGhpcy5kYXRhLnJlc2VydmF0aW9uUGVyaW9kcyk7XHJcbiAgICAgICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uRm9yVGltZUlkcyA9IHRoaXMuZGF0YS5yZXNlcnZhdGlvblBlcmlvZHMuZmlsdGVyKF89Pl8uQ2hlY2tlZCkubWFwKHg9PnguUGVyaW9kSW5kZXgpLmpvaW4oXCIsXCIpO1xyXG4gICAgICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkZvclRpbWUgPSB0aGlzLmRhdGEucmVzZXJ2YXRpb25QZXJpb2RzLmZpbHRlcihfPT5fLkNoZWNrZWQpLm1hcCh4PT4geC5QZXJpb2RUaXRsZSkuam9pbihcIixcIik7XHJcbiAgICBcclxuICAgICAgICBicmVhaztcclxuXHJcbiAgICAgIGNhc2UgNDpcclxuICAgICAgICBpZih0aGlzLnZhbGlkYXRlSW5wdXRQYXJhbXMoKSl7XHJcbiAgICAgICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICAgICAgcmVzZXJ2YXRpb246IHRoaXMuZGF0YS5yZXNlcnZhdGlvblxyXG4gICAgICAgICAgfSk7XHJcbiAgICAgICAgfWVsc2V7XHJcbiAgICAgICAgICB0aGlzLmRhdGEuc3RlcEluZGV4LS07XHJcbiAgICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIFxyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiB0cnVlO1xyXG4gIH0sXHJcblxyXG4gIHByZXZTdGVwKGV2ZW50OiBhbnkpIHtcclxuICAgIHRoaXMuZGF0YS5zdGVwSW5kZXgtLTtcclxuICAgIHRoaXMub25TdGVwQ2hhbmdlKCk7XHJcbiAgICAvL1xyXG4gICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgIHN0ZXBJbmRleDogdGhpcy5kYXRhLnN0ZXBJbmRleFxyXG4gICAgfSk7XHJcbiAgfSxcclxuICBuZXh0U3RlcChldmVudDogYW55KSB7XHJcbiAgICB0aGlzLmRhdGEuc3RlcEluZGV4Kys7XHJcbiAgICB0aGlzLm9uU3RlcENoYW5nZSgpO1xyXG4gICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgIHN0ZXBJbmRleDogdGhpcy5kYXRhLnN0ZXBJbmRleFxyXG4gICAgfSk7XHJcbiAgfSxcclxuXHJcbiAgb25QbGFjZUNoYW5nZShldmVudDogYW55KSB7XHJcbiAgICBjb25zdCB7IHBpY2tlciwgdmFsdWUsIGluZGV4IH0gPSBldmVudC5kZXRhaWw7XHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgc2VsZWN0ZWRQbGFjZUluZGV4OiBpbmRleFxyXG4gICAgfSk7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QbGFjZUlkID0gdGhpcy5kYXRhLnBsYWNlc1tpbmRleF0uUGxhY2VJZDtcclxuICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBsYWNlTmFtZSA9IHRoaXMuZGF0YS5wbGFjZXNbaW5kZXhdLlBsYWNlTmFtZTtcclxuICB9LFxyXG5cclxuICBvbkRhdGVJbnB1dChldmVudDogYW55KSB7XHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgY3VycmVudERhdGU6IGV2ZW50LmRldGFpbFxyXG4gICAgfSk7XHJcbiAgICBsZXQgZGF0ZVN0ciA9IHV0aWwuZm9ybWF0RGF0ZShuZXcgRGF0ZShldmVudC5kZXRhaWwpKTtcclxuICAgIGNvbnNvbGUubG9nKGBkYXRlOiAke2RhdGVTdHJ9YCk7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25Gb3JEYXRlID0gZGF0ZVN0cjtcclxuXHJcbiAgfSxcclxuXHJcbiAgb25QZXJpb2RzQ2hhbmdlKGV2ZW50OiBhbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKGV2ZW50KTtcclxuXHJcbiAgICBsZXQgaWR4cyA9IG5ldyBBcnJheTxudW1iZXI+KCk7XHJcbiAgICBmb3IgKGxldCBuYW1lIG9mIChldmVudC5kZXRhaWwgYXMgQXJyYXk8c3RyaW5nPikpIHtcclxuICAgICAgbGV0IGlkeCA9IE51bWJlci5wYXJzZUludChuYW1lLnN1YnN0cig3KSk7ICAgICAgXHJcbiAgICAgIGlkeHMucHVzaChpZHgpO1xyXG4gICAgfVxyXG4gICAgZm9yKGxldCBwIG9mIHRoaXMuZGF0YS5yZXNlcnZhdGlvblBlcmlvZHMpe1xyXG4gICAgICBpZihpZHhzLmluZGV4T2YocC5QZXJpb2RJbmRleCkgPiAtMSl7XHJcbiAgICAgICAgcC5DaGVja2VkID0gdHJ1ZTtcclxuICAgICAgfVxyXG4gICAgfVxyXG5cclxuICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICBjaGVja2VkUGVyaW9kczogZXZlbnQuZGV0YWlsXHJcbiAgICB9KTtcclxuICB9LFxyXG5cclxuICBvblVuaXRDaGFuZ2UoZXZlbnQ6IGFueSkge1xyXG4gICAgY29uc29sZS5sb2coZXZlbnQpO1xyXG4gICAgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uVW5pdCA9IGV2ZW50LmRldGFpbDtcclxuICB9LFxyXG4gIG9uQWN0aXZpdHlDb250ZW50Q2hhbmdlKGV2ZW50OiBhbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKGV2ZW50KTtcclxuICAgIHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkFjdGl2aXR5Q29udGVudCA9IGV2ZW50LmRldGFpbDtcclxuICB9LFxyXG4gIG9uUGVyc29uTmFtZUNoYW5nZShldmVudDogYW55KSB7XHJcbiAgICBjb25zb2xlLmxvZyhldmVudCk7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QZXJzb25OYW1lID0gZXZlbnQuZGV0YWlsO1xyXG4gIH0sXHJcbiAgb25QZXJzb25QaG9uZUNoYW5nZShldmVudDogYW55KSB7XHJcbiAgICBjb25zb2xlLmxvZyhldmVudCk7XHJcbiAgICB0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QZXJzb25QaG9uZSA9IGV2ZW50LmRldGFpbDtcclxuICB9LFxyXG5cclxuXHJcbiAgdmFsaWRhdGVJbnB1dFBhcmFtcygpOiBib29sZWFue1xyXG4gICAgaWYoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblVuaXQpe1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICB1bml0RXJyOiBcIumihOe6puWNleS9jeS4jeiDveS4uuepulwiXHJcbiAgICAgIH0pO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25Vbml0Lmxlbmd0aCA8IDIgfHwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uVW5pdC5sZW5ndGggPiAxNil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHVuaXRFcnI6IFwi6aKE57qm5Y2V5L2N6ZW/5bqm6ZyA6KaB5ZyoIDIg5LiOIDE2IOS5i+mXtFwiXHJcbiAgICAgIH0pO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLmRhdGEudW5pdEVycil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHVuaXRFcnI6IFwiXCJcclxuICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvbkFjdGl2aXR5Q29udGVudCl7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIGFjQ29udGVudEVycjogXCLmtLvliqjlhoXlrrnkuI3og73kuLrnqbpcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uQWN0aXZpdHlDb250ZW50Lmxlbmd0aCA8IDIgfHwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uQWN0aXZpdHlDb250ZW50Lmxlbmd0aCA+IDE2KXtcclxuICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgYWNDb250ZW50RXJyOiBcIua0u+WKqOWGheWuuemVv+W6pumcgOimgeWcqCAyIOS4jiAxNiDkuYvpl7RcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLmFjQ29udGVudEVycil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIGFjQ29udGVudEVycjogXCJcIlxyXG4gICAgICB9KTtcclxuICAgIH1cclxuXHJcbiAgICBpZighdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uTmFtZSl7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBOYW1lRXJyOiBcIumihOe6puS6uuWQjeensOS4jeiDveS4uuepulwiXHJcbiAgICAgIH0pO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcbiAgICBpZih0aGlzLmRhdGEucmVzZXJ2YXRpb24uUmVzZXJ2YXRpb25QZXJzb25OYW1lLmxlbmd0aCA8IDIgfHwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLlJlc2VydmF0aW9uUGVyc29uTmFtZS5sZW5ndGggPiAxNil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBOYW1lRXJyOiBcIumihOe6puS6uuWQjeensOmVv+W6pumcgOimgeWcqCAyIOS4jiAxNiDkuYvpl7RcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnBOYW1lRXJyKXtcclxuICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgcE5hbWVFcnI6IFwiXCJcclxuICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgaWYoIXRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBlcnNvblBob25lKXtcclxuICAgICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgICAgcFBob25lRXJyOiBcIumihOe6puS6uuaJi+acuuWPt+S4jeiDveS4uuepulwiXHJcbiAgICAgIH0pO1xyXG4gICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICB9XHJcbiAgICBpZighL14xWzMtOV1cXGR7OX0kLy50ZXN0KHRoaXMuZGF0YS5yZXNlcnZhdGlvbi5SZXNlcnZhdGlvblBlcnNvblBob25lKSl7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBQaG9uZUVycjogXCLpooTnuqbkurrmiYvmnLrlj7fkuI3lkIjms5VcIlxyXG4gICAgICB9KTtcclxuICAgICAgcmV0dXJuIGZhbHNlO1xyXG4gICAgfVxyXG4gICAgaWYodGhpcy5kYXRhLnBQaG9uZUVycil7XHJcbiAgICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICAgIHBQaG9uZUVycjogXCJcIlxyXG4gICAgICB9KTtcclxuICAgIH1cclxuICAgIHJldHVybiB0cnVlO1xyXG4gIH0sXHJcblxyXG4gIHN1Ym1pdChldmVudDogYW55KSB7XHJcbiAgICAvLyB2YWxpZGF0ZSBwYXJhbSBuYW1lXHJcbiAgICBpZighdGhpcy52YWxpZGF0ZUlucHV0UGFyYW1zKCkpe1xyXG4gICAgICByZXR1cm47XHJcbiAgICB9XHJcbiAgICAvL1xyXG4gICAgcmVzZXJ2YXRpb25TdmMuTmV3UmVzZXJ2YXRpb24ocmVzdWx0ID0+IHtcclxuICAgICAgY29uc29sZS5sb2cocmVzdWx0KTtcclxuICAgICAgaWYocmVzdWx0LlN0YXR1cyA9PSAyMDApe1xyXG4gICAgICAgIHd4LnNob3dUb2FzdCh7XHJcbiAgICAgICAgICB0aXRsZTogXCLpooTnuqbmiJDlip9cIixcclxuICAgICAgICAgIGljb246IFwic3VjY2Vzc1wiLFxyXG4gICAgICAgICAgZHVyYXRpb246IDIwMDAsXHJcbiAgICAgICAgICBjb21wbGV0ZTogcmVzID0+IHtcclxuICAgICAgICAgICAgd3gucmVMYXVuY2goe1xyXG4gICAgICAgICAgICAgIHVybDogJy9wYWdlcy9pbmRleC9pbmRleCdcclxuICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgICB9XHJcbiAgICAgICAgfSk7ICAgICAgICBcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICB3eC5zaG93VG9hc3Qoe1xyXG4gICAgICAgICAgdGl0bGU6IGDpooTnuqblpLHotKUsJHtyZXN1bHQuRXJyb3JNc2d9YCxcclxuICAgICAgICAgIGljb246IFwibm9uZVwiLFxyXG4gICAgICAgICAgZHVyYXRpb246IDIwMDBcclxuICAgICAgICB9KTtcclxuICAgICAgfVxyXG4gICAgfSwgdGhpcy5kYXRhLnJlc2VydmF0aW9uLCAnTm9uZScsICcnKTtcclxuICB9XHJcbn0pIl19