import {
  HttpHeaders,
  HttpContext,
  HttpParams,
  HttpClient,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private REST_API_SERVER = environment.REST_API_SERVER;

  constructor(private httpClient: HttpClient) {}

  // Trong pipe() sẽ nhận được result hoặc error
  get<T>(
    url: string,
    payload?: any,
    options?: {
      headers?:
        | HttpHeaders
        | {
            [header: string]: string | string[];
          };
      context?: HttpContext;
      observe?: 'body';
      params?:
        | HttpParams
        | {
            [param: string]:
              | string
              | number
              | boolean
              | ReadonlyArray<string | number | boolean>;
          };
      reportProgress?: boolean;
      responseType?: 'json';
      withCredentials?: boolean;
    }
  ): Observable<T> {
    if (payload) {
      if (!options) options = {};
      const params: { [index: string]: any } = {};
      for (let key of Object.keys(payload)) {
        const value = payload[key];
        if (value) {
          params[key] = value;
        }
      }
      options.params = params;
    }
    return this.httpClient.get<T>(`${this.REST_API_SERVER}/${url}`, options);
  }

  post<T>(url: string, data: any = null, options = {}): Observable<T> {
    return this.httpClient.post<T>(
      `${this.REST_API_SERVER}/${url}`,
      data,
      options
    );
  }

  put<T>(url: string, data: any = null, options = {}): Observable<T> {
    return this.httpClient.put<T>(
      `${this.REST_API_SERVER}/${url}`,
      data,
      options
    );
  }

  delete<T>(url: string, options = {}): Observable<T> {
    return this.httpClient.delete<T>(`${this.REST_API_SERVER}/${url}`, options);
  }
}
