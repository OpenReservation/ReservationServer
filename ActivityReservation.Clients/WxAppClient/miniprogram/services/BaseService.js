"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BaseService = (function () {
    function BaseService(apiPath) {
        this.apiPath = apiPath;
        this.apiBaseUrl = "https://reservation.weihanli.xyz";
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
                var result = response.data;
                callback(result);
                wx.hideLoading();
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
                wx.hideLoading();
                var result = response.data;
                callback(result);
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
            url: this.apiBaseUrl + "/api/" + this.apiPath,
            success: function (response) {
                wx.hideLoading();
                var result = response.data;
                callback(result);
            }
        });
    };
    BaseService.prototype.Post = function (model) {
    };
    return BaseService;
}());
exports.BaseService = BaseService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiQmFzZVNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJCYXNlU2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOztBQUVBO0lBR0UscUJBQXNCLE9BQWM7UUFBZCxZQUFPLEdBQVAsT0FBTyxDQUFPO1FBRmpCLGVBQVUsR0FBVSxrQ0FBa0MsQ0FBQztJQUcxRSxDQUFDO0lBRU0seUJBQUcsR0FBVixVQUFXLFFBQWdELEVBQUUsTUFBVztRQUN0RSxFQUFFLENBQUMsV0FBVyxDQUFDO1lBQ2IsS0FBSyxFQUFFLFlBQVk7U0FDcEIsQ0FBQyxDQUFDO1FBQ0gsSUFBSSxHQUFHLEdBQU0sSUFBSSxDQUFDLFVBQVUsYUFBUSxJQUFJLENBQUMsT0FBUyxDQUFDO1FBQ25ELElBQUcsTUFBTSxJQUFJLFNBQVMsSUFBSSxNQUFNLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUM7WUFDdkQsR0FBRyxJQUFJLEdBQUcsQ0FBQztZQUNYLEtBQWdCLFVBQW1CLEVBQW5CLEtBQUEsTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsRUFBbkIsY0FBbUIsRUFBbkIsSUFBbUIsRUFBQztnQkFBaEMsSUFBSSxNQUFJLFNBQUE7Z0JBQ1YsR0FBRyxJQUFPLE1BQUksU0FBSSxNQUFNLENBQUMsTUFBSSxDQUFDLE1BQUcsQ0FBQzthQUNuQztTQUNGO1FBQ0QsT0FBTyxDQUFDLEdBQUcsQ0FBQyxVQUFRLEdBQUssQ0FBQyxDQUFDO1FBQzNCLEVBQUUsQ0FBQyxPQUFPLENBQUM7WUFDVixHQUFHLEVBQUUsR0FBRztZQUNSLE9BQU8sRUFBRSxVQUFDLFFBQVE7Z0JBQ2hCLElBQUksTUFBTSxHQUEwQixRQUFRLENBQUMsSUFBSSxDQUFDO2dCQUNsRCxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQ2pCLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztZQUNuQixDQUFDO1NBQ0QsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUVNLDRCQUFNLEdBQWIsVUFBYyxRQUF1QztRQUNuRCxFQUFFLENBQUMsV0FBVyxDQUFDO1lBQ2IsS0FBSyxFQUFFLFlBQVk7U0FDcEIsQ0FBQyxDQUFDO1FBQ0gsRUFBRSxDQUFDLE9BQU8sQ0FBQztZQUNULEdBQUcsRUFBSyxJQUFJLENBQUMsVUFBVSxhQUFRLElBQUksQ0FBQyxPQUFTO1lBQzdDLE9BQU8sRUFBRSxVQUFDLFFBQVE7Z0JBQ2hCLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztnQkFDakIsSUFBSSxNQUFNLEdBQWtCLFFBQVEsQ0FBQyxJQUFJLENBQUM7Z0JBQzFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztZQUNuQixDQUFDO1NBQ0YsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUVNLGdDQUFVLEdBQWpCLFVBQWtCLFFBQThCLEVBQUUsRUFBaUIsRUFBRSxNQUFZO1FBQy9FLEVBQUUsQ0FBQyxXQUFXLENBQUM7WUFDYixLQUFLLEVBQUUsWUFBWTtTQUNwQixDQUFDLENBQUM7UUFDSCxJQUFJLEdBQUcsR0FBTSxJQUFJLENBQUMsVUFBVSxhQUFRLElBQUksQ0FBQyxPQUFPLFNBQUksRUFBSSxDQUFDO1FBQ3pELElBQUcsTUFBTSxJQUFJLE1BQU0sQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsTUFBTSxHQUFHLENBQUMsRUFBQztZQUMxQyxHQUFHLElBQUksR0FBRyxDQUFDO1lBQ1gsS0FBZ0IsVUFBbUIsRUFBbkIsS0FBQSxNQUFNLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxFQUFuQixjQUFtQixFQUFuQixJQUFtQixFQUFDO2dCQUFoQyxJQUFJLE1BQUksU0FBQTtnQkFDVixHQUFHLElBQU8sTUFBSSxTQUFJLE1BQU0sQ0FBQyxNQUFJLENBQUMsTUFBRyxDQUFDO2FBQ25DO1NBQ0Y7UUFDRCxFQUFFLENBQUMsT0FBTyxDQUFDO1lBQ1QsR0FBRyxFQUFLLElBQUksQ0FBQyxVQUFVLGFBQVEsSUFBSSxDQUFDLE9BQVM7WUFDN0MsT0FBTyxFQUFFLFVBQUMsUUFBUTtnQkFDaEIsRUFBRSxDQUFDLFdBQVcsRUFBRSxDQUFDO2dCQUNqQixJQUFJLE1BQU0sR0FBZ0IsUUFBUSxDQUFDLElBQUksQ0FBQztnQkFDeEMsUUFBUSxDQUFDLE1BQU0sQ0FBQyxDQUFDO1lBQ25CLENBQUM7U0FDRixDQUFDLENBQUM7SUFDTCxDQUFDO0lBR00sMEJBQUksR0FBWCxVQUFZLEtBQWE7SUFFekIsQ0FBQztJQUNILGtCQUFDO0FBQUQsQ0FBQyxBQW5FRCxJQW1FQztBQW5FWSxrQ0FBVyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFBhZ2VkTGlzdERhdGEgfSBmcm9tICcuLy4uL21vZGVscy9QYWdlZExpc3REYXRhJztcclxuXHJcbmV4cG9ydCBjbGFzcyBCYXNlU2VydmljZTxUTW9kZWw+e1xyXG4gIHByb3RlY3RlZCByZWFkb25seSBhcGlCYXNlVXJsOnN0cmluZyA9IFwiaHR0cHM6Ly9yZXNlcnZhdGlvbi53ZWloYW5saS54eXpcIjtcclxuXHJcbiAgY29uc3RydWN0b3IocHJvdGVjdGVkIGFwaVBhdGg6c3RyaW5nKXtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBHZXQoY2FsbGJhY2s6IChyZXN1bHQ6UGFnZWRMaXN0RGF0YTxUTW9kZWw+KSA9PiB2b2lkLCBwYXJhbXM/OmFueSkge1xyXG4gICAgd3guc2hvd0xvYWRpbmcoe1xyXG4gICAgICB0aXRsZTogXCJsb2FkaW5nLi4uXCJcclxuICAgIH0pO1xyXG4gICAgbGV0IHVybCA9IGAke3RoaXMuYXBpQmFzZVVybH0vYXBpLyR7dGhpcy5hcGlQYXRofWA7XHJcbiAgICBpZihwYXJhbXMgIT0gdW5kZWZpbmVkICYmIE9iamVjdC5rZXlzKHBhcmFtcykubGVuZ3RoID4gMCl7XHJcbiAgICAgIHVybCArPSBcIj9cIjsgICAgICBcclxuICAgICAgZm9yKGxldCBuYW1lIG9mIE9iamVjdC5rZXlzKHBhcmFtcykpe1xyXG4gICAgICAgIHVybCArPSBgJHtuYW1lfT0ke3BhcmFtc1tuYW1lXX0mYDtcclxuICAgICAgfVxyXG4gICAgfVxyXG4gICAgY29uc29sZS5sb2coYHVybDogJHt1cmx9YCk7XHJcbiAgICB3eC5yZXF1ZXN0KHtcclxuICAgICB1cmw6IHVybCxcclxuICAgICBzdWNjZXNzOiAocmVzcG9uc2UpPT57XHJcbiAgICAgICBsZXQgcmVzdWx0ID0gPFBhZ2VkTGlzdERhdGE8VE1vZGVsPj5yZXNwb25zZS5kYXRhO1xyXG4gICAgICAgY2FsbGJhY2socmVzdWx0KTtcclxuICAgICAgIHd4LmhpZGVMb2FkaW5nKCk7XHJcbiAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgR2V0QWxsKGNhbGxiYWNrOiAocmVzdWx0OiBBcnJheTxUTW9kZWw+KT0+dm9pZCk6IHZvaWQge1xyXG4gICAgd3guc2hvd0xvYWRpbmcoe1xyXG4gICAgICB0aXRsZTogXCJsb2FkaW5nLi4uXCJcclxuICAgIH0pO1xyXG4gICAgd3gucmVxdWVzdCh7XHJcbiAgICAgIHVybDogYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvJHt0aGlzLmFwaVBhdGh9YCxcclxuICAgICAgc3VjY2VzczogKHJlc3BvbnNlKSA9PiB7XHJcbiAgICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPEFycmF5PFRNb2RlbD4+cmVzcG9uc2UuZGF0YTtcclxuICAgICAgICBjYWxsYmFjayhyZXN1bHQpO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBHZXREZXRhaWxzKGNhbGxiYWNrOihyZXN1bHQ6VE1vZGVsKT0+dm9pZCwgaWQ6IG51bWJlcnxzdHJpbmcsIHBhcmFtcz86IGFueSk6IHZvaWQge1xyXG4gICAgd3guc2hvd0xvYWRpbmcoe1xyXG4gICAgICB0aXRsZTogXCJsb2FkaW5nLi4uXCJcclxuICAgIH0pO1xyXG4gICAgbGV0IHVybCA9IGAke3RoaXMuYXBpQmFzZVVybH0vYXBpLyR7dGhpcy5hcGlQYXRofS8ke2lkfWA7XHJcbiAgICBpZihwYXJhbXMgJiYgT2JqZWN0LmtleXMocGFyYW1zKS5sZW5ndGggPiAwKXtcclxuICAgICAgdXJsICs9IFwiP1wiO1xyXG4gICAgICBmb3IobGV0IG5hbWUgb2YgT2JqZWN0LmtleXMocGFyYW1zKSl7XHJcbiAgICAgICAgdXJsICs9IGAke25hbWV9PSR7cGFyYW1zW25hbWVdfSZgO1xyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgICB3eC5yZXF1ZXN0KHtcclxuICAgICAgdXJsOiBgJHt0aGlzLmFwaUJhc2VVcmx9L2FwaS8ke3RoaXMuYXBpUGF0aH1gLFxyXG4gICAgICBzdWNjZXNzOiAocmVzcG9uc2UpID0+IHtcclxuICAgICAgICB3eC5oaWRlTG9hZGluZygpO1xyXG4gICAgICAgIGxldCByZXN1bHQgPSA8VE1vZGVsPjxhbnk+cmVzcG9uc2UuZGF0YTtcclxuICAgICAgICBjYWxsYmFjayhyZXN1bHQpO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG5cclxuICBwdWJsaWMgUG9zdChtb2RlbDogVE1vZGVsKTogdm9pZCB7XHJcbiAgICAvL3JldHVybiB0aGlzLmh0dHAucG9zdChgJHt0aGlzLmFwaUJhc2VVcmx9L2FwaS8ke3RoaXMuYXBpUGF0aH1gLCBtb2RlbCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==