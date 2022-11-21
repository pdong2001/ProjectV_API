import { TransactionType } from "../Enums/TransactionType.enum";
import { StatusType } from "../Enums/StatusType.enum";
import { CostStatusType } from "../Enums/CostStatusType.enum";

export class PatchTransactionDto {
	note: string | null = "";
	check: string | null = "";
	type: TransactionType | null = null;
	posId: string | null = null;
	status: StatusType | null = null;
	payDate: Date | null = null;
	workerId: string | null = null;
	withDraw: number | null = null;
	senderId: string | null = null;
	debtCost: number | null = null;
	customerId: string | null = null;
	costStatus: CostStatusType | null = null;
	posWithDraw: number | null = null;
	collaboratorId: string | null = null;
}