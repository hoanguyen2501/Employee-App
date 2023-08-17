import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { UserLogin } from './models/AppUser/userLogin';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'EmployeeApp.UI';
  currentUser$: Observable<UserLogin | null> = of(null);

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.currentUser$ = this.authService.currentUser$;
    this.setCurrentUser();
  }

  setCurrentUser(): void {
    const userString = localStorage.getItem('user');

    if (!userString) return;

    const user: UserLogin = JSON.parse(userString);
    this.authService.setCurrentUser(user);
  }
}
