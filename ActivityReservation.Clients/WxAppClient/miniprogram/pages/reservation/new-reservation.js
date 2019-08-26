"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ReservationService_1 = require("../../services/ReservationService");
var ReservationPlaceService_1 = require("../../services/ReservationPlaceService");
var reservationSvc = new ReservationService_1.ReservationService();
var reservationPlaceSvc = new ReservationPlaceService_1.ReservationPlaceService();
Page({
    data: {
        steps: [
            {
                text: '选择活动室',
                desc: '选择要预约的活动室'
            },
            {
                text: '选择日期',
                desc: '选择要预约的日期'
            },
            {
                text: '选择时间段',
                desc: '选择要预约的时间段'
            },
            {
                text: '预约信息',
                desc: '填写预约信息'
            },
            {
                text: '确认预约',
                desc: '确认要预约的信息'
            }
        ],
        stepIndex: 0,
        places: [],
        placeNames: [],
        currentDate: new Date().getTime(),
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
    prevStep: function (event) {
        this.setData({
            stepIndex: --this.data.stepIndex
        });
    },
    nextStep: function (event) {
        this.setData({
            stepIndex: ++this.data.stepIndex
        });
    },
    onPlaceChange: function (event) {
        var _a = event.detail, picker = _a.picker, value = _a.value, index = _a.index;
        wx.showToast({
            title: "" + value,
            duration: 2000,
        });
    },
    submit: function (event) {
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmV3LXJlc2VydmF0aW9uLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibmV3LXJlc2VydmF0aW9uLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O0FBQUEsd0VBQXVFO0FBQ3ZFLGtGQUFpRjtBQUlqRixJQUFNLGNBQWMsR0FBRyxJQUFJLHVDQUFrQixFQUFFLENBQUM7QUFDaEQsSUFBTSxtQkFBbUIsR0FBRyxJQUFJLGlEQUF1QixFQUFFLENBQUM7QUFFMUQsSUFBSSxDQUFDO0lBQ0gsSUFBSSxFQUFFO1FBQ0osS0FBSyxFQUFFO1lBQ0w7Z0JBQ0UsSUFBSSxFQUFFLE9BQU87Z0JBQ2IsSUFBSSxFQUFFLFdBQVc7YUFDbEI7WUFDRDtnQkFDRSxJQUFJLEVBQUUsTUFBTTtnQkFDWixJQUFJLEVBQUUsVUFBVTthQUNqQjtZQUNEO2dCQUNFLElBQUksRUFBRSxPQUFPO2dCQUNiLElBQUksRUFBRSxXQUFXO2FBQ2xCO1lBQ0Q7Z0JBQ0UsSUFBSSxFQUFFLE1BQU07Z0JBQ1osSUFBSSxFQUFFLFFBQVE7YUFDZjtZQUNEO2dCQUNFLElBQUksRUFBRSxNQUFNO2dCQUNaLElBQUksRUFBRSxVQUFVO2FBQ2pCO1NBQ0Y7UUFDRCxTQUFTLEVBQUUsQ0FBQztRQUNaLE1BQU0sRUFBRSxFQUE2QjtRQUNyQyxVQUFVLEVBQUUsRUFBbUI7UUFDL0IsV0FBVyxFQUFFLElBQUksSUFBSSxFQUFFLENBQUMsT0FBTyxFQUFFO0tBQ2xDO0lBQ0QsTUFBTSxFQUFOLFVBQU8sTUFBVztRQUFsQixpQkFRQztRQVBDLE9BQU8sQ0FBQyxHQUFHLENBQUMsTUFBTSxDQUFDLENBQUM7UUFDcEIsbUJBQW1CLENBQUMsTUFBTSxDQUFDLFVBQUMsTUFBTTtZQUMxQixLQUFLLENBQUMsT0FBTyxDQUFDO2dCQUNsQixNQUFNLEVBQUUsTUFBTTtnQkFDZCxVQUFVLEVBQUUsTUFBTSxDQUFDLEdBQUcsQ0FBQyxVQUFBLENBQUMsSUFBRSxPQUFBLENBQUMsQ0FBQyxTQUFTLEVBQVgsQ0FBVyxDQUFDO2FBQ3ZDLENBQUMsQ0FBQztRQUNMLENBQUMsQ0FBQyxDQUFBO0lBQ0osQ0FBQztJQUNELFFBQVEsRUFBUixVQUFTLEtBQVU7UUFDWCxJQUFLLENBQUMsT0FBTyxDQUFDO1lBQ2xCLFNBQVMsRUFBRSxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsU0FBUztTQUNqQyxDQUFDLENBQUM7SUFDTCxDQUFDO0lBQ0QsUUFBUSxFQUFSLFVBQVMsS0FBUztRQUNWLElBQUssQ0FBQyxPQUFPLENBQUM7WUFDbEIsU0FBUyxFQUFFLEVBQUUsSUFBSSxDQUFDLElBQUksQ0FBQyxTQUFTO1NBQ2pDLENBQUMsQ0FBQztJQUNMLENBQUM7SUFFRCxhQUFhLEVBQWIsVUFBYyxLQUFTO1FBQ2YsSUFBQSxpQkFBdUMsRUFBckMsa0JBQU0sRUFBRSxnQkFBSyxFQUFFLGdCQUFzQixDQUFDO1FBRTlDLEVBQUUsQ0FBQyxTQUFTLENBQUM7WUFDWCxLQUFLLEVBQUUsS0FBRyxLQUFPO1lBQ2pCLFFBQVEsRUFBRSxJQUFJO1NBQ2YsQ0FBQyxDQUFDO0lBRUwsQ0FBQztJQUVELE1BQU0sRUFBTixVQUFPLEtBQVU7SUFFakIsQ0FBQztDQUNGLENBQUMsQ0FBQSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFJlc2VydmF0aW9uU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL1Jlc2VydmF0aW9uU2VydmljZSc7XHJcbmltcG9ydCB7IFJlc2VydmF0aW9uUGxhY2VTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UnO1xyXG5pbXBvcnQgeyBSZXNlcnZhdGlvbiB9IGZyb20gJy4uLy4uL21vZGVscy9SZXNlcnZhdGlvbic7XHJcbmltcG9ydCB7IFJlc2VydmF0aW9uUGxhY2UgfSBmcm9tICcuLi8uLi9tb2RlbHMvUmVzZXJ2YXRpb25QbGFjZSc7XHJcblxyXG5jb25zdCByZXNlcnZhdGlvblN2YyA9IG5ldyBSZXNlcnZhdGlvblNlcnZpY2UoKTtcclxuY29uc3QgcmVzZXJ2YXRpb25QbGFjZVN2YyA9IG5ldyBSZXNlcnZhdGlvblBsYWNlU2VydmljZSgpO1xyXG5cclxuUGFnZSh7XHJcbiAgZGF0YToge1xyXG4gICAgc3RlcHM6IFtcclxuICAgICAge1xyXG4gICAgICAgIHRleHQ6ICfpgInmi6nmtLvliqjlrqQnLFxyXG4gICAgICAgIGRlc2M6ICfpgInmi6nopoHpooTnuqbnmoTmtLvliqjlrqQnXHJcbiAgICAgIH0sXHJcbiAgICAgIHtcclxuICAgICAgICB0ZXh0OiAn6YCJ5oup5pel5pyfJyxcclxuICAgICAgICBkZXNjOiAn6YCJ5oup6KaB6aKE57qm55qE5pel5pyfJ1xyXG4gICAgICB9LFxyXG4gICAgICB7XHJcbiAgICAgICAgdGV4dDogJ+mAieaLqeaXtumXtOautScsXHJcbiAgICAgICAgZGVzYzogJ+mAieaLqeimgemihOe6pueahOaXtumXtOautSdcclxuICAgICAgfSxcclxuICAgICAge1xyXG4gICAgICAgIHRleHQ6ICfpooTnuqbkv6Hmga8nLFxyXG4gICAgICAgIGRlc2M6ICfloavlhpnpooTnuqbkv6Hmga8nXHJcbiAgICAgIH0sXHJcbiAgICAgIHtcclxuICAgICAgICB0ZXh0OiAn56Gu6K6k6aKE57qmJyxcclxuICAgICAgICBkZXNjOiAn56Gu6K6k6KaB6aKE57qm55qE5L+h5oGvJ1xyXG4gICAgICB9XHJcbiAgICBdLFxyXG4gICAgc3RlcEluZGV4OiAwLFxyXG4gICAgcGxhY2VzOiBbXSBhcyBBcnJheTxSZXNlcnZhdGlvblBsYWNlPixcclxuICAgIHBsYWNlTmFtZXM6IFtdIGFzIEFycmF5PHN0cmluZz4sXHJcbiAgICBjdXJyZW50RGF0ZTogbmV3IERhdGUoKS5nZXRUaW1lKCksXHJcbiAgfSxcclxuICBvbkxvYWQocGFyYW1zOiBhbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKHBhcmFtcyk7XHJcbiAgICByZXNlcnZhdGlvblBsYWNlU3ZjLkdldEFsbCgocmVzdWx0KT0+e1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBwbGFjZXM6IHJlc3VsdCxcclxuICAgICAgICBwbGFjZU5hbWVzOiByZXN1bHQubWFwKHA9PnAuUGxhY2VOYW1lKVxyXG4gICAgICB9KTtcclxuICAgIH0pXHJcbiAgfSxcclxuICBwcmV2U3RlcChldmVudDogYW55KSB7XHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgc3RlcEluZGV4OiAtLXRoaXMuZGF0YS5zdGVwSW5kZXhcclxuICAgIH0pO1xyXG4gIH0sXHJcbiAgbmV4dFN0ZXAoZXZlbnQ6YW55KXtcclxuICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICBzdGVwSW5kZXg6ICsrdGhpcy5kYXRhLnN0ZXBJbmRleFxyXG4gICAgfSk7XHJcbiAgfSxcclxuXHJcbiAgb25QbGFjZUNoYW5nZShldmVudDphbnkpIHtcclxuICAgIGNvbnN0IHsgcGlja2VyLCB2YWx1ZSwgaW5kZXggfSA9IGV2ZW50LmRldGFpbDtcclxuICAgIC8vIGDlvZPliY3lgLzvvJoke3ZhbHVlfSwg5b2T5YmN57Si5byV77yaJHtpbmRleH1gXHJcbiAgICB3eC5zaG93VG9hc3Qoe1xyXG4gICAgICB0aXRsZTogYCR7dmFsdWV9YCxcclxuICAgICAgZHVyYXRpb246IDIwMDAsXHJcbiAgICB9KTtcclxuXHJcbiAgfSxcclxuXHJcbiAgc3VibWl0KGV2ZW50OiBhbnkpe1xyXG5cclxuICB9XHJcbn0pIl19