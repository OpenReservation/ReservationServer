import { PagedListData } from './../models/PagedListData';
import { RetryHelper } from '../utils/RetryHelper';

export class BaseService<TModel>{
  protected readonly apiBaseUrl: string = "https://service-balxf7hr-1251288923.ap-shanghai.apigateway.myqcloud.com/release/reservationWxAppGateway";

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
       console.log(response);
       let result = <PagedListData<TModel>>response.data;
       callback(result);
       wx.hideLoading();
     },
     fail: (err)=>{

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
        console.log(response);
        wx.hideLoading();
        let result = <Array<TModel>>response.data;
        callback(result);
      },
      fail: (err)=>{
       
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
      url: url,
      success: (response) => {
        console.log(response);
        wx.hideLoading();
        let result = <TModel><any>response.data;
        callback(result);
      },
      fail: (err)=>{
       
      }
    });
  }


  public Post(model: TModel): void {
    //return this.http.post(`${this.apiBaseUrl}/api/${this.apiPath}`, model);
  }
}
