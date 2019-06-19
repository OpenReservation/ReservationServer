import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatOptionModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSliderModule } from '@angular/material/slider';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatTableModule} from '@angular/material/table';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatPaginatorModule} from '@angular/material/paginator';

@NgModule({
  imports: [
    MatButtonModule, 
    MatCheckboxModule, 
    MatToolbarModule, 
    MatChipsModule, 
    MatOptionModule, 
    MatGridListModule, 
    MatProgressBarModule, 
    MatSliderModule, 
    MatSlideToggleModule, 
    MatMenuModule, 
    MatDialogModule, 
    MatSnackBarModule, 
    MatSelectModule, 
    MatInputModule, 
    MatSidenavModule, 
    MatCardModule, 
    MatIconModule, 
    MatRadioModule, 
    MatProgressSpinnerModule, 
    MatTabsModule,
    MatListModule,
    MatTableModule,
    BrowserAnimationsModule,
    MatPaginatorModule
  ],
  exports: [
    MatButtonModule, 
    MatCheckboxModule, 
    MatToolbarModule, 
    MatChipsModule, 
    MatOptionModule, 
    MatGridListModule, 
    MatProgressBarModule, 
    MatSliderModule, 
    MatSlideToggleModule, 
    MatMenuModule, 
    MatDialogModule, 
    MatSnackBarModule, 
    MatSelectModule, 
    MatInputModule, 
    MatSidenavModule, 
    MatCardModule, 
    MatIconModule, 
    MatRadioModule, 
    MatProgressSpinnerModule, 
    MatTabsModule, 
    MatListModule, 
    MatTableModule, 
    BrowserAnimationsModule,
    MatPaginatorModule
  ],
})
export class AppMaterialModule { }