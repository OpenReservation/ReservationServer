import { Component, OnInit } from '@angular/core';
import { LoadingService } from '../services/LoadingService';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.less']
})
export class AboutComponent implements OnInit {

  constructor(private loadingSvc: LoadingService) { 
    this.loadingSvc.isLoading = false;
  }

  ngOnInit() {
    
  }

}
