export class ReservationPeriod{
    
    public PeriodId : string;
    
    public PeriodIndex : number;
    
    public PeriodTitle : string;
    
    public IsCanReservate : boolean;
    
    
    private _Checked : boolean = false;
    public get Checked() : boolean {
        return this._Checked;
    }
    public set Checked(v : boolean) {
        if(v){
            this._Checked = true;
        }else{
            this._Checked = false;
        }
    }
    

    constructor() {
        this.Checked = false;
    }
}