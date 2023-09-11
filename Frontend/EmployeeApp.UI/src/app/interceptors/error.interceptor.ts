import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private authService: AuthService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 400:
              this.router.navigateByUrl('/bad-request');
              break;
            case 401:
              if (
                request.url.split('/').pop()?.toLocaleLowerCase() === 'refresh'
              )
                this.authService.logout();
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              this.router.navigateByUrl('/server-error');
              break;
            default:
              break;
          }
        }
        throw error;
      })
    );
  }
}
