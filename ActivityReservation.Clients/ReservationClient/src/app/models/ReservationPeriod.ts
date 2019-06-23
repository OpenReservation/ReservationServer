export class ReservationPeriod{
    
    public PeriodId : string;
    
    public PeriodIndex : number;
    
    public PeriodTitle : string;
    
    public IsCanReservate : boolean;
    
    public Checked : boolean;

    constructor() {
        this.Checked = false;
    }
}