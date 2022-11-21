import { TransactionType } from "../Enums/TransactionType.enum";

export class CreateUpdateTransactionDto {
	customerId: string = "";
	payDate!: Date;
	type: TransactionType = TransactionType.Rut;
}