import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
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
  constructor(
    private http: HttpClient,
    private router: Router,
    private tokenService: TokenStorageService
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
    return this.http.post<AuthAppUser>(
      this.baseUrl + 'auth/refresh',
      {},
      this.httpOptions
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

  setCurrentUser(user: AuthAppUser) {
    this.currentUserSource.next(user);
  }
}
