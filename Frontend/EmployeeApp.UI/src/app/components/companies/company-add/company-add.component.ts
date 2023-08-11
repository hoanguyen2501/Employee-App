import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import {
  ComponentCanDeactivate,
  PendingChangesGuard,
} from 'src/app/guards/pending-changes.guard';
import { CompaniesService } from 'src/app/services/companies.service';

@Component({
  selector: 'app-company-add',
  templateUrl: './company-add.component.html',
  styleUrls: ['./company-add.component.css'],
})
export class CompanyAddComponent implements OnInit, PendingChangesGuard {
  companyForm: FormGroup = new FormGroup({});

  constructor(
    private companyService: CompaniesService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {}

  canDeactivate(
    component: ComponentCanDeactivate
  ): boolean | Observable<boolean> {
    if (this.companyForm.dirty) return false;

    return true;
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.companyForm = this.formBuilder.group({
      companyName: ['', [Validators.required, Validators.minLength(5)]],
      establishedAt: ['', [Validators.required, Validators.minLength(5)]],
      email: [
        '',
        [Validators.required, Validators.minLength(10), Validators.email],
      ],
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.minLength(9),
          Validators.maxLength(20),
        ],
      ],
      address: ['', [Validators.required, Validators.minLength(3)]],
      city: ['', [Validators.required, Validators.minLength(3)]],
      country: ['', [Validators.required, Validators.minLength(3)]],
    });
  }

  addCompany(): void {
    this.companyService.createCompany(this.companyForm.value).subscribe({
      next: (response) => {
        this.companyForm.reset();
        this.router.navigateByUrl(`/companies/${response.id}`);
      },
      error: (error) => {
        this.toastr.error('Failed to add new company', 'Error');
      },
    });
  }
}
