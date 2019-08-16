
import { BaseService } from './BaseService';
import { Notice } from '../models/Notice';

export class NoticeService extends BaseService<Notice>{

  constructor(){
    super('Notice');
  }

}
