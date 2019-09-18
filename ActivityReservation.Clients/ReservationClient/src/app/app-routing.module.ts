import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ReservationListComponent } from './reservation/reservation-list/reservation-list.component';
import { NoticeListComponent } from './notice/notice-list/notice-list.component';
import { NoticeDetailComponent } from './notice/notice-detail/notice-detail.component';
import { AboutComponent } from './about/about.component';
import { NewReservationComponent } from './reservation/new-reservation/new-reservation.component';

const routes: Routes = [
  { path: '', component: ReservationListComponent },
  { path: 'reservation/new', component:NewReservationComponent },
  { path: 'reservation', component: ReservationListComponent },
  { path: 'notice', component: NoticeListComponent },
  { path: 'notice/:noticePath', component: NoticeDetailComponent },
  { path: 'about', component: AboutComponent },
  { path: '**', redirectTo: '/'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
