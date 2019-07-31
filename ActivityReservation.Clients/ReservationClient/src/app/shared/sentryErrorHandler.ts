import { ErrorHandler, Injectable } from '@angular/core';
import * as Sentry from '@sentry/browser';

Sentry.init({
  dsn: 'https://a48b9eb9afa448e7894e41d23274a5fe@sentry.io/1517949'
});

@Injectable()
export class SentryErrorHandler implements ErrorHandler {
  constructor() {}
  handleError(error) {
    const eventId = Sentry.captureException(error.originalError || error);
    Sentry.showReportDialog({ eventId });
  }
}
