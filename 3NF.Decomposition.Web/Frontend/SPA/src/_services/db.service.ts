import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Relation } from 'src/_models/relation';

@Injectable({
	providedIn: 'root'
})
export class DbService {
	baseUrl = environment.apiUrl + 'db/';

	constructor(private http: HttpClient) { }
	

	getRelations(): Observable<Relation[]> {
		return this.http.get<Relation[]>(this.baseUrl + 'GetRelations');
	}

	getRelation(id: number): Observable<Relation> {
		return this.http.get<Relation>(this.baseUrl + 'GetRelation/' + id);
	}
}
