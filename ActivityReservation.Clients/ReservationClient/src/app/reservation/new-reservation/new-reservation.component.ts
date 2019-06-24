import { Component, OnInit } from '@angular/core';
import { Reservation } from '../../models/Reservation';
import { ReservationService } from '../../services/ReservationService';
import { ReservationPlaceService } from 'src/app/services/ReservationPlaceService';
import { ReservationPlace } from 'src/app/models/ReservationPlace';
import {FormBuilder, FormGroup, Validators, FormArray, FormControl} from '@angular/forms';
import {MatSnackBar} from '@angular/material';
import { ReservationPeriod } from 'src/app/models/ReservationPeriod';
import { LoadingService } from '../../services/LoadingService';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-new-reservation',
  templateUrl: './new-reservation.component.html'
})
export class NewReservationComponent implements OnInit {

  public reservationPlaces: Array<ReservationPlace>;
  public reservationPeriods: Array<ReservationPeriod>;

  public minDate: Date;
  public maxDate: Date;
  
  placeFormGroup: FormGroup;
  dateFormGroup: FormGroup;
  periodFormGroup: FormGroup;
  personFormGroup: FormGroup;

  checkedPeriodsFormArray: FormArray;

  reservation: Reservation;

  submiting: boolean = false;

  constructor(private reservationSvc: ReservationService, 
    private reservationPlaceSvc: ReservationPlaceService,
    private _formBuilder: FormBuilder,
    private loadingSvc: LoadingService,
    private router: Router,
    public snackBar: MatSnackBar) {
    var now = new Date();
    this.minDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    this.maxDate = new Date(this.minDate.getTime()+ 24 *60*60*1000*7);
  }

  ngOnInit() {    
    this.placeFormGroup = this._formBuilder.group({
      placeCtrl: ['', Validators.required]
    });
    this.dateFormGroup = this._formBuilder.group({
      dateCtrl: ['', Validators.required]
    });
    this.periodFormGroup = this._formBuilder.group({
      periods: this._formBuilder.array([])
    });
    this.personFormGroup = this._formBuilder.group({
      unitCtrl: ['', Validators.required],
      contentCtrl: ['', Validators.required],
      personNameCtrl: ['', Validators.minLength(2)],
      phoneCtrl: ['', Validators.pattern(/1[3-9]\d{9}/)]
    });
    this.checkedPeriodsFormArray = this.periodFormGroup.get('periods') as FormArray;

    this.loadData();

    this.reservation = new Reservation();
  }

  private loadData(): void{
    this.reservationPlaceSvc.GetAll()
    .subscribe(data => {
      this.reservationPlaces = data;
      this.loadingSvc.isLoading = false;
    });
  }

  onStepChange(event): void{
    let stepIndex = event.selectedIndex;
    console.log(`stepIndex: ${stepIndex}, reservation:${this.reservation}`);
    //
    switch(stepIndex)
    {
      case 0:
        break;

      case 1:
        console.log(this.placeFormGroup.value.placeCtrl);
        this.reservation.ReservationPlaceId = this.placeFormGroup.value.placeCtrl;
        this.reservation.ReservationPlaceName = this.reservationPlaces.filter(p=>p.PlaceId== this.reservation.ReservationPlaceId)[0].PlaceName;
        break;

      case 2:
        let date = new Date(this.dateFormGroup.value.dateCtrl);
        let dt = `${date.getFullYear()}-${date.getMonth()<9? `0${date.getMonth()+1}` : date.getMonth()+1}-${date.getDate()}`;
        console.log(dt);
        this.reservation.ReservationForDate = dt;
        // load periods
        this.reservationPlaceSvc.getAvailablePeriods(this.reservation.ReservationPlaceId, this.reservation.ReservationForDate)
          .subscribe(x=> 
          {
            this.reservationPeriods = x;
            this.checkedPeriodsFormArray.clear();
            this.reservationPeriods.forEach(x => this.checkedPeriodsFormArray.push(new FormControl(x.Checked)));
          });       
        
        break;
      
      case 3:
        console.log(this.periodFormGroup);
        // period
        console.log(this.reservationPeriods);
        //
        let checkedPeriods = this.reservationPeriods.filter(p=>p.Checked);
        this.reservation.ReservationForTime = checkedPeriods.map(p=>p.PeriodTitle).join(","); 
        this.reservation.ReservationForTimeIds = checkedPeriods.map(p=>p.PeriodIndex).join(",");
        break;

      case 4:
        console.log(this.personFormGroup);
        break;

      default:
        break;
    }
  }

  onSubmitReservation(): void{
    this.submiting = true;
    this.reservationSvc.NewReservation(this.reservation, 'None', '')
    .subscribe(x=> {
      console.log(x);
      if(x.Status === 200 || x.Status === 'Success'){
        //
        let snackBarRef = this.snackBar.open("预约成功", "" , {
          duration: 2000,
        });
        snackBarRef.afterDismissed().subscribe(x=>{ this.router.navigateByUrl(""); });
      }else{
        alert(x.ErrorMsg);
      }
      this.submiting = false;      
    });
  }
}
