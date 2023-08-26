import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RefreshDialogComponent } from '../components/dialog/refresh-dialog/refresh-dialog.component';
import { AuthAppUser } from '../models/AppUser/authAppUser';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl: string = environment.baseUrl;
  private currentUserSource = new BehaviorSubject<AuthAppUser | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  httpOptions = {
    withCredentials: true,
  };
  jwtHelper = new JwtHelperService();
  constructor(
    private http: HttpClient,
    private router: Router,
    private tokenService: TokenStorageService,
    private dialog: MatDialog
  ) {}

  login(loginForm: any) {
    return this.http
      .post<AuthAppUser>(
        this.baseUrl + 'auth/login',
        loginForm,
        this.httpOptions
      )
      .pipe(
        map((response: AuthAppUser) => {
          const user = response;
          if (user) {
            this.tokenService.storeUser(user);
            this.setCurrentUser(user);
          }
        })
      );
  }

  refresh() {
    return this.http
      .post<AuthAppUser>(this.baseUrl + 'auth/refresh', {}, this.httpOptions)
      .pipe(
        map((response: AuthAppUser) => {
          const user = response;
          if (user) {
            this.tokenService.storeUser(user);
            this.setCurrentUser(user);
          }
        })
      );
  }

  logout(): void {
    this.http
      .post(this.baseUrl + 'auth/logout', {}, this.httpOptions)
      .subscribe();
    this.tokenService.clearStorage();
    this.currentUserSource.next(null);
    this.router.navigate(['/login']);
  }

  autoLogout(): void {
    const token = this.tokenService.getToken();
    if (token) {
      const nowAsTimestamp = new Date().getTime();
      const expireAsTimestamp = (
        this.jwtHelper.getTokenExpirationDate(token) ?? new Date()
      ).getTime();
      const diff =
        expireAsTimestamp - nowAsTimestamp < 0
          ? 0
          : expireAsTimestamp - nowAsTimestamp;

      setTimeout(() => {
        this.dialog.open(RefreshDialogComponent, {
          width: '32rem',
        });
      }, diff);
    }
  }

  setCurrentUser(user: AuthAppUser) {
    this.currentUserSource.next(user);
  }
}
