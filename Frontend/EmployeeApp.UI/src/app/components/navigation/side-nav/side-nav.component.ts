import { MediaMatcher } from '@angular/cdk/layout';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AuthAppUser } from 'src/app/models/AppUser/authAppUser';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.css'],
})
export class SideNavComponent implements OnInit, OnDestroy {
  mobileQuery: MediaQueryList;
  currentUser$: Observable<AuthAppUser | null> = of(null);

  fillerNav = ['Company', 'Employee'];

  private _mobileQueryListener: () => void;

  constructor(
    changeDetectorRef: ChangeDetectorRef,
    media: MediaMatcher,
    private authService: AuthService
  ) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addEventListener('change', this._mobileQueryListener);
    this.currentUser$ = this.authService.currentUser$;
  }

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this.mobileQuery.removeEventListener('change', this._mobileQueryListener);
  }

  logout() {
    this.authService.logout();
  }
}
