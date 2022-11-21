
export class ServiceResponse<TData> {
	data!: TData | null;
	message: string | null = "";
	success: boolean = false;
}