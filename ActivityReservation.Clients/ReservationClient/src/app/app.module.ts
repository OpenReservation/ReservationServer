import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppMaterialModule } from './app.material.module';
import { HttpClientModule } from '@angular/common/http';
import { ReservationListComponent } from './reservation/reservation-list/reservation-list.component';
import { NoticeListComponent } from './notice/notice-list/notice-list.component';

@NgModule({
  declarations: [
    AppComponent,
    ReservationListComponent,
    NoticeListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppMaterialModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
