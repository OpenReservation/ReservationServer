export class Reservation{
    
    private reservationForDate : string;
    public get ReservationForDate() : string {
        return this.reservationForDate;
    }
    public set ReservationForDate(v : string) {
        this.reservationForDate = v;
    }
    
    private reservationForTime : string;
    public get ReservationForTime() : string {
        return this.reservationForTime;
    }
    public set ReservationForTime(v : string) {
        this.reservationForTime = v;
    }
    
    private reservationForTimeIds : string;
    public get ReservationForTimeIds() : string {
        return this.reservationForTimeIds;
    }
    public set ReservationForTimeIds(v : string) {
        this.reservationForTimeIds = v;
    }
    
    private reservationPersonPhone : string;
    public get ReservationPersonPhone() : string {
        return this.reservationPersonPhone;
    }
    public set ReservationPersonPhone(v : string) {
        this.reservationPersonPhone = v;
    }
    
    private reservationPersonName : string;
    public get ReservationPersonName() : string {
        return this.reservationPersonName;
    }
    public set ReservationPersonName(v : string) {
        this.reservationPersonName = v;
    }
    
    
    private reservationUnit : string;
    public get ReservationUnit() : string {
        return this.reservationUnit;
    }
    public set ReservationUnit(v : string) {
        this.reservationUnit = v;
    }
    
    
    private reservationPlaceName : string;
    public get ReservationPlaceName() : string {
        return this.reservationPlaceName;
    }
    public set ReservationPlaceName(v : string) {
        this.reservationPlaceName = v;
    }

    
    private reservationPlaceId : string;
    public get ReservationPlaceId() : string {
        return this.reservationPlaceId;
    }
    public set ReservationPlaceId(v : string) {
        this.reservationPlaceId = v;
    }
    
    
    
    private reservationActivityContent : string;
    public get ReservationActivityContent() : string {
        return this.reservationActivityContent;
    }
    public set ReservationActivityContent(v : string) {
        this.reservationActivityContent = v;
    }
    
    
    private reservationId : string;
    public get ReservationId() : string {
        return this.reservationId;
    }
    public set ReservationId(v : string) {
        this.reservationId = v;
    }
    
    
    private reservationTime : Date;
    public get ReservationTime() : Date {
        return this.reservationTime;
    }
    public set ReservationTime(v : Date) {
        this.reservationTime = v;
    }
    
    
    private reservationStatus : number;
    public get ReservationStatus() : number {
        return this.reservationStatus;
    }
    public set ReservationStatus(v : number) {
        this.reservationStatus = v;
    }
    
}