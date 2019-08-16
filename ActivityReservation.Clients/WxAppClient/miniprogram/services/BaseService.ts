import { PagedListData } from './../models/PagedListData';

export class BaseService<TModel>{
  protected readonly apiBaseUrl:string;

  constructor(protected apiPath:string){
    this.apiBaseUrl = apiPath;
  }

  public Get(params?:any) {
    let url = `${this.apiBaseUrl}/api/${this.apiPath}`;
    if(params != undefined && Object.keys(params).length > 0){
      url += "?";      
      for(let name of Object.keys(params)){
        url += `${name}=${params[name]}&`;
      }
    }
    wx.request({
     url: url,
     success: (response)=>{
       console.log(response.data);
     }
    });
  }

  public GetAll(): void {
    //return this.http.get<Array<TModel>>(`${this.apiBaseUrl}/api/${this.apiPath}`);
  }

  public GetDetails(id: number|string, params?: any): void {
    let url = `${this.apiBaseUrl}/api/${this.apiPath}/${id}`;
    if(params && Object.keys(params).length > 0){
      url += "?";
      for(let name of Object.keys(params)){
        url += `${name}=${params[name]}&`;
      }
    }
    //return this.http.get<TModel>(url);
  }


  public Post(model: TModel): void {
    //return this.http.post(`${this.apiBaseUrl}/api/${this.apiPath}`, model);
  }
}
