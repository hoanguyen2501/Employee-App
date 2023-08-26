import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Observable, filter, of } from 'rxjs';
import { AuthAppUser } from './models/AppUser/authAppUser';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'EmployeeApp.UI';
  currentUser$: Observable<AuthAppUser | null> = of(null);

  constructor(private authService: AuthService, private router: Router) {
    this.router.events
      .pipe(filter((rs): rs is NavigationEnd => rs instanceof NavigationEnd))
      .subscribe((events) => {
        if (events.id === 1 && events.urlAfterRedirects == events.url) {
          this.authService.autoLogout();
        }
      });
  }

  ngOnInit(): void {
    this.currentUser$ = this.authService.currentUser$;
    this.setCurrentUser();
  }

  setCurrentUser(): void {
    const userString = localStorage.getItem('user');

    if (!userString) return;

    const user: AuthAppUser = JSON.parse(userString);
    this.authService.setCurrentUser(user);
  }
}
