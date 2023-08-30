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
import { BehaviorSubject, Observable, map, of } from 'rxjs';
import { AuthAppUser } from '../models/AppUser/authAppUser';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

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
          let isRefreshed = true;
          const jwtHelper = new JwtHelperService();
          if (jwtHelper.isTokenExpired(user.accessToken)) {
            this.authService.refresh().subscribe({
              next: (user) => {
                isRefreshed = true;
              },
              error: () => {
                this.toastr.error(
                  'Your session is expired, please login again to continue!'
                );
                this.authService.logout();
                isRefreshed = false;
              },
            });
          }

          return isRefreshed;
        } else {
          this.router.navigate(['/login']);
          return false;
        }
      })
    );

    return isAuthenticated;
  }
}
