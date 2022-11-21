import { StaffDto } from "../Staffes/StaffDto.model";
import { AddressDto } from "../AddressDto.model";

export class POSDto {
	id: string = "";
	inChargeId: string = "";
	balance: number = 0;
	name: string | null = "";
	note: string | null = "";
	percent: any = 0;
	inCharge: StaffDto | null = null;
	province: AddressDto | null = null;
	provinceId: number | null = null;
}