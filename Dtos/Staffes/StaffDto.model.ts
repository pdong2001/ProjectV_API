import { UserDto } from "../Users/UserDto.model";

export class StaffDto {
	id: string = "";
	note: string | null = "";
	address: string | null = "";
	fullName: string | null = "";
	phoneNumber: string | null = "";
	user: UserDto | null = null;
	userId: string | null = null;
}