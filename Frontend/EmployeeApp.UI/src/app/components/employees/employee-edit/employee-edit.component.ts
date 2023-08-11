import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import {
  ComponentCanDeactivate,
  PendingChangesGuard,
} from 'src/app/guards/pending-changes.guard';
import { Employee } from 'src/app/models/employee/employee';
import { CompaniesService } from 'src/app/services/companies.service';
import { EmployeesService } from 'src/app/services/employees.service';

@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css'],
})
export class EmployeeEditComponent implements OnInit, PendingChangesGuard {
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.employeeEditForm.dirty) {
      $event.returnValue = true;
    }
  }

  employeeEditForm: FormGroup = new FormGroup({});
  genders = [
    { title: 'Female', value: true },
    { title: 'Male', value: false },
  ];

  companyId: string = '';
  companies: { companyId: string; companyName: string }[] = [];
  employee: Employee | undefined;
  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeesService,
    private companyService: CompaniesService,
    private toastr: ToastrService,
    private formBuilder: FormBuilder
  ) {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');

        if (id) {
          this.loadEmployee(id);
          this.loadAllCompanies();
        }
      },
    });
  }

  canDeactivate(
    component: ComponentCanDeactivate
  ): boolean | Observable<boolean> {
    if (this.employeeEditForm.dirty) return false;

    return true;
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.employeeEditForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.minLength]],
      lastName: ['', [Validators.required, Validators.minLength]],
      dateOfBirth: ['', [Validators.required]],
      gender: [null, [Validators.required]],
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
      address: ['', [Validators.required, Validators.minLength]],
      city: ['', [Validators.required, Validators.minLength]],
      country: ['', [Validators.required, Validators.minLength]],
      companyId: [null, [Validators.required]],
    });
  }

  loadEmployee(id: string) {
    this.employeeService.getEmployee(id).subscribe({
      next: (employee) => {
        this.employee = employee;
        this.updateEmployeeEditForm(employee);
      },
      error: (error) => {
        this.toastr.success('Failed to load employee!', 'Failure');
      },
    });
  }

  updateEmployee(id: string): void {
    console.log(this.employeeEditForm.value);
    this.employeeService
      .updateMember(id, this.employeeEditForm.value)
      .subscribe({
        next: (employee) => {
          this.employee = employee;
          this.employeeEditForm.reset(employee);
          this.toastr.success('Update successfully!', 'Success');
        },
        error: (error) => {
          this.toastr.success('Failed to update!', 'Failure');
        },
      });
  }

  updateEmployeeEditForm(employee: Employee) {
    this.employeeEditForm.patchValue({
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

  loadAllCompanies() {
    this.companyService.getAllCompanies().subscribe({
      next: (companies) =>
        (this.companies = companies.map((company) => {
          return { companyId: company.id, companyName: company.companyName };
        })),
      error: (error) => this.toastr.error('Failed to load companies', 'Error'),
    });
  }
}
