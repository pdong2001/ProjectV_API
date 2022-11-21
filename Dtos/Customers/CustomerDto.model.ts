import { BankDto } from "../Banks/BankDto.model";
import { TransactionDto } from "../Transactions/TransactionDto.model";

export class CustomerDto {
	id: string = "";
	code: string | null = "";
	note: string | null = "";
	address: string | null = "";
	fullName: string | null = "";
	costPercent: any = 0;
	phoneNumber: string | null = "";
	bank: BankDto | null = null;
	limit: number | null = null;
	bankId: string | null = null;
	transactions: TransactionDto[] | null = [];
}