export interface Relation {
	id: number;
	relation: string;
	keys: {[keyName: string]: string};
	fmin: string;	
}
