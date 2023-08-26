import { Injectable } from '@angular/core';
import { AuthAppUser } from '../models/AppUser/authAppUser';

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

  storeUser(user: AuthAppUser): void {
    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('access_token', user.accessToken);
  }
}
