
export class CreateUpdateUserDto {
	email: string | null = "";
	userName: string | null = "";
	password: string | null = "";
	fullName: string | null = "";
	phoneNumber: string | null = "";
	roles: any | null = [];
}