import { Component, OnInit } from '@angular/core';
import { Relation } from 'src/_models/relation';
import { DbService } from 'src/_services/db.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-relations',
	templateUrl: './relations.component.html',
	styleUrls: ['./relations.component.css']
})
export class RelationsComponent implements OnInit {

	relations: Relation[];

	constructor(private dbService: DbService, private router: Router) { }

	ngOnInit() {
		this.dbService.getRelations().subscribe((result) => {
			this.relations = result;
		}, error => {
			console.log(error);
		})
	}

	viewRow(relationId: number) {
		this.router.navigate(['/detailed/' + relationId]);
	}

	getKeyString(keys: {[keyName: string]: string}) {
		let result = "";

		Object.keys(keys).forEach(function(key) {			
			result += key + "=" + keys[key] + ", ";
		});

		return result.substring(0, result.length - 2);
	}

	addRelation() {	
		this.router.navigate(['/create/']);	
	}

}
