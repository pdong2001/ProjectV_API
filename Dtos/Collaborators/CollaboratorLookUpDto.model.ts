
export class CollaboratorLookUpDto {
	pageSize: number = 0;
	pageIndex: number = 0;
	sort: string | null = "";
	search: string | null = "";
	columns: string | null = "";
}