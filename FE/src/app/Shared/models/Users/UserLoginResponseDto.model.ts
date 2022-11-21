import { UserDto } from "../Users/UserDto.model";

export class UserLoginResponseDto {
	token: string | null = "";
	user: UserDto | null = null;
	expires!: Date;
}