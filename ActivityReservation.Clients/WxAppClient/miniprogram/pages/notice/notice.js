"use strict";
Page({
    data: {
        logs: []
    },
    onLoad: function () {
        this.loadNotice();
    },
    loadNotice: function (pageNum, pageSize) {
        if (pageNum === void 0) { pageNum = 1; }
        if (pageSize === void 0) { pageSize = 10; }
        wx.request({
            url: "https://reservation.weihanli.xyz/api/notice?pageNum=" + pageNum + "&pageSize=" + pageSize,
            success: function (res) {
                console.log(res);
            }
        });
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibm90aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibm90aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7QUFBQSxJQUFJLENBQUM7SUFDSCxJQUFJLEVBQUU7UUFDSixJQUFJLEVBQUUsRUFBYztLQUNyQjtJQUNELE1BQU07UUFDSixJQUFJLENBQUMsVUFBVSxFQUFFLENBQUM7SUFDcEIsQ0FBQztJQUVELFVBQVUsRUFBVixVQUFXLE9BQWtCLEVBQUUsUUFBb0I7UUFBeEMsd0JBQUEsRUFBQSxXQUFrQjtRQUFFLHlCQUFBLEVBQUEsYUFBb0I7UUFDakQsRUFBRSxDQUFDLE9BQU8sQ0FBQztZQUNULEdBQUcsRUFBRSx5REFBdUQsT0FBTyxrQkFBYSxRQUFVO1lBRTFGLE9BQU8sRUFBRSxVQUFVLEdBQUc7Z0JBQ3BCLE9BQU8sQ0FBQyxHQUFHLENBQUMsR0FBRyxDQUFDLENBQUE7WUFDbEIsQ0FBQztTQUNGLENBQUMsQ0FBQTtJQUNKLENBQUM7Q0FDRixDQUFDLENBQUEiLCJzb3VyY2VzQ29udGVudCI6WyJQYWdlKHtcclxuICBkYXRhOiB7XHJcbiAgICBsb2dzOiBbXSBhcyBzdHJpbmdbXVxyXG4gIH0sXHJcbiAgb25Mb2FkKCkge1xyXG4gICAgdGhpcy5sb2FkTm90aWNlKCk7XHJcbiAgfSxcclxuXHJcbiAgbG9hZE5vdGljZShwYWdlTnVtOm51bWJlciA9IDEsIHBhZ2VTaXplOm51bWJlciA9IDEwKTp2b2lkIHtcclxuICAgIHd4LnJlcXVlc3Qoe1xyXG4gICAgICB1cmw6IGBodHRwczovL3Jlc2VydmF0aW9uLndlaWhhbmxpLnh5ei9hcGkvbm90aWNlP3BhZ2VOdW09JHtwYWdlTnVtfSZwYWdlU2l6ZT0ke3BhZ2VTaXplfWAsXHJcblxyXG4gICAgICBzdWNjZXNzOiBmdW5jdGlvbiAocmVzKSB7XHJcbiAgICAgICAgY29uc29sZS5sb2cocmVzKS8vIOacjeWKoeWZqOWbnuWMheS/oeaBr1xyXG4gICAgICB9XHJcbiAgICB9KVxyXG4gIH1cclxufSlcclxuIl19