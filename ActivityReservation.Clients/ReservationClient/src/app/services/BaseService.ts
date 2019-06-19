import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedListData } from '../models/PagedListData';

@Injectable()
export class BaseService<TModel>{
  protected readonly apiBaseUrl = "https://reservation.weihanli.xyz";

  constructor(protected http:HttpClient, protected apiPath:string){}

  public Get(params?: Map<string, string>): Observable<PagedListData<TModel>> {
    return this.http.get<PagedListData<TModel>>(`${this.apiBaseUrl}/api/${this.apiPath}`);
  }

  public GetAll(): Observable<Array<TModel>> {
    return this.http.get<Array<TModel>>(`${this.apiBaseUrl}/api/${this.apiPath}`);
  }

  public GetDetails(id: number|string, params?: Map<string, string>): Observable<TModel> {
    let url = `${this.apiBaseUrl}/api/${this.apiPath}/${id}`;
    return this.http.get<TModel>(url);
  }


  public Post(model: TModel): Observable<TModel> {
    return this.http.post<TModel>(`${this.apiBaseUrl}/api/${this.apiPath}`, model);
  }
}
