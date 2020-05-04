import { Routes } from '@angular/router';
import { RelationsComponent } from './relation-list/relations.component';
import { RelationDetailedComponent } from './relation-detailed/relation-detailed.component';
import { RelationCreateComponent } from './relation-create/relation-create.component';

export const appRoutes: Routes = [
	{ path: '', component: RelationsComponent },
	{ path: 'detailed/:id', component: RelationDetailedComponent },
	{ path: 'create', component: RelationCreateComponent },
];