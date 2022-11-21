import { StatusType } from "../Enums/StatusType.enum";

export class UpdateTransactionStatusDto {
	transactionId: string = "";
	status: StatusType = StatusType.Pending;
}