import { Component, OnInit } from '@angular/core';
import { Relation } from 'src/_models/relation';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { FunctionalDependency } from 'src/_models/functional-dependency';
import { DbService } from 'src/_services/db.service';

@Component({
	selector: 'app-relation-create',
	templateUrl: './relation-create.component.html',
	styleUrls: ['./relation-create.component.css']
})
export class RelationCreateComponent implements OnInit {

	displayHtml: boolean;
	relation: string;
	keys: string[] = [""];
	functionalDependencies: FunctionalDependency[] = [];

	constructor(private dbService: DbService, private router: Router, private messageService: MessageService) { }

	ngOnInit() {
		let emptyFunctionalDependency: FunctionalDependency = {
			leftSideAttributes: "",
			rightSideAttributes: ""
		};
		this.functionalDependencies.push(emptyFunctionalDependency);
		this.displayHtml = true;
	}

	cancel() {
		this.router.navigate(['']);
	}

	addKey() {
		this.keys.push("");
	}

	updateKeyValue(keyIndex: number, value: string) {
		this.keys[keyIndex] = value;
	}

	addFunctionalDependency() {
		let newFunctionalDependency: FunctionalDependency = {
			leftSideAttributes: "",
			rightSideAttributes: ""
		};
		this.functionalDependencies.push(newFunctionalDependency);
	}

	updateLeftSideAttributesOfFunctionalDependecy(functionalDependencyIndex: number, value: string) {		
		this.functionalDependencies[functionalDependencyIndex].leftSideAttributes = value;
	}

	updateRightSideAttributesOfFunctionalDependecy(functionalDependencyIndex: number, value: string) {
		this.functionalDependencies[functionalDependencyIndex].rightSideAttributes = value;
	}

	save() {
		// RELATION
		let relationToSubmit = {};
		
		// ATTRIBUTES
		let relationArray = this.relation.split(",").map(s => s.trim());
		relationToSubmit["attributes"] = relationArray;

		// KEYS
		let keyIndex = 1;
		relationToSubmit["keys"] = {};
		this.keys.forEach(key => {
			let keyArray = key.split(",").map(s => s.trim());
			relationToSubmit["keys"][keyIndex] = keyArray;
			keyIndex+=1;
		});

		// Fmin
		relationToSubmit["fmin"] = [];
		this.functionalDependencies.forEach(fd => {
			let leftFunctionalDependencyArray = fd.leftSideAttributes.split(",").map(s => s.trim());
			let rightFunctionalDependencyArray = fd.rightSideAttributes.split(",").map(s => s.trim());

			let functionalDependency = {};

			functionalDependency["leftSideAttributes"] = leftFunctionalDependencyArray;
			functionalDependency["rightSideAttributes"] = rightFunctionalDependencyArray;

			relationToSubmit["fmin"].push(functionalDependency);
		});		

		this.dbService.createRelation(relationToSubmit).subscribe(() => {
			this.messageService.add({ severity: 'success', summary: 'Service Message', detail: 'Succesfully created' });
			this.router.navigate(['']);
		}, error => {
			console.log(error);
			this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Error happend while creating a relation!' });
		});
		
	}


}
