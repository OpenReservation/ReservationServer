export class ReservationPeriod{
    
    private _PeriodId : string;
    public get PeriodId() : string {
        return this._PeriodId;
    }
    public set PeriodId(v : string) {
        this._PeriodId = v;
    }
    
    private _PeriodIndex : number;
    public get PeriodIndex() : number {
        return this._PeriodIndex;
    }
    public set PeriodIndex(v : number) {
        this._PeriodIndex = v;
    }
    
    
    private _PeriodTitle : string;
    public get PeriodTitle() : string {
        return this._PeriodTitle;
    }
    public set PeriodTitle(v : string) {
        this._PeriodTitle = v;
    }
    
    private _IsCanReservate : boolean;
    public get IsCanReservate() : boolean {
        return this._IsCanReservate;
    }
    public set IsCanReservate(v : boolean) {
        this._IsCanReservate = v;
    }
    
}