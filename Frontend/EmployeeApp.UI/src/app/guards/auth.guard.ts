import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable, map } from 'rxjs';
import { UserLogin } from '../models/AppUser/userLogin';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  user: UserLogin | undefined;
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
          if (jwtHelper.isTokenExpired(user.token)) {
            this.authService.logout();
            this.toastr.warning(
              'Your session is expired. Please login agin!',
              'Warning'
            );
            this.router.navigate(['/login']);
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
