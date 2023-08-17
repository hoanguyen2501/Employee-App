import { Injectable } from '@angular/core';
import { UserLogin } from '../models/AppUser/userLogin';

@Injectable({
  providedIn: 'root',
})
export class TokenStorageService {
  constructor() {}

  clearStorage(): void {
    localStorage.clear();
  }

  getToken(): string {
    var token = localStorage.getItem('access_token');
    return token ?? '';
  }

  storeUser(user: UserLogin): void {
    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('access_token', user.token);
  }

  getUser(): any {
    var user = localStorage.getItem('user');
    if (user) return JSON.parse(user);

    return null;
  }
}
