import { Routes } from '@angular/router';
import { RelationsComponent } from './relations/relations.component';

export const appRoutes: Routes = [
	{ path: '', component: RelationsComponent },	
	// { path: 'members', component: MemberListComponent, canActivate: [AuthGuard] },
	// { path: 'messages', component: MessagesComponent },
	// { path: 'lists', component: ListsComponent },
];