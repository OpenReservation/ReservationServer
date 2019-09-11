export class RetryHelper {
    public static tryInvoke(callback: () => any, validFunc: (res: any) => boolean, maxRetryTimes: number = 5, logErrorMinTimes: number = 1): any {
        let timer = 0;
        let isSucceed = false;
        while (!isSucceed && timer <= maxRetryTimes) {
            try {
                let result = callback();
                isSucceed = validFunc(result);
                if (!isSucceed) {
                    timer++;
                }
                return result;
            } catch (error) {
                if(timer++ >= logErrorMinTimes){
                    console.error(`retry failed, retry times: ${timer}, error: ${error}`);
                }
            }
        }
        return null;
    };

    public static async tryInvokeAsync(callback: () => Promise<any>, validFunc: (res: any) => boolean, maxRetryTimes: number = 5, logErrorMinTimes: number = 1) {
        let timer = 0;
        let isSucceed = false;
        while (!isSucceed && timer <= maxRetryTimes) {
            try {
                let result = await callback();
                isSucceed = validFunc(result);
                if (!isSucceed) {
                    timer++;
                }
                return result;
            } catch (error) {
                if(timer++ >= logErrorMinTimes){
                    console.error(`retry failed, retry times: ${timer}, error: ${error}`);
                }
            }
        }
        return null;
    };
}