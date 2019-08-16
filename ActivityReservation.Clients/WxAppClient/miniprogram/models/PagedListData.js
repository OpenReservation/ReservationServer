"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var PagedListModel = (function () {
    function PagedListModel() {
        this._PageSize = 10;
    }
    Object.defineProperty(PagedListModel.prototype, "PageSize", {
        get: function () {
            return this._PageSize;
        },
        set: function (v) {
            this._PageSize = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagedListModel.prototype, "PageNumber", {
        get: function () {
            return this._PageNumber;
        },
        set: function (v) {
            this._PageNumber = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagedListModel.prototype, "Count", {
        get: function () {
            return this._Count;
        },
        set: function (v) {
            this._Count = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagedListModel.prototype, "PageCount", {
        get: function () {
            return this._PageCount;
        },
        set: function (v) {
            this._PageCount = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagedListModel.prototype, "TotalCount", {
        get: function () {
            return this._TotalCount;
        },
        set: function (v) {
            this._TotalCount = v;
        },
        enumerable: true,
        configurable: true
    });
    return PagedListModel;
}());
exports.PagedListModel = PagedListModel;
var PagedListData = (function (_super) {
    __extends(PagedListData, _super);
    function PagedListData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Object.defineProperty(PagedListData.prototype, "Data", {
        get: function () {
            return this.data;
        },
        set: function (v) {
            this.data = v;
        },
        enumerable: true,
        configurable: true
    });
    return PagedListData;
}(PagedListModel));
exports.PagedListData = PagedListData;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUGFnZWRMaXN0RGF0YS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIlBhZ2VkTGlzdERhdGEudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7Ozs7O0FBQUE7SUFBQTtRQUVZLGNBQVMsR0FBWSxFQUFFLENBQUM7SUF1Q3BDLENBQUM7SUF0Q0csc0JBQVcsb0NBQVE7YUFBbkI7WUFDSSxPQUFPLElBQUksQ0FBQyxTQUFTLENBQUM7UUFDMUIsQ0FBQzthQUNELFVBQW9CLENBQVU7WUFDMUIsSUFBSSxDQUFDLFNBQVMsR0FBRyxDQUFDLENBQUM7UUFDdkIsQ0FBQzs7O09BSEE7SUFNRCxzQkFBVyxzQ0FBVTthQUFyQjtZQUNJLE9BQU8sSUFBSSxDQUFDLFdBQVcsQ0FBQztRQUM1QixDQUFDO2FBQ0QsVUFBc0IsQ0FBVTtZQUM1QixJQUFJLENBQUMsV0FBVyxHQUFHLENBQUMsQ0FBQztRQUN6QixDQUFDOzs7T0FIQTtJQU1ELHNCQUFXLGlDQUFLO2FBQWhCO1lBQ0ksT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDO1FBQ3ZCLENBQUM7YUFDRCxVQUFpQixDQUFVO1lBQ3ZCLElBQUksQ0FBQyxNQUFNLEdBQUcsQ0FBQyxDQUFDO1FBQ3BCLENBQUM7OztPQUhBO0lBTUQsc0JBQVcscUNBQVM7YUFBcEI7WUFDSSxPQUFPLElBQUksQ0FBQyxVQUFVLENBQUM7UUFDM0IsQ0FBQzthQUNELFVBQXFCLENBQVU7WUFDM0IsSUFBSSxDQUFDLFVBQVUsR0FBRyxDQUFDLENBQUM7UUFDeEIsQ0FBQzs7O09BSEE7SUFNRCxzQkFBVyxzQ0FBVTthQUFyQjtZQUNJLE9BQU8sSUFBSSxDQUFDLFdBQVcsQ0FBQztRQUM1QixDQUFDO2FBQ0QsVUFBc0IsQ0FBVTtZQUM1QixJQUFJLENBQUMsV0FBVyxHQUFHLENBQUMsQ0FBQztRQUN6QixDQUFDOzs7T0FIQTtJQUlMLHFCQUFDO0FBQUQsQ0FBQyxBQXpDRCxJQXlDQztBQXpDWSx3Q0FBYztBQTJDM0I7SUFBc0MsaUNBQWM7SUFBcEQ7O0lBU0EsQ0FBQztJQU5DLHNCQUFXLCtCQUFJO2FBQWY7WUFDRSxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUM7UUFDbkIsQ0FBQzthQUNELFVBQWdCLENBQVk7WUFDMUIsSUFBSSxDQUFDLElBQUksR0FBRyxDQUFDLENBQUM7UUFDaEIsQ0FBQzs7O09BSEE7SUFJSCxvQkFBQztBQUFELENBQUMsQUFURCxDQUFzQyxjQUFjLEdBU25EO0FBVFksc0NBQWEiLCJzb3VyY2VzQ29udGVudCI6WyJleHBvcnQgY2xhc3MgUGFnZWRMaXN0TW9kZWx7XHJcblxyXG4gICAgcHJpdmF0ZSBfUGFnZVNpemUgOiBudW1iZXIgPSAxMDtcclxuICAgIHB1YmxpYyBnZXQgUGFnZVNpemUoKSA6IG51bWJlciB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMuX1BhZ2VTaXplO1xyXG4gICAgfVxyXG4gICAgcHVibGljIHNldCBQYWdlU2l6ZSh2IDogbnVtYmVyKSB7XHJcbiAgICAgICAgdGhpcy5fUGFnZVNpemUgPSB2O1xyXG4gICAgfVxyXG5cclxuICAgIHByaXZhdGUgX1BhZ2VOdW1iZXIgOiBudW1iZXI7XHJcbiAgICBwdWJsaWMgZ2V0IFBhZ2VOdW1iZXIoKSA6IG51bWJlciB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMuX1BhZ2VOdW1iZXI7XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IFBhZ2VOdW1iZXIodiA6IG51bWJlcikge1xyXG4gICAgICAgIHRoaXMuX1BhZ2VOdW1iZXIgPSB2O1xyXG4gICAgfVxyXG5cclxuICAgIHByaXZhdGUgX0NvdW50IDogbnVtYmVyO1xyXG4gICAgcHVibGljIGdldCBDb3VudCgpIDogbnVtYmVyIHtcclxuICAgICAgICByZXR1cm4gdGhpcy5fQ291bnQ7XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IENvdW50KHYgOiBudW1iZXIpIHtcclxuICAgICAgICB0aGlzLl9Db3VudCA9IHY7XHJcbiAgICB9XHJcblxyXG4gICAgcHJpdmF0ZSBfUGFnZUNvdW50IDogbnVtYmVyO1xyXG4gICAgcHVibGljIGdldCBQYWdlQ291bnQoKSA6IG51bWJlciB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMuX1BhZ2VDb3VudDtcclxuICAgIH1cclxuICAgIHB1YmxpYyBzZXQgUGFnZUNvdW50KHYgOiBudW1iZXIpIHtcclxuICAgICAgICB0aGlzLl9QYWdlQ291bnQgPSB2O1xyXG4gICAgfVxyXG5cclxuICAgIHByaXZhdGUgX1RvdGFsQ291bnQgOiBudW1iZXI7XHJcbiAgICBwdWJsaWMgZ2V0IFRvdGFsQ291bnQoKSA6IG51bWJlciB7XHJcbiAgICAgICAgcmV0dXJuIHRoaXMuX1RvdGFsQ291bnQ7XHJcbiAgICB9XHJcbiAgICBwdWJsaWMgc2V0IFRvdGFsQ291bnQodiA6IG51bWJlcikge1xyXG4gICAgICAgIHRoaXMuX1RvdGFsQ291bnQgPSB2O1xyXG4gICAgfVxyXG59XHJcblxyXG5leHBvcnQgY2xhc3MgUGFnZWRMaXN0RGF0YTxUPiBleHRlbmRzIFBhZ2VkTGlzdE1vZGVse1xyXG5cclxuICBwcml2YXRlIGRhdGEgOiBBcnJheTxUPjtcclxuICBwdWJsaWMgZ2V0IERhdGEoKSA6IEFycmF5PFQ+IHtcclxuICAgIHJldHVybiB0aGlzLmRhdGE7XHJcbiAgfVxyXG4gIHB1YmxpYyBzZXQgRGF0YSh2IDogQXJyYXk8VD4pIHtcclxuICAgIHRoaXMuZGF0YSA9IHY7XHJcbiAgfVxyXG59XHJcbiJdfQ==