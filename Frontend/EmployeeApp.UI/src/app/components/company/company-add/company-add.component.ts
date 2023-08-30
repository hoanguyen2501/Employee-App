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
    private toastr: ToastrService
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
      this.companyService.addCompany(this.companyForm.value).subscribe({
        next: (response) => {
          this.companyForm.reset();
          this.formGroupDirective.resetForm();
          this.toastr.success('Created successfully', 'Success');
        },
        error: (error) => {
          this.toastr.error('Failed to create', 'Failure');
          console.log(error);
        },
      });
    } else {
      return;
    }
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

  onReset(): void {
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
