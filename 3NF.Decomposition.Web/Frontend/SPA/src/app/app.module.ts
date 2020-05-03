import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { RelationsComponent } from '../relations/relations.component';
import { appRoutes } from 'src/routes';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DbService } from '../_services/db.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
	declarations: [
		AppComponent,
		RelationsComponent
	],
	imports: [
		BrowserModule,
		HttpClientModule,
		FormsModule,
		RouterModule.forRoot(appRoutes),
		TableModule,
		ButtonModule
	],
	providers: [
		DbService,
	],
	bootstrap: [
		AppComponent
	]
})
export class AppModule { }
