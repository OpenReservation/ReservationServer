"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var RetryHelper = (function () {
    function RetryHelper() {
    }
    RetryHelper.tryInvoke = function (callback, validFunc, maxRetryTimes, logErrorMinTimes) {
        if (maxRetryTimes === void 0) { maxRetryTimes = 5; }
        if (logErrorMinTimes === void 0) { logErrorMinTimes = 1; }
        var timer = 0;
        var isSucceed = false;
        while (!isSucceed && timer <= maxRetryTimes) {
            try {
                var result = callback();
                isSucceed = validFunc(result);
                if (!isSucceed) {
                    timer++;
                }
                return result;
            }
            catch (error) {
                if (timer++ >= logErrorMinTimes) {
                    console.error("retry failed, retry times: " + timer + ", error: " + error);
                }
            }
        }
        return null;
    };
    ;
    RetryHelper.tryInvokeAsync = function (callback, validFunc, maxRetryTimes, logErrorMinTimes) {
        if (maxRetryTimes === void 0) { maxRetryTimes = 5; }
        if (logErrorMinTimes === void 0) { logErrorMinTimes = 1; }
        return __awaiter(this, void 0, void 0, function () {
            var timer, isSucceed, result, error_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        timer = 0;
                        isSucceed = false;
                        _a.label = 1;
                    case 1:
                        if (!(!isSucceed && timer <= maxRetryTimes)) return [3, 6];
                        _a.label = 2;
                    case 2:
                        _a.trys.push([2, 4, , 5]);
                        return [4, callback()];
                    case 3:
                        result = _a.sent();
                        isSucceed = validFunc(result);
                        if (!isSucceed) {
                            timer++;
                        }
                        return [2, result];
                    case 4:
                        error_1 = _a.sent();
                        if (timer++ >= logErrorMinTimes) {
                            console.error("retry failed, retry times: " + timer + ", error: " + error_1);
                        }
                        return [3, 5];
                    case 5: return [3, 1];
                    case 6: return [2, null];
                }
            });
        });
    };
    ;
    return RetryHelper;
}());
exports.RetryHelper = RetryHelper;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiUmV0cnlIZWxwZXIuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJSZXRyeUhlbHBlci50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7O0FBQUE7SUFBQTtJQXdDQSxDQUFDO0lBdkNpQixxQkFBUyxHQUF2QixVQUF3QixRQUFtQixFQUFFLFNBQWdDLEVBQUUsYUFBeUIsRUFBRSxnQkFBNEI7UUFBdkQsOEJBQUEsRUFBQSxpQkFBeUI7UUFBRSxpQ0FBQSxFQUFBLG9CQUE0QjtRQUNsSSxJQUFJLEtBQUssR0FBRyxDQUFDLENBQUM7UUFDZCxJQUFJLFNBQVMsR0FBRyxLQUFLLENBQUM7UUFDdEIsT0FBTyxDQUFDLFNBQVMsSUFBSSxLQUFLLElBQUksYUFBYSxFQUFFO1lBQ3pDLElBQUk7Z0JBQ0EsSUFBSSxNQUFNLEdBQUcsUUFBUSxFQUFFLENBQUM7Z0JBQ3hCLFNBQVMsR0FBRyxTQUFTLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQzlCLElBQUksQ0FBQyxTQUFTLEVBQUU7b0JBQ1osS0FBSyxFQUFFLENBQUM7aUJBQ1g7Z0JBQ0QsT0FBTyxNQUFNLENBQUM7YUFDakI7WUFBQyxPQUFPLEtBQUssRUFBRTtnQkFDWixJQUFHLEtBQUssRUFBRSxJQUFJLGdCQUFnQixFQUFDO29CQUMzQixPQUFPLENBQUMsS0FBSyxDQUFDLGdDQUE4QixLQUFLLGlCQUFZLEtBQU8sQ0FBQyxDQUFDO2lCQUN6RTthQUNKO1NBQ0o7UUFDRCxPQUFPLElBQUksQ0FBQztJQUNoQixDQUFDO0lBQUEsQ0FBQztJQUVrQiwwQkFBYyxHQUFsQyxVQUFtQyxRQUE0QixFQUFFLFNBQWdDLEVBQUUsYUFBeUIsRUFBRSxnQkFBNEI7UUFBdkQsOEJBQUEsRUFBQSxpQkFBeUI7UUFBRSxpQ0FBQSxFQUFBLG9CQUE0Qjs7Ozs7O3dCQUNsSixLQUFLLEdBQUcsQ0FBQyxDQUFDO3dCQUNWLFNBQVMsR0FBRyxLQUFLLENBQUM7Ozs2QkFDZixDQUFBLENBQUMsU0FBUyxJQUFJLEtBQUssSUFBSSxhQUFhLENBQUE7Ozs7d0JBRXRCLFdBQU0sUUFBUSxFQUFFLEVBQUE7O3dCQUF6QixNQUFNLEdBQUcsU0FBZ0I7d0JBQzdCLFNBQVMsR0FBRyxTQUFTLENBQUMsTUFBTSxDQUFDLENBQUM7d0JBQzlCLElBQUksQ0FBQyxTQUFTLEVBQUU7NEJBQ1osS0FBSyxFQUFFLENBQUM7eUJBQ1g7d0JBQ0QsV0FBTyxNQUFNLEVBQUM7Ozt3QkFFZCxJQUFHLEtBQUssRUFBRSxJQUFJLGdCQUFnQixFQUFDOzRCQUMzQixPQUFPLENBQUMsS0FBSyxDQUFDLGdDQUE4QixLQUFLLGlCQUFZLE9BQU8sQ0FBQyxDQUFDO3lCQUN6RTs7OzRCQUdULFdBQU8sSUFBSSxFQUFDOzs7O0tBQ2Y7SUFBQSxDQUFDO0lBQ04sa0JBQUM7QUFBRCxDQUFDLEFBeENELElBd0NDO0FBeENZLGtDQUFXIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IGNsYXNzIFJldHJ5SGVscGVyIHtcclxuICAgIHB1YmxpYyBzdGF0aWMgdHJ5SW52b2tlKGNhbGxiYWNrOiAoKSA9PiBhbnksIHZhbGlkRnVuYzogKHJlczogYW55KSA9PiBib29sZWFuLCBtYXhSZXRyeVRpbWVzOiBudW1iZXIgPSA1LCBsb2dFcnJvck1pblRpbWVzOiBudW1iZXIgPSAxKTogYW55IHtcclxuICAgICAgICBsZXQgdGltZXIgPSAwO1xyXG4gICAgICAgIGxldCBpc1N1Y2NlZWQgPSBmYWxzZTtcclxuICAgICAgICB3aGlsZSAoIWlzU3VjY2VlZCAmJiB0aW1lciA8PSBtYXhSZXRyeVRpbWVzKSB7XHJcbiAgICAgICAgICAgIHRyeSB7XHJcbiAgICAgICAgICAgICAgICBsZXQgcmVzdWx0ID0gY2FsbGJhY2soKTtcclxuICAgICAgICAgICAgICAgIGlzU3VjY2VlZCA9IHZhbGlkRnVuYyhyZXN1bHQpO1xyXG4gICAgICAgICAgICAgICAgaWYgKCFpc1N1Y2NlZWQpIHtcclxuICAgICAgICAgICAgICAgICAgICB0aW1lcisrO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuIHJlc3VsdDtcclxuICAgICAgICAgICAgfSBjYXRjaCAoZXJyb3IpIHtcclxuICAgICAgICAgICAgICAgIGlmKHRpbWVyKysgPj0gbG9nRXJyb3JNaW5UaW1lcyl7XHJcbiAgICAgICAgICAgICAgICAgICAgY29uc29sZS5lcnJvcihgcmV0cnkgZmFpbGVkLCByZXRyeSB0aW1lczogJHt0aW1lcn0sIGVycm9yOiAke2Vycm9yfWApO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHJldHVybiBudWxsO1xyXG4gICAgfTtcclxuXHJcbiAgICBwdWJsaWMgc3RhdGljIGFzeW5jIHRyeUludm9rZUFzeW5jKGNhbGxiYWNrOiAoKSA9PiBQcm9taXNlPGFueT4sIHZhbGlkRnVuYzogKHJlczogYW55KSA9PiBib29sZWFuLCBtYXhSZXRyeVRpbWVzOiBudW1iZXIgPSA1LCBsb2dFcnJvck1pblRpbWVzOiBudW1iZXIgPSAxKSB7XHJcbiAgICAgICAgbGV0IHRpbWVyID0gMDtcclxuICAgICAgICBsZXQgaXNTdWNjZWVkID0gZmFsc2U7XHJcbiAgICAgICAgd2hpbGUgKCFpc1N1Y2NlZWQgJiYgdGltZXIgPD0gbWF4UmV0cnlUaW1lcykge1xyXG4gICAgICAgICAgICB0cnkge1xyXG4gICAgICAgICAgICAgICAgbGV0IHJlc3VsdCA9IGF3YWl0IGNhbGxiYWNrKCk7XHJcbiAgICAgICAgICAgICAgICBpc1N1Y2NlZWQgPSB2YWxpZEZ1bmMocmVzdWx0KTtcclxuICAgICAgICAgICAgICAgIGlmICghaXNTdWNjZWVkKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgdGltZXIrKztcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIHJldHVybiByZXN1bHQ7XHJcbiAgICAgICAgICAgIH0gY2F0Y2ggKGVycm9yKSB7XHJcbiAgICAgICAgICAgICAgICBpZih0aW1lcisrID49IGxvZ0Vycm9yTWluVGltZXMpe1xyXG4gICAgICAgICAgICAgICAgICAgIGNvbnNvbGUuZXJyb3IoYHJldHJ5IGZhaWxlZCwgcmV0cnkgdGltZXM6ICR7dGltZXJ9LCBlcnJvcjogJHtlcnJvcn1gKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gbnVsbDtcclxuICAgIH07XHJcbn0iXX0=