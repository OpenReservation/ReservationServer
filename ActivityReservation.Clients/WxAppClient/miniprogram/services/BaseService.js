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
        console.log("request url: " + url);
        wx.request({
            url: url,
            success: function (response) {
                console.log(response);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiQmFzZVNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJCYXNlU2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOztBQUVBO0lBR0UscUJBQXNCLE9BQWM7UUFBZCxZQUFPLEdBQVAsT0FBTyxDQUFPO1FBRmpCLGVBQVUsR0FBVyx5R0FBeUcsQ0FBQztJQUdsSixDQUFDO0lBRU0seUJBQUcsR0FBVixVQUFXLFFBQWdELEVBQUUsTUFBVztRQUN0RSxFQUFFLENBQUMsV0FBVyxDQUFDO1lBQ2IsS0FBSyxFQUFFLFlBQVk7U0FDcEIsQ0FBQyxDQUFDO1FBQ0gsSUFBSSxHQUFHLEdBQU0sSUFBSSxDQUFDLFVBQVUsYUFBUSxJQUFJLENBQUMsT0FBUyxDQUFDO1FBQ25ELElBQUcsTUFBTSxJQUFJLFNBQVMsSUFBSSxNQUFNLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUM7WUFDdkQsR0FBRyxJQUFJLEdBQUcsQ0FBQztZQUNYLEtBQWdCLFVBQW1CLEVBQW5CLEtBQUEsTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsRUFBbkIsY0FBbUIsRUFBbkIsSUFBbUIsRUFBQztnQkFBaEMsSUFBSSxNQUFJLFNBQUE7Z0JBQ1YsR0FBRyxJQUFPLE1BQUksU0FBSSxNQUFNLENBQUMsTUFBSSxDQUFDLE1BQUcsQ0FBQzthQUNuQztTQUNGO1FBQ0QsT0FBTyxDQUFDLEdBQUcsQ0FBQyxVQUFRLEdBQUssQ0FBQyxDQUFDO1FBQzNCLEVBQUUsQ0FBQyxPQUFPLENBQUM7WUFDVixHQUFHLEVBQUUsR0FBRztZQUNSLE9BQU8sRUFBRSxVQUFDLFFBQVE7Z0JBQ2hCLE9BQU8sQ0FBQyxHQUFHLENBQUMsUUFBUSxDQUFDLENBQUM7Z0JBQ3RCLElBQUksTUFBTSxHQUEwQixRQUFRLENBQUMsSUFBSSxDQUFDO2dCQUNsRCxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQ2pCLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztZQUNuQixDQUFDO1NBQ0QsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUVNLDRCQUFNLEdBQWIsVUFBYyxRQUF1QztRQUNuRCxFQUFFLENBQUMsV0FBVyxDQUFDO1lBQ2IsS0FBSyxFQUFFLFlBQVk7U0FDcEIsQ0FBQyxDQUFDO1FBQ0gsRUFBRSxDQUFDLE9BQU8sQ0FBQztZQUNULEdBQUcsRUFBSyxJQUFJLENBQUMsVUFBVSxhQUFRLElBQUksQ0FBQyxPQUFTO1lBQzdDLE9BQU8sRUFBRSxVQUFDLFFBQVE7Z0JBQ2hCLE9BQU8sQ0FBQyxHQUFHLENBQUMsUUFBUSxDQUFDLENBQUM7Z0JBQ3RCLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztnQkFDakIsSUFBSSxNQUFNLEdBQWtCLFFBQVEsQ0FBQyxJQUFJLENBQUM7Z0JBQzFDLFFBQVEsQ0FBQyxNQUFNLENBQUMsQ0FBQztZQUNuQixDQUFDO1NBQ0YsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUVNLGdDQUFVLEdBQWpCLFVBQWtCLFFBQThCLEVBQUUsRUFBaUIsRUFBRSxNQUFZO1FBQy9FLEVBQUUsQ0FBQyxXQUFXLENBQUM7WUFDYixLQUFLLEVBQUUsWUFBWTtTQUNwQixDQUFDLENBQUM7UUFDSCxJQUFJLEdBQUcsR0FBTSxJQUFJLENBQUMsVUFBVSxhQUFRLElBQUksQ0FBQyxPQUFPLFNBQUksRUFBSSxDQUFDO1FBQ3pELElBQUcsTUFBTSxJQUFJLE1BQU0sQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsTUFBTSxHQUFHLENBQUMsRUFBQztZQUMxQyxHQUFHLElBQUksR0FBRyxDQUFDO1lBQ1gsS0FBZ0IsVUFBbUIsRUFBbkIsS0FBQSxNQUFNLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxFQUFuQixjQUFtQixFQUFuQixJQUFtQixFQUFDO2dCQUFoQyxJQUFJLE1BQUksU0FBQTtnQkFDVixHQUFHLElBQU8sTUFBSSxTQUFJLE1BQU0sQ0FBQyxNQUFJLENBQUMsTUFBRyxDQUFDO2FBQ25DO1NBQ0Y7UUFDRCxPQUFPLENBQUMsR0FBRyxDQUFDLGtCQUFnQixHQUFLLENBQUMsQ0FBQztRQUNuQyxFQUFFLENBQUMsT0FBTyxDQUFDO1lBQ1QsR0FBRyxFQUFFLEdBQUc7WUFDUixPQUFPLEVBQUUsVUFBQyxRQUFRO2dCQUNoQixPQUFPLENBQUMsR0FBRyxDQUFDLFFBQVEsQ0FBQyxDQUFDO2dCQUN0QixFQUFFLENBQUMsV0FBVyxFQUFFLENBQUM7Z0JBQ2pCLElBQUksTUFBTSxHQUFnQixRQUFRLENBQUMsSUFBSSxDQUFDO2dCQUN4QyxRQUFRLENBQUMsTUFBTSxDQUFDLENBQUM7WUFDbkIsQ0FBQztTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7SUFHTSwwQkFBSSxHQUFYLFVBQVksS0FBYTtJQUV6QixDQUFDO0lBQ0gsa0JBQUM7QUFBRCxDQUFDLEFBdkVELElBdUVDO0FBdkVZLGtDQUFXIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUGFnZWRMaXN0RGF0YSB9IGZyb20gJy4vLi4vbW9kZWxzL1BhZ2VkTGlzdERhdGEnO1xyXG5cclxuZXhwb3J0IGNsYXNzIEJhc2VTZXJ2aWNlPFRNb2RlbD57XHJcbiAgcHJvdGVjdGVkIHJlYWRvbmx5IGFwaUJhc2VVcmw6IHN0cmluZyA9IFwiaHR0cHM6Ly9zZXJ2aWNlLWJhbHhmN2hyLTEyNTEyODg5MjMuYXAtc2hhbmdoYWkuYXBpZ2F0ZXdheS5teXFjbG91ZC5jb20vcmVsZWFzZS9yZXNlcnZhdGlvbld4QXBwR2F0ZXdheVwiO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgYXBpUGF0aDpzdHJpbmcpe1xyXG4gIH1cclxuXHJcbiAgcHVibGljIEdldChjYWxsYmFjazogKHJlc3VsdDpQYWdlZExpc3REYXRhPFRNb2RlbD4pID0+IHZvaWQsIHBhcmFtcz86YW55KSB7XHJcbiAgICB3eC5zaG93TG9hZGluZyh7XHJcbiAgICAgIHRpdGxlOiBcImxvYWRpbmcuLi5cIlxyXG4gICAgfSk7XHJcbiAgICBsZXQgdXJsID0gYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvJHt0aGlzLmFwaVBhdGh9YDtcclxuICAgIGlmKHBhcmFtcyAhPSB1bmRlZmluZWQgJiYgT2JqZWN0LmtleXMocGFyYW1zKS5sZW5ndGggPiAwKXtcclxuICAgICAgdXJsICs9IFwiP1wiOyAgICAgIFxyXG4gICAgICBmb3IobGV0IG5hbWUgb2YgT2JqZWN0LmtleXMocGFyYW1zKSl7XHJcbiAgICAgICAgdXJsICs9IGAke25hbWV9PSR7cGFyYW1zW25hbWVdfSZgO1xyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgICBjb25zb2xlLmxvZyhgdXJsOiAke3VybH1gKTtcclxuICAgIHd4LnJlcXVlc3Qoe1xyXG4gICAgIHVybDogdXJsLFxyXG4gICAgIHN1Y2Nlc3M6IChyZXNwb25zZSk9PntcclxuICAgICAgIGNvbnNvbGUubG9nKHJlc3BvbnNlKTtcclxuICAgICAgIGxldCByZXN1bHQgPSA8UGFnZWRMaXN0RGF0YTxUTW9kZWw+PnJlc3BvbnNlLmRhdGE7XHJcbiAgICAgICBjYWxsYmFjayhyZXN1bHQpO1xyXG4gICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBHZXRBbGwoY2FsbGJhY2s6IChyZXN1bHQ6IEFycmF5PFRNb2RlbD4pPT52b2lkKTogdm9pZCB7XHJcbiAgICB3eC5zaG93TG9hZGluZyh7XHJcbiAgICAgIHRpdGxlOiBcImxvYWRpbmcuLi5cIlxyXG4gICAgfSk7XHJcbiAgICB3eC5yZXF1ZXN0KHtcclxuICAgICAgdXJsOiBgJHt0aGlzLmFwaUJhc2VVcmx9L2FwaS8ke3RoaXMuYXBpUGF0aH1gLFxyXG4gICAgICBzdWNjZXNzOiAocmVzcG9uc2UpID0+IHtcclxuICAgICAgICBjb25zb2xlLmxvZyhyZXNwb25zZSk7XHJcbiAgICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPEFycmF5PFRNb2RlbD4+cmVzcG9uc2UuZGF0YTtcclxuICAgICAgICBjYWxsYmFjayhyZXN1bHQpO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBHZXREZXRhaWxzKGNhbGxiYWNrOihyZXN1bHQ6VE1vZGVsKT0+dm9pZCwgaWQ6IG51bWJlcnxzdHJpbmcsIHBhcmFtcz86IGFueSk6IHZvaWQge1xyXG4gICAgd3guc2hvd0xvYWRpbmcoe1xyXG4gICAgICB0aXRsZTogXCJsb2FkaW5nLi4uXCJcclxuICAgIH0pO1xyXG4gICAgbGV0IHVybCA9IGAke3RoaXMuYXBpQmFzZVVybH0vYXBpLyR7dGhpcy5hcGlQYXRofS8ke2lkfWA7XHJcbiAgICBpZihwYXJhbXMgJiYgT2JqZWN0LmtleXMocGFyYW1zKS5sZW5ndGggPiAwKXtcclxuICAgICAgdXJsICs9IFwiP1wiO1xyXG4gICAgICBmb3IobGV0IG5hbWUgb2YgT2JqZWN0LmtleXMocGFyYW1zKSl7XHJcbiAgICAgICAgdXJsICs9IGAke25hbWV9PSR7cGFyYW1zW25hbWVdfSZgO1xyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgICBjb25zb2xlLmxvZyhgcmVxdWVzdCB1cmw6ICR7dXJsfWApO1xyXG4gICAgd3gucmVxdWVzdCh7XHJcbiAgICAgIHVybDogdXJsLFxyXG4gICAgICBzdWNjZXNzOiAocmVzcG9uc2UpID0+IHtcclxuICAgICAgICBjb25zb2xlLmxvZyhyZXNwb25zZSk7XHJcbiAgICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICAgICBsZXQgcmVzdWx0ID0gPFRNb2RlbD48YW55PnJlc3BvbnNlLmRhdGE7XHJcbiAgICAgICAgY2FsbGJhY2socmVzdWx0KTtcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuXHJcbiAgcHVibGljIFBvc3QobW9kZWw6IFRNb2RlbCk6IHZvaWQge1xyXG4gICAgLy9yZXR1cm4gdGhpcy5odHRwLnBvc3QoYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvJHt0aGlzLmFwaVBhdGh9YCwgbW9kZWwpO1xyXG4gIH1cclxufVxyXG4iXX0=