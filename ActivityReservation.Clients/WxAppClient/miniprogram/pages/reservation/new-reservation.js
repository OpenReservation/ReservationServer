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
        stepIndex: 0
    },
    onLoad: function (params) {
        console.log(params);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmV3LXJlc2VydmF0aW9uLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibmV3LXJlc2VydmF0aW9uLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLElBQUksQ0FBQztJQUNILElBQUksRUFBRTtRQUNKLEtBQUssRUFBRTtZQUNMO2dCQUNFLElBQUksRUFBRSxPQUFPO2dCQUNiLElBQUksRUFBRSxXQUFXO2FBQ2xCO1lBQ0Q7Z0JBQ0UsSUFBSSxFQUFFLE1BQU07Z0JBQ1osSUFBSSxFQUFFLFVBQVU7YUFDakI7WUFDRDtnQkFDRSxJQUFJLEVBQUUsT0FBTztnQkFDYixJQUFJLEVBQUUsV0FBVzthQUNsQjtZQUNEO2dCQUNFLElBQUksRUFBRSxNQUFNO2dCQUNaLElBQUksRUFBRSxRQUFRO2FBQ2Y7WUFDRDtnQkFDRSxJQUFJLEVBQUUsTUFBTTtnQkFDWixJQUFJLEVBQUUsVUFBVTthQUNqQjtTQUNGO1FBQ0QsU0FBUyxFQUFFLENBQUM7S0FDYjtJQUNELE1BQU0sRUFBTixVQUFPLE1BQVc7UUFDaEIsT0FBTyxDQUFDLEdBQUcsQ0FBQyxNQUFNLENBQUMsQ0FBQztJQUN0QixDQUFDO0lBQ0QsUUFBUSxFQUFSLFVBQVMsS0FBVTtRQUNYLElBQUssQ0FBQyxPQUFPLENBQUM7WUFDbEIsU0FBUyxFQUFFLEVBQUUsSUFBSSxDQUFDLElBQUksQ0FBQyxTQUFTO1NBQ2pDLENBQUMsQ0FBQztJQUNMLENBQUM7SUFDRCxRQUFRLEVBQVIsVUFBUyxLQUFTO1FBQ1YsSUFBSyxDQUFDLE9BQU8sQ0FBQztZQUNsQixTQUFTLEVBQUUsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLFNBQVM7U0FDakMsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUNELE1BQU0sRUFBTixVQUFPLEtBQVU7SUFFakIsQ0FBQztDQUNGLENBQUMsQ0FBQSIsInNvdXJjZXNDb250ZW50IjpbIlBhZ2Uoe1xyXG4gIGRhdGE6IHtcclxuICAgIHN0ZXBzOiBbXHJcbiAgICAgIHtcclxuICAgICAgICB0ZXh0OiAn6YCJ5oup5rS75Yqo5a6kJyxcclxuICAgICAgICBkZXNjOiAn6YCJ5oup6KaB6aKE57qm55qE5rS75Yqo5a6kJ1xyXG4gICAgICB9LFxyXG4gICAgICB7XHJcbiAgICAgICAgdGV4dDogJ+mAieaLqeaXpeacnycsXHJcbiAgICAgICAgZGVzYzogJ+mAieaLqeimgemihOe6pueahOaXpeacnydcclxuICAgICAgfSxcclxuICAgICAge1xyXG4gICAgICAgIHRleHQ6ICfpgInmi6nml7bpl7TmrrUnLFxyXG4gICAgICAgIGRlc2M6ICfpgInmi6nopoHpooTnuqbnmoTml7bpl7TmrrUnXHJcbiAgICAgIH0sXHJcbiAgICAgIHtcclxuICAgICAgICB0ZXh0OiAn6aKE57qm5L+h5oGvJyxcclxuICAgICAgICBkZXNjOiAn5aGr5YaZ6aKE57qm5L+h5oGvJ1xyXG4gICAgICB9LFxyXG4gICAgICB7XHJcbiAgICAgICAgdGV4dDogJ+ehruiupOmihOe6picsXHJcbiAgICAgICAgZGVzYzogJ+ehruiupOimgemihOe6pueahOS/oeaBrydcclxuICAgICAgfVxyXG4gICAgXSxcclxuICAgIHN0ZXBJbmRleDogMFxyXG4gIH0sXHJcbiAgb25Mb2FkKHBhcmFtczogYW55KSB7XHJcbiAgICBjb25zb2xlLmxvZyhwYXJhbXMpO1xyXG4gIH0sXHJcbiAgcHJldlN0ZXAoZXZlbnQ6IGFueSkge1xyXG4gICAgKDxhbnk+dGhpcykuc2V0RGF0YSh7XHJcbiAgICAgIHN0ZXBJbmRleDogLS10aGlzLmRhdGEuc3RlcEluZGV4XHJcbiAgICB9KTtcclxuICB9LFxyXG4gIG5leHRTdGVwKGV2ZW50OmFueSl7XHJcbiAgICAoPGFueT50aGlzKS5zZXREYXRhKHtcclxuICAgICAgc3RlcEluZGV4OiArK3RoaXMuZGF0YS5zdGVwSW5kZXhcclxuICAgIH0pO1xyXG4gIH0sXHJcbiAgc3VibWl0KGV2ZW50OiBhbnkpe1xyXG5cclxuICB9XHJcbn0pIl19