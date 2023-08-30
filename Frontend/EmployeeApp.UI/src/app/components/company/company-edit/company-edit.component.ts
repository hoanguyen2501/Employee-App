import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormGroupDirective,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import {
  ComponentCanDeactivate,
  PendingChangesGuard,
} from 'src/app/guards/pending-changes.guard';
import { Company } from 'src/app/models/company';
import { CompanyService } from 'src/app/services/company.service';

@Component({
  selector: 'app-company-edit',
  templateUrl: './company-edit.component.html',
  styleUrls: ['./company-edit.component.css'],
})
export class CompanyEditComponent implements OnInit, PendingChangesGuard {
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.companyForm.dirty) {
      $event.returnValue = true;
    }
  }
  companyForm: FormGroup = new FormGroup({});
  @ViewChild(FormGroupDirective, { static: false })
  formGroupDirective: any = FormGroupDirective;
  company: Company | undefined;
  companyId: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private companyService: CompanyService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.initialForm();
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          this.companyId = id;
          this.loadCompany(id);
        }
      },
    });
  }

  canDeactivate(
    component: ComponentCanDeactivate
  ): boolean | Observable<boolean> {
    if (this.companyForm.dirty) return false;

    return true;
  }

  initialForm() {
    this.companyForm = this.formBuilder.group({
      companyName: ['', Validators.required],
      establishedAt: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      email: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[\d\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/),
        ],
      ],
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.minLength(10),
          Validators.pattern(/^[\d]*$/),
        ],
      ],
    });
  }

  onSubmit(): void {
    if (this.companyForm.dirty && this.companyForm.valid) {
      this.companyService
        .editCompany(this.companyId, this.companyForm.value)
        .subscribe({
          next: (updatedCompany) => {
            this.company = updatedCompany;
            this.companyForm.reset(updatedCompany);
            this.toastr.success('Updated successfully');
          },
          error: (error) => {
            this.toastr.error('Failed to update');
            console.log(error);
          },
        });
    } else {
      return;
    }
  }

  onReset(): void {
    if (this.companyForm.dirty) {
      const isReset = confirm(
        'WARNING: You have unsaved changes. Press Cancel to go back and save these changes, or OK to lose these changes.'
      );

      if (isReset) {
        this.formGroupDirective.resetForm(this.company);
      }
    }
  }

  loadCompany(employeeId: string): void {
    this.companyService.getCompany(employeeId).subscribe({
      next: (company) => {
        this.company = company;
        this.updateEditForm(company);
      },
      error: (error) => console.log(error),
    });
  }

  updateEditForm(company: Company): void {
    this.companyForm.patchValue({
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
