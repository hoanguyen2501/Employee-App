import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
import { AuthAppUser } from '../models/AppUser/authAppUser';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  user: AuthAppUser | undefined;
  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.user;
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const isAuthenticated = this.authService.currentUser$.pipe(
      map((user) => {
        if (user) {
          var jwtHelper = new JwtHelperService();
          if (jwtHelper.isTokenExpired(user.accessToken)) {
            this.toastr.warning(
              'Your session is expired. Please login agin!',
              'Warning'
            );
            this.authService.logout();
            return false;
          }

          return true;
        } else {
          this.router.navigate(['/login']);
          return false;
        }
      })
    );

    return isAuthenticated;
  }
}
