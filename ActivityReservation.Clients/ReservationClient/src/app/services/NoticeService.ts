import { Injectable } from '@angular/core';
import { BaseService } from './BaseService';
import { HttpClient } from '@angular/common/http';
import { Notice } from '../models/Notice';
import { ConfigService } from './ConfigService';

@Injectable({
  providedIn: 'root'
})
export class NoticeService extends BaseService<Notice>{

  constructor(http: HttpClient, config: ConfigService){
    super(http, config, 'Notice');
  }

}
