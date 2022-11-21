import { Injectable } from '@angular/core';
import { lastValueFrom, tap } from 'rxjs';
import { ServiceResponse } from '../models/ServiceResponse.model';
import { UserLoginRequestDto } from '../models/Users/UserLoginRequestDto.model';
import { UserLoginResponseDto } from '../models/Users/UserLoginResponseDto.model';
import { HttpService } from './http.service';
import { StorageKeys } from '../constants/constants.module';
import { UserDto } from '../models/Users/UserDto.model';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  roles!: string[];
  constructor(private http: HttpService) {}

  public login(request: UserLoginRequestDto) {
    return this.http
      .post<ServiceResponse<UserLoginResponseDto>>('api/auth/login', request)
      .pipe(
        tap((res) => {
          console.log(res);
          if (res.success && res.data) {
            this.setToken(res.data);
            return true;
          }
          return false;
        })
      );
  }

  logout() {
    this.setToken(null);
  }

  public user() {
    return this.http.get<ServiceResponse<UserDto>>('api/auth/user').pipe(
      tap((res) => {
        if (res.success && res.data) {
          this.roles = res.data.roles ?? [];
        }
      })
    );
  }

  public getToken(): string | null {
    console.time('ReadToken');
    var strExpires = localStorage.getItem(StorageKeys.EXPIRES_KEY);
    if (strExpires) {
      var expires = JSON.parse(strExpires) as number;
      if (expires > new Date().getTime()) {
        console.timeEnd('ReadToken');
        return localStorage.getItem(StorageKeys.TOKEN_KEY);
      }
    }
    console.timeEnd('ReadToken');
    return null;
  }

  public setToken(data: UserLoginResponseDto | null) {
    console.log(data);
    if (data) {
      if (data.token) {
        localStorage.setItem(
          StorageKeys.EXPIRES_KEY,
          JSON.stringify(data.expires.getTime())
        );
        localStorage.setItem(StorageKeys.TOKEN_KEY, data.token);
        if (data.user?.roles) {
          this.roles = data.user.roles;
        }
      }
    } else {
      localStorage.removeItem(StorageKeys.EXPIRES_KEY);
      localStorage.removeItem(StorageKeys.TOKEN_KEY);
      localStorage.removeItem(StorageKeys.USER_KEY);
    }
  }

  public async isGranted(
    roles: string[],
    requiredAll: boolean = false
  ): Promise<boolean> {
    if (!roles.length) return true;
    if (this.roles == undefined) {
      await lastValueFrom(this.user());
    }
    if (!this.roles) return false;
    if (requiredAll) {
      for (let index = 0; index < this.roles.length; index++) {
        const role = this.roles[index];
        if (!this.roles.includes(role)) return false;
      }
      return true;
    } else {
      for (let index = 0; index < roles.length; index++) {
        const role = roles[index];
        if (this.roles.includes(role)) return true;
      }
    }
    return false;
  }

  public isLoggedIn(): boolean {
    return this.getToken() != null;
  }
}
