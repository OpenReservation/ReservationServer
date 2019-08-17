import { PagedListData } from './../models/PagedListData';

export class BaseService<TModel>{
  protected readonly apiBaseUrl:string = "https://reservation.weihanli.xyz";

  constructor(protected apiPath:string){
  }

  public Get(callback: (result:PagedListData<TModel>) => void, params?:any) {
    wx.showLoading({
      title: "loading..."
    });
    let url = `${this.apiBaseUrl}/api/${this.apiPath}`;
    if(params != undefined && Object.keys(params).length > 0){
      url += "?";      
      for(let name of Object.keys(params)){
        url += `${name}=${params[name]}&`;
      }
    }
    console.log(`url: ${url}`);
    wx.request({
     url: url,
     success: (response)=>{
       let result = <PagedListData<TModel>>response.data;
       callback(result);
       wx.hideLoading();
     }
    });
  }

  public GetAll(callback: (result: Array<TModel>)=>void): void {
    wx.showLoading({
      title: "loading..."
    });
    wx.request({
      url: `${this.apiBaseUrl}/api/${this.apiPath}`,
      success: (response) => {
        wx.hideLoading();
        let result = <Array<TModel>>response.data;
        callback(result);
      }
    });
  }

  public GetDetails(callback:(result:TModel)=>void, id: number|string, params?: any): void {
    wx.showLoading({
      title: "loading..."
    });
    let url = `${this.apiBaseUrl}/api/${this.apiPath}/${id}`;
    if(params && Object.keys(params).length > 0){
      url += "?";
      for(let name of Object.keys(params)){
        url += `${name}=${params[name]}&`;
      }
    }
    wx.request({
      url: `${this.apiBaseUrl}/api/${this.apiPath}`,
      success: (response) => {
        wx.hideLoading();
        let result = <TModel><any>response.data;
        callback(result);
      }
    });
  }


  public Post(model: TModel): void {
    //return this.http.post(`${this.apiBaseUrl}/api/${this.apiPath}`, model);
  }
}
