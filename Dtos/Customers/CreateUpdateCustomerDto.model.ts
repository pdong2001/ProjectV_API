
export class CreateUpdateCustomerDto {
	code: string | null = "";
	note: string | null = "";
	address: string | null = "";
	fullName: string | null = "";
	costPercent: any = 0;
	phoneNumber: string | null = "";
	limit: number | null = null;
	bankId: string | null = null;
}