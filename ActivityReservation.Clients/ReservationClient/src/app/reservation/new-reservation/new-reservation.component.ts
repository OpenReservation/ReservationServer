import { Component, OnInit, OnDestroy } from '@angular/core';
import { Reservation } from '../../models/Reservation';
import { ReservationService } from '../../services/ReservationService';
import { ReservationPlaceService } from 'src/app/services/ReservationPlaceService';
import { ReservationPlace } from 'src/app/models/ReservationPlace';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReservationPeriod } from 'src/app/models/ReservationPeriod';
import { LoadingService } from '../../services/LoadingService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-reservation',
  templateUrl: './new-reservation.component.html'
})
export class NewReservationComponent implements OnInit, OnDestroy {

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

  maxReservationPeriod: number = 2;
  checkedPeriodNum: number = 0;

  submiting: boolean = false;
  captchaValid: boolean = false;
  captchaInfo = {
    nonce: '',
    ticket: ''
  };

  tencentRecaptcha:TencentCaptcha = null;


  constructor(private reservationSvc: ReservationService,
    private reservationPlaceSvc: ReservationPlaceService,
    private _formBuilder: FormBuilder,
    private loadingSvc: LoadingService,
    private router: Router,
    public snackBar: MatSnackBar) {
    var now = new Date();
    this.minDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    this.maxDate = new Date(this.minDate.getTime()+ 24*60*60*1000*7);
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
      phoneCtrl: ['', Validators.pattern(/^1[3-9]\d{9}$/)]
    });
    this.checkedPeriodsFormArray = this.periodFormGroup.get('periods') as FormArray;

    this.loadData();

    this.reservation = new Reservation();
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.tencentRecaptcha = null;
  }

  private loadCaptcha(): void {
    var tCaptcha = document.getElementById("tCaptcha");
    if(tCaptcha){
      this.InitCaptcha();
      return;
    }
    let script = <any>document.createElement('script');
    script.id = "tCaptcha";
    script.type = 'text/javascript';
    script.src = "https://ssl.captcha.qq.com/TCaptcha.js"
    if (script.readyState) {  //IE
        script.onreadystatechange = () => {
            if (script.readyState === "loaded" || script.readyState === "complete") {
              this.InitCaptcha();
            }
        };
    } else {  //Others
        script.onload = () => {
            this.InitCaptcha();
        };
    }
    document.getElementsByTagName('body')[0].appendChild(script);
  }

  private InitCaptcha():void{
    let captchaDom = document.getElementById('TencentCaptcha1');
    if(!captchaDom){
      return;
    }
    this.tencentRecaptcha = new TencentCaptcha(
      captchaDom, '2062135016', (res) => {
          this.captchaValid = false;
          console.log(res);
          // res（用户主动关闭验证码）= {ret: 2, ticket: null}
          // res（验证成功） = {ret: 0, ticket: "String", randstr: "String"}
          if (res.ret === 0) {
              this.captchaInfo.nonce = res.randstr;
              this.captchaInfo.ticket = res.ticket;
              this.captchaValid = true;
              this.tencentRecaptcha.destroy();

              let button = <HTMLElement>document.getElementById("btnSubmit");
              button.click();
          }
      }
    );
    console.log(`captcha inited`);
    this.tencentRecaptcha.show();
  }

  private loadData(): void{
    if(this.loadingSvc.isLoading === false){
      this.loadingSvc.isLoading = true;
    }
    this.reservationPlaceSvc.GetAll()
    .subscribe(data => {
      this.reservationPlaces = data;
      this.loadingSvc.isLoading = false;
    });
  }

  onPeriodClick(pIndex: number, period: ReservationPeriod):void{
    console.group(`onPeriodClick ${pIndex}`);
    console.log(`click period ${pIndex}, ${period.PeriodTitle}, checked:${period.Checked}`);
    if(period.Checked === true){
      period.Checked = false;
      this.checkedPeriodNum = this.checkedPeriodNum-1;
      console.log(`uncheck ${period.PeriodTitle}, checked:${period.Checked}, checkedPeriodNum:${this.checkedPeriodNum}`);
    }else{
      period.Checked = true;
      this.checkedPeriodNum = this.checkedPeriodNum+1;
      console.log(`check ${period.PeriodTitle}, checked:${period.Checked}, checkedPeriodNum:${this.checkedPeriodNum}`);
    }
    console.groupEnd();
  }

  onStepChange(event): void{
    console.log(event);

    let stepIndex = event.selectedIndex;
    console.log(`stepIndex: ${stepIndex}, reservation:${JSON.stringify(this.reservation)}`);
    //
    switch(stepIndex)
    {
      case 0:
        break;

      case 1:
        console.log(this.placeFormGroup.value.placeCtrl);
        this.reservation.ReservationPlaceId = this.placeFormGroup.value.placeCtrl;
        let checkedPlace = this.reservationPlaces.filter(p=>p.PlaceId == this.reservation.ReservationPlaceId)[0];
        this.reservation.ReservationPlaceName = checkedPlace.PlaceName;
        if(checkedPlace.MaxReservationPeriodNum > 0){
          this.maxReservationPeriod = checkedPlace.MaxReservationPeriodNum;
        }
        this.checkedPeriodNum = 0;
        console.log(`checked place:${JSON.stringify(checkedPlace)}`);
        break;

      case 2:
        let date = new Date(this.dateFormGroup.value.dateCtrl);
        let dt = `${date.getFullYear()}-${date.getMonth()<9? `0${date.getMonth()+1}` : date.getMonth()+1}-${date.getDate()<=9? `0${date.getDate()}` : date.getDate()}`;
        console.log(dt);
        if(this.reservation.ReservationForDate != dt){
          this.reservation.ReservationForDate = dt;
          // load periods
          this.reservationPlaceSvc.getAvailablePeriods(this.reservation.ReservationPlaceId, this.reservation.ReservationForDate)
            .subscribe(x=>
            {
              this.reservationPeriods = x;
              this.checkedPeriodsFormArray.clear();
              this.reservationPeriods.forEach(x => this.checkedPeriodsFormArray.push(new FormControl(x.Checked)));
            });
        }

        break;

      case 3:
        console.log(this.periodFormGroup);
        // period
        console.log(this.reservationPeriods);
        let checkedPeriods = new Array();
        if(this.reservationPeriods)
        {
          checkedPeriods = this.reservationPeriods.filter(p=>p.Checked);
        }
        //
        if(checkedPeriods.length == 0){
          alert(`至少要选中一个时间段`);
          return;
        }
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
    if(!this.reservation.ReservationForTime){
      alert(`至少要选中一个时间段`);
      return;
    }
    if(this.captchaValid === false){
      this.loadCaptcha();
      return;
    }
    console.log(`submit reservation`);
    if(this.submiting === true){
      return;
    }
    this.submiting = true;
    this.reservationSvc.NewReservation(this.reservation, 'Tencent', JSON.stringify(this.captchaInfo))
    .subscribe(x=> {
      console.log(x);
      if(x.Status === 200 || x.Status === 'Success'){
        let snackBarRef = this.snackBar.open("预约成功", "" , {
          duration: 2000,
        });
        snackBarRef.afterDismissed()
        .subscribe(x=>{
          this.submiting = false;
          this.router.navigateByUrl("");
        });
      }else{
        alert(x.ErrorMsg);
        this.captchaValid = false;
        this.submiting = false;
      }
    });
  }
}
