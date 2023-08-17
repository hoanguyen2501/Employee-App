import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserLogin } from '../models/AppUser/userLogin';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl: string = environment.baseUrl;
  private currentUserSource = new BehaviorSubject<UserLogin | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(
    private http: HttpClient,
    private tokenService: TokenStorageService
  ) {}

  login(loginForm: any) {
    return this.http
      .post<UserLogin>(this.baseUrl + 'auth/login', loginForm)
      .pipe(
        map((response: UserLogin) => {
          const user = response;
          if (user) {
            this.tokenService.storeUser(user);
            this.setCurrentUser(user);
          }
        })
      );
  }

  logout() {
    this.tokenService.clearStorage();
    this.currentUserSource.next(null);
  }

  setCurrentUser(user: UserLogin) {
    this.currentUserSource.next(user);
  }
}
