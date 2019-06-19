export class PagedListModel{

    private _PageSize : number = 10;
    public get PageSize() : number {
        return this._PageSize;
    }
    public set PageSize(v : number) {
        this._PageSize = v;
    }

    private _PageNumber : number;
    public get PageNumber() : number {
        return this._PageNumber;
    }
    public set PageNumber(v : number) {
        this._PageNumber = v;
    }

    private _Count : number;
    public get Count() : number {
        return this._Count;
    }
    public set Count(v : number) {
        this._Count = v;
    }

    private _PageCount : number;
    public get PageCount() : number {
        return this._PageCount;
    }
    public set PageCount(v : number) {
        this._PageCount = v;
    }

    private _TotalCount : number;
    public get TotalCount() : number {
        return this._TotalCount;
    }
    public set TotalCount(v : number) {
        this._TotalCount = v;
    }
}

export class PagedListData<T> extends PagedListModel{

  private _Data : Array<T>;
  public get Data() : Array<T> {
      return this._Data;
  }
  public set Data(v : Array<T>) {
      this._Data = v;
  }
}
