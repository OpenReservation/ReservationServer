"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
Page({
    data: {
        notices: [],
        pageNum: 1,
        pageSize: 10,
        totalPage: 0,
        totalCount: 0
    },
    onLoad: function () {
        this.loadNotice(1, 10);
    },
    loadNext: function () {
        this.data.pageNum++;
        this.loadNotice(this.data.pageNum, this.data.pageSize);
    },
    loadNotice: function (pageNum, pageSize) {
        if (pageNum === void 0) { pageNum = 1; }
        if (pageSize === void 0) { pageSize = 10; }
        var _this = this;
        wx.showLoading({
            title: "loading..."
        });
        wx.request({
            url: "https://reservation.weihanli.xyz/api/notice?pageNum=" + pageNum + "&pageSize=" + pageSize,
            success: function (res) {
                wx.hideLoading();
                console.log(res.data);
                var result = res.data;
                console.log("result:" + JSON.stringify(result));
                _this.setData({
                    notices: result.Data,
                    pageNum: result.PageNumber,
                    pageSize: result.PageSize,
                    totalPage: result.PageCount,
                    totalCount: result.TotalCount
                });
            }
        });
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibm90aWNlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsibm90aWNlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O0FBSUEsSUFBSSxDQUFDO0lBQ0gsSUFBSSxFQUFFO1FBQ0osT0FBTyxFQUFFLEVBQW1CO1FBQzVCLE9BQU8sRUFBRSxDQUFDO1FBQ1YsUUFBUSxFQUFFLEVBQUU7UUFDWixTQUFTLEVBQUUsQ0FBQztRQUNaLFVBQVUsRUFBRSxDQUFDO0tBQ2Q7SUFDRCxNQUFNO1FBQ0osSUFBSSxDQUFDLFVBQVUsQ0FBQyxDQUFDLEVBQUUsRUFBRSxDQUFDLENBQUM7SUFDekIsQ0FBQztJQUVELFFBQVE7UUFDTixJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sRUFBRSxDQUFDO1FBQ3BCLElBQUksQ0FBQyxVQUFVLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLEVBQUUsSUFBSSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQztJQUN6RCxDQUFDO0lBRUQsVUFBVSxFQUFWLFVBQVcsT0FBa0IsRUFBRSxRQUFvQjtRQUF4Qyx3QkFBQSxFQUFBLFdBQWtCO1FBQUUseUJBQUEsRUFBQSxhQUFvQjtRQUNqRCxJQUFJLEtBQUssR0FBRyxJQUFJLENBQUM7UUFDakIsRUFBRSxDQUFDLFdBQVcsQ0FBQztZQUNiLEtBQUssRUFBRSxZQUFZO1NBQ3BCLENBQUMsQ0FBQztRQUNILEVBQUUsQ0FBQyxPQUFPLENBQUM7WUFDVCxHQUFHLEVBQUUseURBQXVELE9BQU8sa0JBQWEsUUFBVTtZQUMxRixPQUFPLEVBQUUsVUFBQyxHQUFHO2dCQUNYLEVBQUUsQ0FBQyxXQUFXLEVBQUUsQ0FBQztnQkFDakIsT0FBTyxDQUFDLEdBQUcsQ0FBQyxHQUFHLENBQUMsSUFBSSxDQUFDLENBQUE7Z0JBQ3JCLElBQUksTUFBTSxHQUEwQixHQUFHLENBQUMsSUFBSSxDQUFDO2dCQUM3QyxPQUFPLENBQUMsR0FBRyxDQUFDLFlBQVUsSUFBSSxDQUFDLFNBQVMsQ0FBQyxNQUFNLENBQUcsQ0FBQyxDQUFDO2dCQUMxQyxLQUFNLENBQUMsT0FBTyxDQUFDO29CQUNuQixPQUFPLEVBQUUsTUFBTSxDQUFDLElBQUk7b0JBQ3BCLE9BQU8sRUFBRSxNQUFNLENBQUMsVUFBVTtvQkFDMUIsUUFBUSxFQUFFLE1BQU0sQ0FBQyxRQUFRO29CQUN6QixTQUFTLEVBQUUsTUFBTSxDQUFDLFNBQVM7b0JBQzNCLFVBQVUsRUFBRSxNQUFNLENBQUMsVUFBVTtpQkFDOUIsQ0FBQyxDQUFDO1lBQ0wsQ0FBQztTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7Q0FFRixDQUFDLENBQUEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBOb3RpY2UgfSBmcm9tICcuLi8uLi9tb2RlbHMvTm90aWNlJztcclxuaW1wb3J0IHsgUGFnZWRMaXN0RGF0YSB9IGZyb20gJy4uLy4uL21vZGVscy9QYWdlZExpc3REYXRhJztcclxuaW1wb3J0IHsgZm9ybWF0VGltZSB9IGZyb20gJy4uLy4uL3V0aWxzL3V0aWwnO1xyXG5cclxuUGFnZSh7XHJcbiAgZGF0YToge1xyXG4gICAgbm90aWNlczogW10gYXMgQXJyYXk8Tm90aWNlPixcclxuICAgIHBhZ2VOdW06IDEsXHJcbiAgICBwYWdlU2l6ZTogMTAsXHJcbiAgICB0b3RhbFBhZ2U6IDAsXHJcbiAgICB0b3RhbENvdW50OiAwICAgIFxyXG4gIH0sXHJcbiAgb25Mb2FkKCkge1xyXG4gICAgdGhpcy5sb2FkTm90aWNlKDEsIDEwKTtcclxuICB9LFxyXG5cclxuICBsb2FkTmV4dCgpe1xyXG4gICAgdGhpcy5kYXRhLnBhZ2VOdW0rKztcclxuICAgIHRoaXMubG9hZE5vdGljZSh0aGlzLmRhdGEucGFnZU51bSwgdGhpcy5kYXRhLnBhZ2VTaXplKTtcclxuICB9LFxyXG5cclxuICBsb2FkTm90aWNlKHBhZ2VOdW06bnVtYmVyID0gMSwgcGFnZVNpemU6bnVtYmVyID0gMTApIHtcclxuICAgIGxldCBfdGhpcyA9IHRoaXM7XHJcbiAgICB3eC5zaG93TG9hZGluZyh7XHJcbiAgICAgIHRpdGxlOiBcImxvYWRpbmcuLi5cIlxyXG4gICAgfSk7XHJcbiAgICB3eC5yZXF1ZXN0KHtcclxuICAgICAgdXJsOiBgaHR0cHM6Ly9yZXNlcnZhdGlvbi53ZWloYW5saS54eXovYXBpL25vdGljZT9wYWdlTnVtPSR7cGFnZU51bX0mcGFnZVNpemU9JHtwYWdlU2l6ZX1gLFxyXG4gICAgICBzdWNjZXNzOiAocmVzKSA9PiB7XHJcbiAgICAgICAgd3guaGlkZUxvYWRpbmcoKTtcclxuICAgICAgICBjb25zb2xlLmxvZyhyZXMuZGF0YSkvLyDmnI3liqHlmajlm57ljIXkv6Hmga8gXHJcbiAgICAgICAgbGV0IHJlc3VsdCA9IDxQYWdlZExpc3REYXRhPE5vdGljZT4+cmVzLmRhdGE7XHJcbiAgICAgICAgY29uc29sZS5sb2coYHJlc3VsdDoke0pTT04uc3RyaW5naWZ5KHJlc3VsdCl9YCk7XHJcbiAgICAgICAgKDxhbnk+X3RoaXMpLnNldERhdGEoe1xyXG4gICAgICAgICAgbm90aWNlczogcmVzdWx0LkRhdGEsXHJcbiAgICAgICAgICBwYWdlTnVtOiByZXN1bHQuUGFnZU51bWJlcixcclxuICAgICAgICAgIHBhZ2VTaXplOiByZXN1bHQuUGFnZVNpemUsXHJcbiAgICAgICAgICB0b3RhbFBhZ2U6IHJlc3VsdC5QYWdlQ291bnQsXHJcbiAgICAgICAgICB0b3RhbENvdW50OiByZXN1bHQuVG90YWxDb3VudFxyXG4gICAgICAgIH0pOyBcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxufSlcclxuIl19