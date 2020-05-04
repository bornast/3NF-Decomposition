import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DbService } from 'src/_services/db.service';
import { Relation } from 'src/_models/relation';

@Component({
	selector: 'app-relation-detailed',
	templateUrl: './relation-detailed.component.html',
	styleUrls: ['./relation-detailed.component.css']
})
export class RelationDetailedComponent implements OnInit {

	relation: Relation;
	relationId: number;
	displayHtml: boolean = false;
	decompositionResult: string;
	relationKeys: string;

	constructor(private dbService: DbService, private route: ActivatedRoute, private router: Router) { }

	ngOnInit() {
		this.route.params.subscribe(params => {
			this.dbService.getRelation(params["id"]).subscribe((result) => {
				this.relation = result;
				this.relationKeys = this.getKeyString(this.relation.keys);
				this.displayHtml = true;
			}, error => {
				this.router.navigate(['']);
			})
		});
	}	

	getKeyString(keys: {[keyName: string]: string}) {
		let result = "";

		Object.keys(keys).forEach(function(key) {			
			result += key + "=" + keys[key] + ", ";
		});

		return result.substring(0, result.length - 2);
	}

	decompose() {
		this.dbService.decompose(this.relation.id).subscribe((result) => {
			this.decompositionResult = result;
		}, error => {
			console.log(error);
			this.decompositionResult = "error!";
		});
	}

	cancel() {
		this.router.navigate(['']);
	}

}
