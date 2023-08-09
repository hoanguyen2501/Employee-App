import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import {
  ComponentCanDeactivate,
  PendingChangesGuard,
} from 'src/app/guards/pending-changes.guard';
import { Company } from 'src/app/models/companies/company';
import { CompaniesService } from 'src/app/services/companies.service';

@Component({
  selector: 'app-company-edit',
  templateUrl: './company-edit.component.html',
  styleUrls: ['./company-edit.component.css'],
})
export class CompanyEditComponent implements OnInit, PendingChangesGuard {
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.companyEditForm.dirty) {
      $event.returnValue = true;
    }
  }

  companyEditForm: FormGroup = new FormGroup({});
  company: Company | undefined;

  constructor(
    private companyService: CompaniesService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          this.loadCompany(id);
        }
      },
    });
  }

  canDeactivate(
    component: ComponentCanDeactivate
  ): boolean | Observable<boolean> {
    if (this.companyEditForm.dirty) return false;
    return true;
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.companyEditForm = this.formBuilder.group({
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
      address: ['', [Validators.required, Validators.minLength(5)]],
      city: ['', [Validators.required, Validators.minLength(5)]],
      country: ['', [Validators.required, Validators.minLength(3)]],
    });
  }

  loadCompany(id: string) {
    this.companyService.getCompany(id).subscribe({
      next: (company) => {
        this.company = company;
        this.updateCompanyEditForm(company);
      },
      error: (error) => {
        this.toastr.error('Failed to load company', 'Error');
      },
    });
  }

  updateCompany(id: string) {
    this.companyService
      .updateCompany(id, this.companyEditForm.value)
      .subscribe({
        next: (company) => {
          this.company = company;
          this.companyEditForm.reset(company);
          this.toastr.success('Update successfully', 'Success');
        },
        error: (error) => {
          this.toastr.error('Failed to update company', 'Error');
        },
      });
  }

  updateCompanyEditForm(company: Company) {
    this.companyEditForm.patchValue({
      companyName: company.companyName,
      establishedAt: company.establishedAt,
      email: company.email,
      phoneNumber: company.phoneNumber,
      address: company.address,
      city: company.city,
      country: company.country,
    });
  }
}
