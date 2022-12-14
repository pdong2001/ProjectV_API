import { CustomerDto } from "../Customers/CustomerDto.model";
import { CollaboratorDto } from "../Collaborators/CollaboratorDto.model";
import { TransactionType } from "../Enums/TransactionType.enum";
import { StatusType } from "../Enums/StatusType.enum";
import { UserDto } from "../Users/UserDto.model";
import { StaffDto } from "../Staffes/StaffDto.model";
import { CostStatusType } from "../Enums/CostStatusType.enum";
import { POSDto } from "../POSes/POSDto.model";

export class TransactionDto {
	id: string = "";
	customerId: string = "";
	cost: number = 0;
	withDraw: number = 0;
	debtCost: number = 0;
	posRefund: number = 0;
	bankRefund: number = 0;
	posWithDraw: number = 0;
	moneyRemaining: number = 0;
	customerReceive: number = 0;
	pos: POSDto | null = null;
	note: string | null = "";
	check: string | null = "";
	percent: any = 0;
	costPercent: any = 0;
	worker: UserDto | null = null;
	sender: StaffDto | null = null;
	payDate!: Date;
	createdAt!: Date;
	limit: number | null = null;
	posId: string | null = null;
	status: StatusType = StatusType.Pending;
	workerId: string | null = null;
	senderId: string | null = null;
	collaboratorId: string | null = null;
	customer: CustomerDto | null = null;
	costStatus: CostStatusType = CostStatusType.DaTT;
	type: TransactionType = TransactionType.Rut;
	collaborator: CollaboratorDto | null = null;
}