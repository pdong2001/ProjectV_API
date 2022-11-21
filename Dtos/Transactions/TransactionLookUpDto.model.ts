import { StatusType } from "../Enums/StatusType.enum";
import { TransactionType } from "../Enums/TransactionType.enum";
import { CostStatusType } from "../Enums/CostStatusType.enum";

export class TransactionLookUpDto {
	pageSize: number = 0;
	pageIndex: number = 0;
	sort: string | null = "";
	search: string | null = "";
	columns: string | null = "";
	date: Date | null = null;
	type: TransactionType | null = null;
	status: StatusType | null = null;
	sender: string | null = null;
	customerId: string | null = null;
	costStatus: CostStatusType | null = null;
	collaboratorId: string | null = null;
}