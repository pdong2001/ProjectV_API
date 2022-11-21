
export class POSLookUpDto {
	pageSize: number = 0;
	pageIndex: number = 0;
	sort: string | null = "";
	search: string | null = "";
	columns: string | null = "";
	inChargeId: string | null = null;
}