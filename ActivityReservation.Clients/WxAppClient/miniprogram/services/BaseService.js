"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BaseService = (function () {
    function BaseService(apiPath) {
        this.apiPath = apiPath;
        this.apiBaseUrl = apiPath;
    }
    BaseService.prototype.Get = function (params) {
        var url = this.apiBaseUrl + "/api/" + this.apiPath;
        if (params != undefined && Object.keys(params).length > 0) {
            url += "?";
            for (var _i = 0, _a = Object.keys(params); _i < _a.length; _i++) {
                var name_1 = _a[_i];
                url += name_1 + "=" + params[name_1] + "&";
            }
        }
        wx.request({
            url: url,
            success: function (response) {
                console.log(response.data);
            }
        });
    };
    BaseService.prototype.GetAll = function () {
    };
    BaseService.prototype.GetDetails = function (id, params) {
        var url = this.apiBaseUrl + "/api/" + this.apiPath + "/" + id;
        if (params && Object.keys(params).length > 0) {
            url += "?";
            for (var _i = 0, _a = Object.keys(params); _i < _a.length; _i++) {
                var name_2 = _a[_i];
                url += name_2 + "=" + params[name_2] + "&";
            }
        }
    };
    BaseService.prototype.Post = function (model) {
    };
    return BaseService;
}());
exports.BaseService = BaseService;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiQmFzZVNlcnZpY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJCYXNlU2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOztBQUVBO0lBR0UscUJBQXNCLE9BQWM7UUFBZCxZQUFPLEdBQVAsT0FBTyxDQUFPO1FBQ2xDLElBQUksQ0FBQyxVQUFVLEdBQUcsT0FBTyxDQUFDO0lBQzVCLENBQUM7SUFFTSx5QkFBRyxHQUFWLFVBQVcsTUFBVztRQUNwQixJQUFJLEdBQUcsR0FBTSxJQUFJLENBQUMsVUFBVSxhQUFRLElBQUksQ0FBQyxPQUFTLENBQUM7UUFDbkQsSUFBRyxNQUFNLElBQUksU0FBUyxJQUFJLE1BQU0sQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsTUFBTSxHQUFHLENBQUMsRUFBQztZQUN2RCxHQUFHLElBQUksR0FBRyxDQUFDO1lBQ1gsS0FBZ0IsVUFBbUIsRUFBbkIsS0FBQSxNQUFNLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxFQUFuQixjQUFtQixFQUFuQixJQUFtQixFQUFDO2dCQUFoQyxJQUFJLE1BQUksU0FBQTtnQkFDVixHQUFHLElBQU8sTUFBSSxTQUFJLE1BQU0sQ0FBQyxNQUFJLENBQUMsTUFBRyxDQUFDO2FBQ25DO1NBQ0Y7UUFDRCxFQUFFLENBQUMsT0FBTyxDQUFDO1lBQ1YsR0FBRyxFQUFFLEdBQUc7WUFDUixPQUFPLEVBQUUsVUFBQyxRQUFRO2dCQUNoQixPQUFPLENBQUMsR0FBRyxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsQ0FBQztZQUM3QixDQUFDO1NBQ0QsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUVNLDRCQUFNLEdBQWI7SUFFQSxDQUFDO0lBRU0sZ0NBQVUsR0FBakIsVUFBa0IsRUFBaUIsRUFBRSxNQUFZO1FBQy9DLElBQUksR0FBRyxHQUFNLElBQUksQ0FBQyxVQUFVLGFBQVEsSUFBSSxDQUFDLE9BQU8sU0FBSSxFQUFJLENBQUM7UUFDekQsSUFBRyxNQUFNLElBQUksTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQyxNQUFNLEdBQUcsQ0FBQyxFQUFDO1lBQzFDLEdBQUcsSUFBSSxHQUFHLENBQUM7WUFDWCxLQUFnQixVQUFtQixFQUFuQixLQUFBLE1BQU0sQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLEVBQW5CLGNBQW1CLEVBQW5CLElBQW1CLEVBQUM7Z0JBQWhDLElBQUksTUFBSSxTQUFBO2dCQUNWLEdBQUcsSUFBTyxNQUFJLFNBQUksTUFBTSxDQUFDLE1BQUksQ0FBQyxNQUFHLENBQUM7YUFDbkM7U0FDRjtJQUVILENBQUM7SUFHTSwwQkFBSSxHQUFYLFVBQVksS0FBYTtJQUV6QixDQUFDO0lBQ0gsa0JBQUM7QUFBRCxDQUFDLEFBMUNELElBMENDO0FBMUNZLGtDQUFXIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUGFnZWRMaXN0RGF0YSB9IGZyb20gJy4vLi4vbW9kZWxzL1BhZ2VkTGlzdERhdGEnO1xyXG5cclxuZXhwb3J0IGNsYXNzIEJhc2VTZXJ2aWNlPFRNb2RlbD57XHJcbiAgcHJvdGVjdGVkIHJlYWRvbmx5IGFwaUJhc2VVcmw6c3RyaW5nO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcm90ZWN0ZWQgYXBpUGF0aDpzdHJpbmcpe1xyXG4gICAgdGhpcy5hcGlCYXNlVXJsID0gYXBpUGF0aDtcclxuICB9XHJcblxyXG4gIHB1YmxpYyBHZXQocGFyYW1zPzphbnkpIHtcclxuICAgIGxldCB1cmwgPSBgJHt0aGlzLmFwaUJhc2VVcmx9L2FwaS8ke3RoaXMuYXBpUGF0aH1gO1xyXG4gICAgaWYocGFyYW1zICE9IHVuZGVmaW5lZCAmJiBPYmplY3Qua2V5cyhwYXJhbXMpLmxlbmd0aCA+IDApe1xyXG4gICAgICB1cmwgKz0gXCI/XCI7ICAgICAgXHJcbiAgICAgIGZvcihsZXQgbmFtZSBvZiBPYmplY3Qua2V5cyhwYXJhbXMpKXtcclxuICAgICAgICB1cmwgKz0gYCR7bmFtZX09JHtwYXJhbXNbbmFtZV19JmA7XHJcbiAgICAgIH1cclxuICAgIH1cclxuICAgIHd4LnJlcXVlc3Qoe1xyXG4gICAgIHVybDogdXJsLFxyXG4gICAgIHN1Y2Nlc3M6IChyZXNwb25zZSk9PntcclxuICAgICAgIGNvbnNvbGUubG9nKHJlc3BvbnNlLmRhdGEpO1xyXG4gICAgIH1cclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgcHVibGljIEdldEFsbCgpOiB2b2lkIHtcclxuICAgIC8vcmV0dXJuIHRoaXMuaHR0cC5nZXQ8QXJyYXk8VE1vZGVsPj4oYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvJHt0aGlzLmFwaVBhdGh9YCk7XHJcbiAgfVxyXG5cclxuICBwdWJsaWMgR2V0RGV0YWlscyhpZDogbnVtYmVyfHN0cmluZywgcGFyYW1zPzogYW55KTogdm9pZCB7XHJcbiAgICBsZXQgdXJsID0gYCR7dGhpcy5hcGlCYXNlVXJsfS9hcGkvJHt0aGlzLmFwaVBhdGh9LyR7aWR9YDtcclxuICAgIGlmKHBhcmFtcyAmJiBPYmplY3Qua2V5cyhwYXJhbXMpLmxlbmd0aCA+IDApe1xyXG4gICAgICB1cmwgKz0gXCI/XCI7XHJcbiAgICAgIGZvcihsZXQgbmFtZSBvZiBPYmplY3Qua2V5cyhwYXJhbXMpKXtcclxuICAgICAgICB1cmwgKz0gYCR7bmFtZX09JHtwYXJhbXNbbmFtZV19JmA7XHJcbiAgICAgIH1cclxuICAgIH1cclxuICAgIC8vcmV0dXJuIHRoaXMuaHR0cC5nZXQ8VE1vZGVsPih1cmwpO1xyXG4gIH1cclxuXHJcblxyXG4gIHB1YmxpYyBQb3N0KG1vZGVsOiBUTW9kZWwpOiB2b2lkIHtcclxuICAgIC8vcmV0dXJuIHRoaXMuaHR0cC5wb3N0KGAke3RoaXMuYXBpQmFzZVVybH0vYXBpLyR7dGhpcy5hcGlQYXRofWAsIG1vZGVsKTtcclxuICB9XHJcbn1cclxuIl19