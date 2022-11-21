import { Observable } from 'rxjs';
import { PageResultDto } from '../models/PageResultDto.model';
import { ServiceResponse } from '../models/ServiceResponse.model';
import { HttpService } from './http.service';

export class PagedCRUDService<TKey, TDto, TUpSert, TLookUp> {
  public controller: string;
  public prefix: string = 'api/';
  constructor(protected http: HttpService, controller: string) {
    this.controller = controller;
  }

  public get(id: TKey): Observable<ServiceResponse<TDto>> {
    const url = `${this.prefix}${this.controller}/${id}`;
    return this.http.get<ServiceResponse<TDto>>(url);
  }
  public create(payload: TUpSert): Observable<ServiceResponse<TDto>> {
    const url = `${this.prefix}${this.controller}`;
    return this.http.post<ServiceResponse<TDto>>(url, payload);
  }
  public update(id: TKey, payload: TUpSert): Observable<ServiceResponse<TDto>> {
    const url = `${this.prefix}${this.controller}/${id}`;
    return this.http.put<ServiceResponse<TDto>>(url, payload);
  }
  public getList(): Observable<ServiceResponse<TDto[]>> {
    const url = `${this.prefix}${this.controller}`;
    return this.http.get<ServiceResponse<TDto[]>>(url);
  }
  public search(
    request: TLookUp
  ): Observable<ServiceResponse<PageResultDto<TDto>>> {
    const url = `${this.prefix}${this.controller}/search`;
    return this.http.get<ServiceResponse<PageResultDto<TDto>>>(url, request);
  }
}
