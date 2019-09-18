import { ErrorHandler } from '@angular/core';
import * as fundebug from 'fundebug-javascript';
fundebug.apikey = '82d577568365c3b0d1649fe50a3727acbba3b19cac700d7735754300ebd17340';

// 定义FundebugErrorHandler
export class FundebugErrorHandler implements ErrorHandler {
  handleError(err: any): void {
    fundebug.notifyError(err);
    }
}
