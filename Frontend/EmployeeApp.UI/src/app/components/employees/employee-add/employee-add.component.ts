import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import {
  ComponentCanDeactivate,
  PendingChangesGuard,
} from 'src/app/guards/pending-changes.guard';
import { CompaniesService } from 'src/app/services/companies.service';
import { EmployeesService } from 'src/app/services/employees.service';

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
  employeeForm: FormGroup = new FormGroup({});
  companies: { companyId: string; companyName: string }[] = [];
  genders = [
    { title: 'Female', value: true },
    { title: 'Male', value: false },
  ];

  constructor(
    private employeeService: EmployeesService,
    private companyService: CompaniesService,
    private formBuilder: FormBuilder,
    private router: Router,
    private toastr: ToastrService
  ) {}

  canDeactivate(
    component: ComponentCanDeactivate
  ): boolean | Observable<boolean> {
    if (this.employeeForm.dirty) return false;

    return true;
  }

  ngOnInit(): void {
    this.loadAllEmployee();
    this.initializeForm();
  }

  initializeForm() {
    this.employeeForm = this.formBuilder.group({
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
      address: ['', [Validators.required, Validators.minLength(3)]],
      city: ['', [Validators.required, Validators.minLength(3)]],
      country: ['', [Validators.required, Validators.minLength(3)]],
      companyId: [null, [Validators.required]],
    });
  }

  addEmployee() {
    this.employeeService.addEmployee(this.employeeForm.value).subscribe({
      next: (response) => {
        this.employeeForm.reset();
        this.router.navigateByUrl(`/employees/${response.id}`);
      },
      error: (error) => {
        this.toastr.success('Failed to add new employee!', 'Failure');
      },
    });
  }

  loadAllEmployee() {
    this.companyService.getAllCompanies().subscribe({
      next: (companies) => {
        this.companies = companies.map((company) => {
          return { companyId: company.id, companyName: company.companyName };
        });
      },
    });
  }
}
