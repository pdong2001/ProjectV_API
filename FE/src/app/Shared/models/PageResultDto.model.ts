
export class PageResultDto<TData> {
	total: number = 0;
	items: TData[] | null = [];
}