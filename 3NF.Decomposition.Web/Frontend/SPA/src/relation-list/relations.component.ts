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
		// this.dbService.getRelations().subscribe((result) => {
		// 	this.relations = result;
		// }, error => {
		// 	console.log(error);
		// })
		this.getRelations();
	}

	getRelations() {
		this.dbService.getRelations().subscribe((result) => {
			this.relations = result;
		}, error => {
			console.log(error);
		});
	}

	viewRow(relationId: number) {
		this.router.navigate(['/detailed/' + relationId]);
	}

	deleteRow(relationId: number) {
		var confirmation = confirm("Are you sure you want to delete this row?");
		if (confirmation == true) {
			this.dbService.deleteRelation(relationId).subscribe(() => {
				this.getRelations();
			}, error => {
				console.log(error);
			});
		}
	}

	getKeyString(keys: { [keyName: string]: string }) {
		let result = "";

		Object.keys(keys).forEach(function (key) {
			result += key + "=" + keys[key] + ", ";
		});

		return result.substring(0, result.length - 2);
	}

	addRelation() {
		this.router.navigate(['/create/']);
	}

}
