import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { TokenStorageService } from '../services/token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private tokenService: TokenStorageService,
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
          // let isRefreshed = true;
          // const jwtHelper = new JwtHelperService();
          // if (jwtHelper.isTokenExpired(user.accessToken)) {
          //   this.authService.refresh().subscribe({
          //     next: (user: AuthAppUser) => {
          //       isRefreshed = true;
          //       this.tokenService.storeUser(user);
          //       this.authService.setCurrentUser(user);
          //     },
          //     error: () => {
          //       this.toastr.error(
          //         'Your session is expired, please login again to continue!'
          //       );
          //       this.authService.logout();
          //       isRefreshed = false;
          //     },
          //   });
          // }

          // return isRefreshed;
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
