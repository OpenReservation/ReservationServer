"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
Page({
    data: {
        notices: [],
        pageNum: 1,
        pageSize: 10,
        totalPage: 0,
    },
    onLoad: function () {
        this.loadNotice(this, 1, 10);
    },
    loadNotice: function (page, pageNum, pageSize) {
        if (pageNum === void 0) { pageNum = 1; }
        if (pageSize === void 0) { pageSize = 10; }
        wx.request({
            url: "https://reservation.weihanli.xyz/api/notice?pageNum=" + pageNum + "&pageSize=" + pageSize,
            success: function (res) {
                console.log(res.data);
                var result = res.data;
                if (null == result || undefined == result) {
                    console.log("result as is null");
                    return;
                }
                console.log("notice result: " + JSON.stringify(result));
                page.setData({
                    notices: result.Data,
                    pageNum: result.PageNumber,
                    pageSize: result.PageSize,
                    totalPage: result.PageCount
                });
            }
        });
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibm90aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibm90aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O0FBR0EsSUFBSSxDQUFDO0lBQ0gsSUFBSSxFQUFFO1FBQ0osT0FBTyxFQUFFLEVBQW1CO1FBQzVCLE9BQU8sRUFBRSxDQUFDO1FBQ1YsUUFBUSxFQUFFLEVBQUU7UUFDWixTQUFTLEVBQUUsQ0FBQztLQUNiO0lBQ0QsTUFBTTtRQUNKLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQztJQUMvQixDQUFDO0lBRUQsVUFBVSxFQUFWLFVBQVcsSUFBdUIsRUFBRSxPQUFrQixFQUFFLFFBQW9CO1FBQXhDLHdCQUFBLEVBQUEsV0FBa0I7UUFBRSx5QkFBQSxFQUFBLGFBQW9CO1FBQzFFLEVBQUUsQ0FBQyxPQUFPLENBQUM7WUFDVCxHQUFHLEVBQUUseURBQXVELE9BQU8sa0JBQWEsUUFBVTtZQUUxRixPQUFPLEVBQUUsVUFBQyxHQUFHO2dCQUNYLE9BQU8sQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLElBQUksQ0FBQyxDQUFBO2dCQUNyQixJQUFJLE1BQU0sR0FBRyxHQUFHLENBQUMsSUFBNkIsQ0FBQztnQkFDL0MsSUFBRyxJQUFJLElBQUksTUFBTSxJQUFJLFNBQVMsSUFBSSxNQUFNLEVBQUM7b0JBQ3ZDLE9BQU8sQ0FBQyxHQUFHLENBQUMsbUJBQW1CLENBQUMsQ0FBQztvQkFDakMsT0FBTztpQkFDUjtnQkFDRCxPQUFPLENBQUMsR0FBRyxDQUFDLG9CQUFrQixJQUFJLENBQUMsU0FBUyxDQUFDLE1BQU0sQ0FBRyxDQUFDLENBQUM7Z0JBQ3hELElBQUksQ0FBQyxPQUFPLENBQUM7b0JBQ1gsT0FBTyxFQUFFLE1BQU0sQ0FBQyxJQUFJO29CQUNwQixPQUFPLEVBQUUsTUFBTSxDQUFDLFVBQVU7b0JBQzFCLFFBQVEsRUFBRSxNQUFNLENBQUMsUUFBUTtvQkFDekIsU0FBUyxFQUFFLE1BQU0sQ0FBQyxTQUFTO2lCQUM1QixDQUFDLENBQUM7WUFDTCxDQUFDO1NBQ0YsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztDQUNGLENBQUMsQ0FBQSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IE5vdGljZSB9IGZyb20gJy4uLy4uL21vZGVscy9Ob3RpY2UnO1xyXG5pbXBvcnQgeyBQYWdlZExpc3REYXRhIH0gZnJvbSAnLi4vLi4vbW9kZWxzL1BhZ2VkTGlzdERhdGEnO1xyXG5cclxuUGFnZSh7XHJcbiAgZGF0YToge1xyXG4gICAgbm90aWNlczogW10gYXMgQXJyYXk8Tm90aWNlPixcclxuICAgIHBhZ2VOdW06IDEsXHJcbiAgICBwYWdlU2l6ZTogMTAsXHJcbiAgICB0b3RhbFBhZ2U6IDAsICAgIFxyXG4gIH0sXHJcbiAgb25Mb2FkKCkge1xyXG4gICAgdGhpcy5sb2FkTm90aWNlKHRoaXMsIDEsIDEwKTtcclxuICB9LFxyXG5cclxuICBsb2FkTm90aWNlKHBhZ2U6IFBhZ2UuUGFnZUluc3RhbmNlLCBwYWdlTnVtOm51bWJlciA9IDEsIHBhZ2VTaXplOm51bWJlciA9IDEwKTp2b2lkIHtcclxuICAgIHd4LnJlcXVlc3Qoe1xyXG4gICAgICB1cmw6IGBodHRwczovL3Jlc2VydmF0aW9uLndlaWhhbmxpLnh5ei9hcGkvbm90aWNlP3BhZ2VOdW09JHtwYWdlTnVtfSZwYWdlU2l6ZT0ke3BhZ2VTaXplfWAsXHJcblxyXG4gICAgICBzdWNjZXNzOiAocmVzKSA9PiB7XHJcbiAgICAgICAgY29uc29sZS5sb2cocmVzLmRhdGEpLy8g5pyN5Yqh5Zmo5Zue5YyF5L+h5oGvICAgICAgICBcclxuICAgICAgICBsZXQgcmVzdWx0ID0gcmVzLmRhdGEgYXMgUGFnZWRMaXN0RGF0YTxOb3RpY2U+O1xyXG4gICAgICAgIGlmKG51bGwgPT0gcmVzdWx0IHx8IHVuZGVmaW5lZCA9PSByZXN1bHQpe1xyXG4gICAgICAgICAgY29uc29sZS5sb2coYHJlc3VsdCBhcyBpcyBudWxsYCk7XHJcbiAgICAgICAgICByZXR1cm47XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGNvbnNvbGUubG9nKGBub3RpY2UgcmVzdWx0OiAke0pTT04uc3RyaW5naWZ5KHJlc3VsdCl9YCk7XHJcbiAgICAgICAgcGFnZS5zZXREYXRhKHtcclxuICAgICAgICAgIG5vdGljZXM6IHJlc3VsdC5EYXRhLFxyXG4gICAgICAgICAgcGFnZU51bTogcmVzdWx0LlBhZ2VOdW1iZXIsXHJcbiAgICAgICAgICBwYWdlU2l6ZTogcmVzdWx0LlBhZ2VTaXplLFxyXG4gICAgICAgICAgdG90YWxQYWdlOiByZXN1bHQuUGFnZUNvdW50XHJcbiAgICAgICAgfSk7ICAgICAgICBcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG59KVxyXG4iXX0=