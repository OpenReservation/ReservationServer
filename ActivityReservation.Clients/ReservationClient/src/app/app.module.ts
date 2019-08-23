import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppMaterialModule } from './app.material.module';
import { HttpClientModule } from '@angular/common/http';
import { ReservationListComponent } from './reservation/reservation-list/reservation-list.component';
import { NoticeListComponent } from './notice/notice-list/notice-list.component';
import { NoticeDetailComponent } from './notice/notice-detail/notice-detail.component';
import { AboutComponent } from './about/about.component';
import { SanitizeHtmlPipe } from './shared/pipes/safe-html';
import { SentryErrorHandler } from './shared/sentryErrorHandler';
import { FundebugErrorHandler } from './shared/fundebugErrorHandler';
import { NewReservationComponent } from './reservation/new-reservation/new-reservation.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NewReservationComponent,
    ReservationListComponent,
    NoticeListComponent,
    NoticeDetailComponent,
    AboutComponent,
    SanitizeHtmlPipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppMaterialModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
  ],
  providers: [
    { provide: ErrorHandler, useClass: SentryErrorHandler },
    { provide: ErrorHandler, useClass: FundebugErrorHandler }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
