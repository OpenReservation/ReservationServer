export class ReservationPlace{
    
    private _PlaceId : string;
    public get PlaceId() : string {
        return this._PlaceId;
    }
    public set PlaceId(v : string) {
        this._PlaceId = v;
    }
    
    
    private _PlaceName : string;
    public get PlaceName() : string {
        return this._PlaceName;
    }
    public set PlaceName(v : string) {
        this._PlaceName = v;
    }
    
    
    private _PlaceIndex : number;
    public get PlaceIndex() : number {
        return this._PlaceIndex;
    }
    public set PlaceIndex(v : number) {
        this._PlaceIndex = v;
    }
    
    
    private _MaxReservationPeriodNum : number;
    public get MaxReservationPeriodNum() : number {
        return this._MaxReservationPeriodNum;
    }
    public set MaxReservationPeriodNum(v : number) {
        this._MaxReservationPeriodNum = v;
    }
    
}