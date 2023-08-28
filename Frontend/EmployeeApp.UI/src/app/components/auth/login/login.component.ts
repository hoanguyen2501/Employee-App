import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.authService.currentUser$.subscribe({
      next: (user) => {
        if (user) {
          this.router.navigateByUrl('');
        }
      },
    });
  }

  ngOnInit(): void {
    this.initialize();
  }

  initialize(): void {
    this.loginForm = this.formBuilder.group({
      username: [null, [Validators.required]],
      password: [null, [Validators.required]],
    });
  }

  onSubmit(): void {
    this.authService.login(this.loginForm.value).subscribe({
      next: (user) => {
        this.toastr.success('Logged in successfully', 'Login');
        this.authService.autoLogout();
        this.router.navigate(['']);
      },
      error: (error) => {
        this.toastr.error(error.error, 'Failure');
      },
    });
  }
}
