import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthAppUser } from '../models/AppUser/authAppUser';
import { AuthService } from '../services/auth.service';
import { TokenStorageService } from '../services/token-storage.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private tokenStorage: TokenStorageService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const token = this.tokenStorage.getToken();
    if (token) {
      const authRequest = this.setAuthHeader(request, token);
      return next.handle(authRequest).pipe(
        catchError((error: HttpErrorResponse) => {
          if (
            error.status === 401 &&
            request.url.split('/').pop() !== 'refresh'
          ) {
            return this.authService.refresh().pipe(
              switchMap((response: AuthAppUser) => {
                this.tokenStorage.storeUser(response);
                this.authService.setCurrentUser(response);
                this.router.navigateByUrl(this.router.url);
                const newAuthRequest = this.setAuthHeader(
                  request,
                  response.accessToken
                );
                return next.handle(newAuthRequest);
              }),
              catchError((error: HttpErrorResponse) => {
                this.authService.logout();
                this.toastr.warning(error.message);
                return throwError(() => error);
              })
            );
          } else {
            return throwError(() => error);
          }
        })
      );
    }
    return next.handle(request);
  }

  private setAuthHeader(
    request: HttpRequest<unknown>,
    accessToken: string
  ): HttpRequest<unknown> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
      withCredentials: true,
    });
  }
}
