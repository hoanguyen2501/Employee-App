import { HttpErrorResponse } from '@angular/common/http';
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
import { DateFormatService } from 'src/app/services/date-format.service';

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
    private toastr: ToastrService,
    private dateFormatter: DateFormatService
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
      companyName: ['', [Validators.required, Validators.minLength(4)]],
      establishedAt: ['', [Validators.required]],
      address: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(100),
        ],
      ],
      city: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      country: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      email: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[\d\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/),
          Validators.email,
        ],
      ],
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.minLength(10),
          Validators.pattern(/^[\d]*$/),
          Validators.maxLength(12),
        ],
      ],
    });
  }

  onSubmit(): void {
    if (this.companyForm.dirty && this.companyForm.valid) {
      this.companyForm.value['establishedAt'] = new Date(
        this.dateFormatter.setDate(this.companyForm.value['establishedAt'])
      );

      this.companyService
        .editCompany(this.companyId, this.companyForm.value)
        .subscribe({
          next: (updatedCompany: Company) => {
            this.company = updatedCompany;
            this.companyForm.reset(updatedCompany);
            this.toastr.success('Updated successfully');
          },
          error: (error: HttpErrorResponse) => {
            this.toastr.error(error.error);
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
