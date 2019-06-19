export class Reservation{
    
    private _ReservationForDate : string;
    public get ReservationForDate() : string {
        return this._ReservationForDate;
    }
    public set ReservationForDate(v : string) {
        this._ReservationForDate = v;
    }
    
    private _ReservationForTime : string;
    public get ReservationForTime() : string {
        return this._ReservationForTime;
    }
    public set ReservationForTime(v : string) {
        this._ReservationForTime = v;
    }
    
    private _ReservationPersonPhone : string;
    public get ReservationPersonPhone() : string {
        return this._ReservationPersonPhone;
    }
    public set ReservationPersonPhone(v : string) {
        this._ReservationPersonPhone = v;
    }
    

    
    private _ReservationPersonName : string;
    public get ReservationPersonName() : string {
        return this._ReservationPersonName;
    }
    public set ReservationPersonName(v : string) {
        this._ReservationPersonName = v;
    }
    
    
    private _ReservationUnit : string;
    public get ReservationUnit() : string {
        return this._ReservationUnit;
    }
    public set ReservationUnit(v : string) {
        this._ReservationUnit = v;
    }
    
    
    private _ReservationPlaceName : string;
    public get ReservationPlaceName() : string {
        return this._ReservationPlaceName;
    }
    public set ReservationPlaceName(v : string) {
        this._ReservationPlaceName = v;
    }
    
    
    private _ReservationActivityContent : string;
    public get ReservationActivityContent() : string {
        return this._ReservationActivityContent;
    }
    public set ReservationActivityContent(v : string) {
        this._ReservationActivityContent = v;
    }
    
    
    private _ReservationId : string;
    public get ReservationId() : string {
        return this._ReservationId;
    }
    public set ReservationId(v : string) {
        this._ReservationId = v;
    }
    
    
    private _ReservationTime : Date;
    public get ReservationTime() : Date {
        return this._ReservationTime;
    }
    public set ReservationTime(v : Date) {
        this._ReservationTime = v;
    }
    
    
    private _ReservationStatus : number;
    public get ReservationStatus() : number {
        return this._ReservationStatus;
    }
    public set ReservationStatus(v : number) {
        this._ReservationStatus = v;
    }
    
}