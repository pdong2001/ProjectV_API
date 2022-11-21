import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(private auth: AuthService, private router: Router) {}
  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    var roles = childRoute.data['Roles'] as string[] | undefined;
    if (this.auth.isLoggedIn()) {
      if (roles?.length) {
        return this.auth.isGranted(roles).then((r) => {
          if (!r) {
            this.router.navigateByUrl('/');
          }
          return r;
        });
      }
      return true;
    } else {
      this.router.navigateByUrl('/');
    }
    return false;
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (this.auth.isLoggedIn()) {
      return true;
    } else {
      this.router.navigateByUrl('/auth/login');
    }
    return false;
  }
}
