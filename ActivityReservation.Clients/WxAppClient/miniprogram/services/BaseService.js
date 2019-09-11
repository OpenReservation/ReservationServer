"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BaseService = (function () {
    function BaseService(apiPath) {
        this.apiPath = apiPath;
        this.apiBaseUrl = "https://service-balxf7hr-1251288923.ap-shanghai.apigateway.myqcloud.com/release/reservationWxAppGateway";
    }
    BaseService.prototype.Get = function (callback, params) {
        wx.showLoading({
            title: "loading..."
        });
        var url = this.apiBaseUrl + "/api/" + this.apiPath;
        if (params != undefined && Object.keys(params).length > 0) {
            url += "?";
            for (var _i = 0, _a = Object.keys(params); _i < _a.length; _i++) {
                var name_1 = _a[_i];
                url += name_1 + "=" + params[name_1] + "&";
            }
        }
        console.log("url: " + url);
        wx.request({
            url: url,
            success: function (response) {
                console.log(response);
                var result = response.data;
                callback(result);
                wx.hideLoading();
            },
            fail: function (err) {
            }
        });
    };
    BaseService.prototype.GetAll = function (callback) {
        wx.showLoading({
            title: "loading..."
        });
        wx.request({
            url: this.apiBaseUrl + "/api/" + this.apiPath,
            success: function (response) {
                console.log(response);
                wx.hideLoading();
                var result = response.data;
                callback(result);
            },
            fail: function (err) {
            }
        });
    };
    BaseService.prototype.GetDetails = function (callback, id, params) {
        wx.showLoading({
            title: "loading..."
        });
        var url = this.apiBaseUrl + "/api/" + this.apiPath + "/" + id;
        if (params && Object.keys(params).length > 0) {
            url += "?";
            for (var _i = 0, _a = Object.keys(params); _i < _a.length; _i++) {
                var name_2 = _a[_i];
                url += name_2 + "=" + params[name_2] + "&";
            }
        }
        wx.request({
            url: url,
            success: function (response) {
                console.log(response);
                wx.hideLoading();
                var result = response.data;
                callback(result);
            },
            fail: function (err) {
            }
        });
    };
    BaseService.prototype.Post = function (model) {
    };
    return BaseService;
}());
exports.BaseService = BaseService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiQmFzZVNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJCYXNlU2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOztBQUdBO0lBR0UscUJBQXNCLE9BQWM7UUFBZCxZQUFPLEdBQVAsT0FBTyxDQUFPO1FBRmpCLGVBQVUsR0FBVyx5R0FBeUcsQ0FBQztJQUdsSixDQUFDO0lBRU0seUJBQUcsR0FBVixVQUFXLFFBQWdELEVBQUUsTUFBVztRQUN0RSxFQUFFLENBQUMsV0FBVyxDQUFDO1lBQ2IsS0FBSyxFQUFFLFlBQVk7U0FDcEIsQ0FBQyxDQUFDO1FBQ0gsSUFBSSxHQUFHLEdBQU0sSUFBSSxDQUFDLFVBQVUsYUFBUSxJQUFJLENBQUMsT0FBUyxDQUFDO1FBQ25ELElBQUcsTUFBTSxJQUFJLFNBQVMsSUFBSSxNQUFNLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUM7WUFDdkQsR0FBRyxJQUFJLEdBQUcsQ0FBQztZQUNYLEtBQWdCLFVBQW1CLEVBQW5CLEtBQUEsTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsRUFBbkIsY0FBbUIsRUFBbkIsSUFBbUIsRUFBQztnQkFBaEMsSUFBSSxNQUFJLFNBQUE7Z0JBQ1YsR0FBRyxJQUFPLE1BQUksU0FBSSxNQUFNLENBQUMsTUFBSSxDQUFDLE1BQUcsQ0FBQzthQUNuQztTQUNGO1FBQ0QsT0FBTyxDQUFDLEdBQUcsQ0FBQyxVQUFRLEdBQUssQ0FBQyxDQUFDO1FBQzNCLEVBQUUsQ0FBQyxPQUFPLENBQUM7WUFDVixHQUFHLEVBQUUsR0FBRztZQUNSLE9BQU8sRUFBRSxVQUFDLFFBQVE7Z0JBQ2hCLE9BQU8sQ0FBQyxHQUFHLENBQUMsUUFBUSxDQUFDLENBQUM7Z0JBQ3RCLElBQUksTUFBTSxHQUEwQixRQUFRLENBQUMsSUFBSSxDQUFDO2dCQUNsRCxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQ2pCLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztZQUNuQixDQUFDO1lBQ0QsSUFBSSxFQUFFLFVBQUMsR0FBRztZQUVWLENBQUM7U0FDRCxDQUFDLENBQUM7SUFDTCxDQUFDO0lBRU0sNEJBQU0sR0FBYixVQUFjLFFBQXVDO1FBQ25ELEVBQUUsQ0FBQyxXQUFXLENBQUM7WUFDYixLQUFLLEVBQUUsWUFBWTtTQUNwQixDQUFDLENBQUM7UUFDSCxFQUFFLENBQUMsT0FBTyxDQUFDO1lBQ1QsR0FBRyxFQUFLLElBQUksQ0FBQyxVQUFVLGFBQVEsSUFBSSxDQUFDLE9BQVM7WUFDN0MsT0FBTyxFQUFFLFVBQUMsUUFBUTtnQkFDaEIsT0FBTyxDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQztnQkFDdEIsRUFBRSxDQUFDLFdBQVcsRUFBRSxDQUFDO2dCQUNqQixJQUFJLE1BQU0sR0FBa0IsUUFBUSxDQUFDLElBQUksQ0FBQztnQkFDMUMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO1lBQ25CLENBQUM7WUFDRCxJQUFJLEVBQUUsVUFBQyxHQUFHO1lBRVYsQ0FBQztTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7SUFFTSxnQ0FBVSxHQUFqQixVQUFrQixRQUE4QixFQUFFLEVBQWlCLEVBQUUsTUFBWTtRQUMvRSxFQUFFLENBQUMsV0FBVyxDQUFDO1lBQ2IsS0FBSyxFQUFFLFlBQVk7U0FDcEIsQ0FBQyxDQUFDO1FBQ0gsSUFBSSxHQUFHLEdBQU0sSUFBSSxDQUFDLFVBQVUsYUFBUSxJQUFJLENBQUMsT0FBTyxTQUFJLEVBQUksQ0FBQztRQUN6RCxJQUFHLE1BQU0sSUFBSSxNQUFNLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUM7WUFDMUMsR0FBRyxJQUFJLEdBQUcsQ0FBQztZQUNYLEtBQWdCLFVBQW1CLEVBQW5CLEtBQUEsTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsRUFBbkIsY0FBbUIsRUFBbkIsSUFBbUIsRUFBQztnQkFBaEMsSUFBSSxNQUFJLFNBQUE7Z0JBQ1YsR0FBRyxJQUFPLE1BQUksU0FBSSxNQUFNLENBQUMsTUFBSSxDQUFDLE1BQUcsQ0FBQzthQUNuQztTQUNGO1FBQ0QsRUFBRSxDQUFDLE9BQU8sQ0FBQztZQUNULEdBQUcsRUFBRSxHQUFHO1lBQ1IsT0FBTyxFQUFFLFVBQUMsUUFBUTtnQkFDaEIsT0FBTyxDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQztnQkFDdEIsRUFBRSxDQUFDLFdBQVcsRUFBRSxDQUFDO2dCQUNqQixJQUFJLE1BQU0sR0FBZ0IsUUFBUSxDQUFDLElBQUksQ0FBQztnQkFDeEMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO1lBQ25CLENBQUM7WUFDRCxJQUFJLEVBQUUsVUFBQyxHQUFHO1lBRVYsQ0FBQztTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7SUFHTSwwQkFBSSxHQUFYLFVBQVksS0FBYTtJQUV6QixDQUFDO0lBQ0gsa0JBQUM7QUFBRCxDQUFDLEFBL0VELElBK0VDO0FBL0VZLGtDQUFXIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUGFnZWRMaXN0RGF0YSB9IGZyb20gJy4vLi4vbW9kZWxzL1BhZ2VkTGlzdERhdGEnO1xyXG5pbXBvcnQgeyBSZXRyeUhlbHBlciB9IGZyb20gJy4uL3V0aWxzL1JldHJ5SGVscGVyJztcclxuXHJcbmV4cG9ydCBjbGFzcyBCYXNlU2VydmljZTxUTW9kZWw+e1xyXG4gIHByb3RlY3RlZCByZWFkb25seSBhcGlCYXNlVXJsOiBzdHJpbmcgPSBcImh0dHBzOi8vc2VydmljZS1iYWx4Zjdoci0xMjUxMjg4OTIzLmFwLXNoYW5naGFpLmFwaWdhdGV3YXkubXlxY2xvdWQuY29tL3JlbGVhc2UvcmVzZXJ2YXRpb25XeEFwcEdhdGV3YXlcIjtcclxuXHJcbiAgY29uc3RydWN0b3IocHJvdGVjdGVkIGFwaVBhdGg6c3RyaW5nKXtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBHZXQoY2FsbGJhY2s6IChyZXN1bHQ6UGFnZWRMaXN0RGF0YTxUTW9kZWw+KSA9PiB2b2lkLCBwYXJhbXM/OmFueSkge1xyXG4gICAgd3guc2hvd0xvYWRpbmcoe1xyXG4gICAgICB0aXRsZTogXCJsb2FkaW5nLi4uXCJcclxuICAgIH0pO1xyXG4gICAgbGV0IHVybCA9IGAke3RoaXMuYXBpQmFzZVVybH0vYXBpLyR7dGhpcy5hcGlQYXRofWA7XHJcbiAgICBpZihwYXJhbXMgIT0gdW5kZWZpbmVkICYmIE9iamVjdC5rZXlzKHBhcmFtcykubGVuZ3RoID4gMCl7XHJcbiAgICAgIHVybCArPSBcIj9cIjsgICAgICBcclxuICAgICAgZm9yKGxldCBuYW1lIG9mIE9iamVjdC5rZXlzKHBhcmFtcykpe1xyXG4gICAgICAgIHVybCArPSBgJHtuYW1lfT0ke3BhcmFtc1tuYW1lXX0mYDtcclxuICAgICAgfVxyXG4gICAgfVxyXG4gICAgY29uc29sZS5sb2coYHVybDogJHt1cmx9YCk7XHJcbiAgICB3eC5yZXF1ZXN0KHtcclxuICAgICB1cmw6IHVybCxcclxuICAgICBzdWNjZXNzOiAocmVzcG9uc2UpPT57XHJcbiAgICAgICBjb25zb2xlLmxvZyhyZXNwb25zZSk7XHJcbiAgICAgICBsZXQgcmVzdWx0ID0gPFBhZ2VkTGlzdERhdGE8VE1vZGVsPj5yZXNwb25zZS5kYXRhO1xyXG4gICAgICAgY2FsbGJhY2socmVzdWx0KTtcclxuICAgICAgIHd4LmhpZGVMb2FkaW5nKCk7XHJcbiAgICAgfSxcclxuICAgICBmYWlsOiAoZXJyKT0+e1xyXG5cclxuICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBHZXRBbGwoY2FsbGJhY2s6IChyZXN1bHQ6IEFycmF5PFRNb2RlbD4pPT52b2lkKTogdm9pZCB7XHJcbiAgICB3eC5zaG93TG9hZGluZyh7XHJcbiAgICAgIHRpdGxlOiBcImxvYWRpbmcuLi5cIlxyXG4gICAgfSk7XHJcbiAgICB3eC5yZXF1ZXN0KHtcclxuICAgICAgdXJsOiBgJHt0aGlzLmFwaUJhc2VVcmx9L2FwaS8ke3RoaXMuYXBpUGF0aH1gLFxyXG4gICAgICBzdWNjZXNzOiAocmVzcG9uc2UpID0+IHtcclxuICAgICAgICBjb25zb2xlLmxvZyhyZXNwb25zZSk7XHJcbiAgICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPEFycmF5PFRNb2RlbD4+cmVzcG9uc2UuZGF0YTtcclxuICAgICAgICBjYWxsYmFjayhyZXN1bHQpO1xyXG4gICAgICB9LFxyXG4gICAgICBmYWlsOiAoZXJyKT0+e1xyXG4gICAgICAgXHJcbiAgICAgIH1cclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIEdldERldGFpbHMoY2FsbGJhY2s6KHJlc3VsdDpUTW9kZWwpPT52b2lkLCBpZDogbnVtYmVyfHN0cmluZywgcGFyYW1zPzogYW55KTogdm9pZCB7XHJcbiAgICB3eC5zaG93TG9hZGluZyh7XHJcbiAgICAgIHRpdGxlOiBcImxvYWRpbmcuLi5cIlxyXG4gICAgfSk7XHJcbiAgICBsZXQgdXJsID0gYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvJHt0aGlzLmFwaVBhdGh9LyR7aWR9YDtcclxuICAgIGlmKHBhcmFtcyAmJiBPYmplY3Qua2V5cyhwYXJhbXMpLmxlbmd0aCA+IDApe1xyXG4gICAgICB1cmwgKz0gXCI/XCI7XHJcbiAgICAgIGZvcihsZXQgbmFtZSBvZiBPYmplY3Qua2V5cyhwYXJhbXMpKXtcclxuICAgICAgICB1cmwgKz0gYCR7bmFtZX09JHtwYXJhbXNbbmFtZV19JmA7XHJcbiAgICAgIH1cclxuICAgIH1cclxuICAgIHd4LnJlcXVlc3Qoe1xyXG4gICAgICB1cmw6IHVybCxcclxuICAgICAgc3VjY2VzczogKHJlc3BvbnNlKSA9PiB7XHJcbiAgICAgICAgY29uc29sZS5sb2cocmVzcG9uc2UpO1xyXG4gICAgICAgIHd4LmhpZGVMb2FkaW5nKCk7XHJcbiAgICAgICAgbGV0IHJlc3VsdCA9IDxUTW9kZWw+PGFueT5yZXNwb25zZS5kYXRhO1xyXG4gICAgICAgIGNhbGxiYWNrKHJlc3VsdCk7XHJcbiAgICAgIH0sXHJcbiAgICAgIGZhaWw6IChlcnIpPT57XHJcbiAgICAgICBcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuXHJcbiAgcHVibGljIFBvc3QobW9kZWw6IFRNb2RlbCk6IHZvaWQge1xyXG4gICAgLy9yZXR1cm4gdGhpcy5odHRwLnBvc3QoYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvJHt0aGlzLmFwaVBhdGh9YCwgbW9kZWwpO1xyXG4gIH1cclxufVxyXG4iXX0=