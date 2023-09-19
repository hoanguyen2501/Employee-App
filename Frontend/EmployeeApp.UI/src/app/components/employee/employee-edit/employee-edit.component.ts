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
import { Employee } from 'src/app/models/employee';
import { CompanyService } from 'src/app/services/company.service';
import { DateFormatService } from 'src/app/services/date-format.service';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css'],
})
export class EmployeeEditComponent implements OnInit, PendingChangesGuard {
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.employeeForm.dirty) {
      $event.returnValue = true;
    }
  }
  employeeForm: FormGroup = new FormGroup({});
  @ViewChild(FormGroupDirective, { static: false })
  formGroupDirective: any = FormGroupDirective;
  employee: Employee | undefined;
  employeeId: string = '';

  genders = [
    { title: 'Female', value: true },
    { title: 'Male', value: false },
  ];

  companies = [{ id: '', companyName: '' }];

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private companyService: CompanyService,
    private dateFormatter: DateFormatService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.initialForm();
    this.loadCompanies();
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          this.employeeId = id;
          this.loadEmployee(this.employeeId);
        }
      },
    });
  }

  canDeactivate(
    component: ComponentCanDeactivate
  ): boolean | Observable<boolean> {
    if (this.employeeForm.dirty) return false;

    return true;
  }

  initialForm() {
    this.employeeForm = this.formBuilder.group({
      companyId: [null, Validators.required],
      firstName: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z]+$/),
          Validators.minLength(4),
          Validators.maxLength(100),
        ],
      ],
      lastName: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z]+$/),
          Validators.minLength(4),
          Validators.maxLength(100),
        ],
      ],
      gender: [null, Validators.required],
      dateOfBirth: ['', Validators.required],
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
          Validators.maxLength(100),
        ],
      ],
      country: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(100),
          Validators.pattern(/^[A-Za-z]+$/),
        ],
      ],
      email: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[\d\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/),
          Validators.maxLength(50),
          Validators.email,
        ],
      ],
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[\d]*$/),
          Validators.minLength(10),
          Validators.maxLength(12),
        ],
      ],
    });
  }

  onSubmit(): void {
    if (this.employeeForm.dirty && this.employeeForm.valid) {
      this.employeeForm.value['dateOfBirth'] = new Date(
        this.dateFormatter.setDate(this.employeeForm.value['dateOfBirth'])
      );
      this.employeeService
        .editEmployee(this.employeeId, this.employeeForm.value)
        .subscribe({
          next: (updatedEmployee) => {
            this.employee = updatedEmployee;
            this.employeeForm.reset(updatedEmployee);
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
    if (this.employeeForm.dirty || this.employeeForm.touched) {
      const isReset = confirm(
        'WARNING: You have unsaved changes. Press Cancel to go back and save these changes, or OK to lose these changes.'
      );

      if (isReset) {
        this.employeeForm.reset();
        this.formGroupDirective.resetForm(this.employee);
      }
    }
  }

  loadEmployee(employeeId: string): void {
    this.employeeService.getEmployee(employeeId).subscribe({
      next: (employee) => {
        this.employee = employee;
        this.updateEditForm(employee);
      },
      error: (error) => console.log(error),
    });
  }

  updateEditForm(employee: Employee): void {
    this.employeeForm.patchValue({
      firstName: employee.firstName,
      lastName: employee.lastName,
      dateOfBirth: employee.dateOfBirth,
      gender: employee.gender,
      email: employee.email,
      phoneNumber: employee.phoneNumber,
      address: employee.address,
      city: employee.city,
      country: employee.country,
      companyId: employee.companyId,
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
}
