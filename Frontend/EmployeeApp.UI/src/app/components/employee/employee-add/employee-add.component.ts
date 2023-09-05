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
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-employee-add',
  templateUrl: './employee-add.component.html',
  styleUrls: ['./employee-add.component.css'],
})
export class EmployeeAddComponent implements OnInit, PendingChangesGuard {
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.employeeForm.dirty) {
      $event.returnValue = true;
    }
  }
  @ViewChild(FormGroupDirective, { static: false })
  formGroupDirective: any = FormGroupDirective;
  employeeForm: FormGroup = new FormGroup({});
  submitted: boolean = false;

  genders = [
    { title: 'Female', value: true },
    { title: 'Male', value: false },
  ];
  companies = [{ id: '', companyName: '' }];

  constructor(
    private formBuilder: FormBuilder,
    private companyService: CompanyService,
    private employeeService: EmployeeService,
    private dateFormatter: DateFormatService,
    private toastr: ToastrService
  ) {}

  canDeactivate(
    component: ComponentCanDeactivate
  ): boolean | Observable<boolean> {
    if (this.employeeForm.dirty) return false;

    return true;
  }

  ngOnInit(): void {
    this.initialForm();
    this.loadCompanies();
  }

  onSubmit(): void {
    if (this.employeeForm.dirty && this.employeeForm.valid) {
      this.employeeForm.value['dateOfBirth'] = new Date(
        this.dateFormatter.setDate(this.employeeForm.value['dateOfBirth'])
      );
      this.employeeService.addEmployee(this.employeeForm.value).subscribe({
        next: (response) => {
          this.employeeForm.reset();
          this.formGroupDirective.resetForm();
          this.toastr.success('Created successfully');
        },
        error: (error) => {
          this.toastr.error('Failed to create');
          console.log(error);
        },
      });
    } else {
      return;
    }
  }

  initialForm() {
    this.employeeForm = this.formBuilder.group({
      companyId: [null, Validators.required],
      firstName: [
        '',
        [Validators.required, , Validators.pattern(/^[A-Za-z]+$/)],
      ],
      lastName: [
        '',
        [Validators.required, , Validators.pattern(/^[A-Za-z]+$/)],
      ],
      gender: [null, Validators.required],
      dateOfBirth: ['', Validators.required],
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

  loadCompanies(): void {
    this.companyService.getCompanies().subscribe({
      next: (companies) => {
        this.companies = companies.map((company) => {
          return { id: company.id, companyName: company.companyName };
        });
      },
    });
  }

  onReset(): void {
    if (this.employeeForm.dirty) {
      const isReset = confirm(
        'WARNING: You have unsaved changes. Press Cancel to go back and save these changes, or OK to lose these changes.'
      );

      if (isReset) {
        this.employeeForm.reset();
        this.formGroupDirective.resetForm();
      }
    }
  }
}
