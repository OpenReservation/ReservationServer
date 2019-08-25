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
        places: []
    },
    onLoad: function (params) {
        var _this = this;
        console.log(params);
        reservationPlaceSvc.GetAll(function (result) {
            _this.setData({
                places: result
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
    submit: function (event) {
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmV3LXJlc2VydmF0aW9uLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibmV3LXJlc2VydmF0aW9uLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O0FBQUEsd0VBQXVFO0FBQ3ZFLGtGQUFpRjtBQUlqRixJQUFNLGNBQWMsR0FBRyxJQUFJLHVDQUFrQixFQUFFLENBQUM7QUFDaEQsSUFBTSxtQkFBbUIsR0FBRyxJQUFJLGlEQUF1QixFQUFFLENBQUM7QUFFMUQsSUFBSSxDQUFDO0lBQ0gsSUFBSSxFQUFFO1FBQ0osS0FBSyxFQUFFO1lBQ0w7Z0JBQ0UsSUFBSSxFQUFFLE9BQU87Z0JBQ2IsSUFBSSxFQUFFLFdBQVc7YUFDbEI7WUFDRDtnQkFDRSxJQUFJLEVBQUUsTUFBTTtnQkFDWixJQUFJLEVBQUUsVUFBVTthQUNqQjtZQUNEO2dCQUNFLElBQUksRUFBRSxPQUFPO2dCQUNiLElBQUksRUFBRSxXQUFXO2FBQ2xCO1lBQ0Q7Z0JBQ0UsSUFBSSxFQUFFLE1BQU07Z0JBQ1osSUFBSSxFQUFFLFFBQVE7YUFDZjtZQUNEO2dCQUNFLElBQUksRUFBRSxNQUFNO2dCQUNaLElBQUksRUFBRSxVQUFVO2FBQ2pCO1NBQ0Y7UUFDRCxTQUFTLEVBQUUsQ0FBQztRQUNaLE1BQU0sRUFBRSxFQUE2QjtLQUN0QztJQUNELE1BQU0sRUFBTixVQUFPLE1BQVc7UUFBbEIsaUJBT0M7UUFOQyxPQUFPLENBQUMsR0FBRyxDQUFDLE1BQU0sQ0FBQyxDQUFDO1FBQ3BCLG1CQUFtQixDQUFDLE1BQU0sQ0FBQyxVQUFDLE1BQU07WUFDMUIsS0FBSyxDQUFDLE9BQU8sQ0FBQztnQkFDbEIsTUFBTSxFQUFFLE1BQU07YUFDZixDQUFDLENBQUM7UUFDTCxDQUFDLENBQUMsQ0FBQTtJQUNKLENBQUM7SUFDRCxRQUFRLEVBQVIsVUFBUyxLQUFVO1FBQ1gsSUFBSyxDQUFDLE9BQU8sQ0FBQztZQUNsQixTQUFTLEVBQUUsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLFNBQVM7U0FDakMsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUNELFFBQVEsRUFBUixVQUFTLEtBQVM7UUFDVixJQUFLLENBQUMsT0FBTyxDQUFDO1lBQ2xCLFNBQVMsRUFBRSxFQUFFLElBQUksQ0FBQyxJQUFJLENBQUMsU0FBUztTQUNqQyxDQUFDLENBQUM7SUFDTCxDQUFDO0lBQ0QsTUFBTSxFQUFOLFVBQU8sS0FBVTtJQUVqQixDQUFDO0NBQ0YsQ0FBQyxDQUFBIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUmVzZXJ2YXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvUmVzZXJ2YXRpb25TZXJ2aWNlJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb25QbGFjZVNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9SZXNlcnZhdGlvblBsYWNlU2VydmljZSc7XHJcbmltcG9ydCB7IFJlc2VydmF0aW9uIH0gZnJvbSAnLi4vLi4vbW9kZWxzL1Jlc2VydmF0aW9uJztcclxuaW1wb3J0IHsgUmVzZXJ2YXRpb25QbGFjZSB9IGZyb20gJy4uLy4uL21vZGVscy9SZXNlcnZhdGlvblBsYWNlJztcclxuXHJcbmNvbnN0IHJlc2VydmF0aW9uU3ZjID0gbmV3IFJlc2VydmF0aW9uU2VydmljZSgpO1xyXG5jb25zdCByZXNlcnZhdGlvblBsYWNlU3ZjID0gbmV3IFJlc2VydmF0aW9uUGxhY2VTZXJ2aWNlKCk7XHJcblxyXG5QYWdlKHtcclxuICBkYXRhOiB7XHJcbiAgICBzdGVwczogW1xyXG4gICAgICB7XHJcbiAgICAgICAgdGV4dDogJ+mAieaLqea0u+WKqOWupCcsXHJcbiAgICAgICAgZGVzYzogJ+mAieaLqeimgemihOe6pueahOa0u+WKqOWupCdcclxuICAgICAgfSxcclxuICAgICAge1xyXG4gICAgICAgIHRleHQ6ICfpgInmi6nml6XmnJ8nLFxyXG4gICAgICAgIGRlc2M6ICfpgInmi6nopoHpooTnuqbnmoTml6XmnJ8nXHJcbiAgICAgIH0sXHJcbiAgICAgIHtcclxuICAgICAgICB0ZXh0OiAn6YCJ5oup5pe26Ze05q61JyxcclxuICAgICAgICBkZXNjOiAn6YCJ5oup6KaB6aKE57qm55qE5pe26Ze05q61J1xyXG4gICAgICB9LFxyXG4gICAgICB7XHJcbiAgICAgICAgdGV4dDogJ+mihOe6puS/oeaBrycsXHJcbiAgICAgICAgZGVzYzogJ+Whq+WGmemihOe6puS/oeaBrydcclxuICAgICAgfSxcclxuICAgICAge1xyXG4gICAgICAgIHRleHQ6ICfnoa7orqTpooTnuqYnLFxyXG4gICAgICAgIGRlc2M6ICfnoa7orqTopoHpooTnuqbnmoTkv6Hmga8nXHJcbiAgICAgIH1cclxuICAgIF0sXHJcbiAgICBzdGVwSW5kZXg6IDAsXHJcbiAgICBwbGFjZXM6IFtdIGFzIEFycmF5PFJlc2VydmF0aW9uUGxhY2U+XHJcbiAgfSxcclxuICBvbkxvYWQocGFyYW1zOiBhbnkpIHtcclxuICAgIGNvbnNvbGUubG9nKHBhcmFtcyk7XHJcbiAgICByZXNlcnZhdGlvblBsYWNlU3ZjLkdldEFsbCgocmVzdWx0KT0+e1xyXG4gICAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgICBwbGFjZXM6IHJlc3VsdFxyXG4gICAgICB9KTtcclxuICAgIH0pXHJcbiAgfSxcclxuICBwcmV2U3RlcChldmVudDogYW55KSB7XHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgc3RlcEluZGV4OiAtLXRoaXMuZGF0YS5zdGVwSW5kZXhcclxuICAgIH0pO1xyXG4gIH0sXHJcbiAgbmV4dFN0ZXAoZXZlbnQ6YW55KXtcclxuICAgICg8YW55PnRoaXMpLnNldERhdGEoe1xyXG4gICAgICBzdGVwSW5kZXg6ICsrdGhpcy5kYXRhLnN0ZXBJbmRleFxyXG4gICAgfSk7XHJcbiAgfSxcclxuICBzdWJtaXQoZXZlbnQ6IGFueSl7XHJcblxyXG4gIH1cclxufSkiXX0=