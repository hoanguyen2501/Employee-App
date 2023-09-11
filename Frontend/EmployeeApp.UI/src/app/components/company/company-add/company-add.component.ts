import { HttpErrorResponse } from '@angular/common/http';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormGroupDirective,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import {
  ComponentCanDeactivate,
  PendingChangesGuard,
} from 'src/app/guards/pending-changes.guard';
import { CompanyService } from 'src/app/services/company.service';
import { DateFormatService } from 'src/app/services/date-format.service';

@Component({
  selector: 'app-company-add',
  templateUrl: './company-add.component.html',
  styleUrls: ['./company-add.component.css'],
})
export class CompanyAddComponent implements OnInit, PendingChangesGuard {
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.companyForm.dirty) {
      $event.returnValue = true;
    }
  }
  @ViewChild(FormGroupDirective, { static: false })
  formGroupDirective: any = FormGroupDirective;
  companyForm: FormGroup = new FormGroup({});

  constructor(
    private formBuilder: FormBuilder,
    private companyService: CompanyService,
    private toastr: ToastrService,
    private dateFormatter: DateFormatService
  ) {}

  canDeactivate(
    component: ComponentCanDeactivate
  ): boolean | Observable<boolean> {
    if (this.companyForm.dirty) return false;

    return true;
  }

  ngOnInit(): void {
    this.initialForm();
  }

  onSubmit(): void {
    if (this.companyForm.dirty && this.companyForm.valid) {
      this.companyForm.value['establishedAt'] = new Date(
        this.dateFormatter.setDate(this.companyForm.value['establishedAt'])
      );
      this.companyService.addCompany(this.companyForm.value).subscribe({
        next: (response) => {
          this.companyForm.reset();
          this.formGroupDirective.resetForm();
          this.toastr.success('Created successfully');
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

  onCancel(): void {
    if (this.companyForm.dirty) {
      const isReset = confirm(
        'WARNING: You have unsaved changes. Press Cancel to go back and save these changes, or OK to lose these changes.'
      );

      if (isReset) {
        this.companyForm.reset();
        this.formGroupDirective.resetForm();
      }
    }
  }
}
