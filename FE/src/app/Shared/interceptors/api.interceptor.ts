import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpResponse,
} from '@angular/common/http';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  private dateRegex =
    /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})[.]\d+(([+|-]\d{2}:\d{2})|Z)$/;

  constructor(
    messageService: MessageService,
    router: Router,
    private auth: AuthService
  ) {
    ApiInterceptor.messageService = messageService;
    ApiInterceptor.router = router;
  }

  static messageService: MessageService;
  static nextMessageAt: Date = new Date();
  static processingUnauthorized: boolean = false;
  static router: Router;
  handleError(error: HttpErrorResponse) {
    switch (error.status) {
      case 500:
        if (new Date() >= ApiInterceptor.nextMessageAt) {
          ApiInterceptor.nextMessageAt = new Date(Date.now() + 2000);
          ApiInterceptor.messageService.add({
            severity: 'error',
            summary: 'Thông báo',
            detail: `Đã có lỗi xảy ra! ${
              environment.production ? '' : error.message
            }`,
          });
        }
        break;
      case 401:
        if (
          !ApiInterceptor.processingUnauthorized &&
          new Date() >= ApiInterceptor.nextMessageAt
        ) {
          ApiInterceptor.processingUnauthorized = true;
          new Promise(() => {
            alert('Bạn không có quyền truy cập vào chức năng này!');
            // const origin = location.origin;
            ApiInterceptor.nextMessageAt = new Date(Date.now() + 2000);
            ApiInterceptor.processingUnauthorized = false;
          });
        }
        break;
      default:
        break;
    }
    return throwError(() => error);
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = this.auth.getToken();
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }
    return next.handle(request).pipe(
      catchError(this.handleError),
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          this.convertDates(event.body);
        }
      })
    );
  }

  private convertDates(object: any) {
    if (!object || !(object instanceof Object)) {
      return;
    }

    if (object instanceof Array) {
      for (const item of object) {
        this.convertDates(item);
      }
    }

    for (const key of Object.keys(object)) {
      const value = object[key];

      if (value instanceof Array) {
        for (const item of value) {
          this.convertDates(item);
        }
      }

      if (value instanceof Object) {
        this.convertDates(value);
      }

      if (typeof value === 'string' && this.dateRegex.test(value)) {
        object[key] = new Date(value);
      }
    }
  }
}
