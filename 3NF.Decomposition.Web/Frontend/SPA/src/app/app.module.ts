import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { RelationsComponent } from '../relation-list/relations.component';
import { appRoutes } from 'src/routes';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DbService } from '../_services/db.service';
import { HttpClientModule } from '@angular/common/http';
import { RelationDetailedComponent } from '../relation-detailed/relation-detailed.component';
import { PanelModule } from 'primeng/panel';
import { InputTextModule } from 'primeng/inputtext';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

@NgModule({
   declarations: [
      AppComponent,
      RelationsComponent,
      RelationDetailedComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      RouterModule.forRoot(appRoutes),
      TableModule,
	  ButtonModule,
	  PanelModule,
	  InputTextModule,
	  BrowserAnimationsModule
   ],
   providers: [
      DbService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
