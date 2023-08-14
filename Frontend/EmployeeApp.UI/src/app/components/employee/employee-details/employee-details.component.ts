import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Employee } from 'src/app/models/employee';
import { CompanyService } from 'src/app/services/company.service';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-employee-details',
  templateUrl: './employee-details.component.html',
  styleUrls: ['./employee-details.component.css'],
})
export class EmployeeDetailsComponent implements OnInit {
  employeeForm: FormGroup = new FormGroup({});
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

  initialForm() {
    this.employeeForm = this.formBuilder.group({
      companyId: [{ value: null, disabled: true }, Validators.required],
      firstName: [{ value: '', disabled: true }, Validators.required],
      lastName: [{ value: '', disabled: true }, Validators.required],
      gender: [{ value: null, disabled: true }, Validators.required],
      dateOfBirth: [{ value: '', disabled: true }, Validators.required],
      address: [{ value: '', disabled: true }, Validators.required],
      city: [{ value: '', disabled: true }, Validators.required],
      country: [{ value: '', disabled: true }, Validators.required],
      email: [{ value: '', disabled: true }, Validators.required],
      phoneNumber: [{ value: '', disabled: true }, Validators.required],
    });
  }

  onSubmit(): void {
    if (this.employeeForm.dirty && this.employeeForm.valid) {
      this.employeeService
        .editEmployee(this.employeeId, this.employeeForm.value)
        .subscribe({
          next: (updatedEmployee) => {
            this.employee = updatedEmployee;
            this.employeeForm.reset(updatedEmployee);
            this.toastr.success('Updated successfully', 'Success');
          },
          error: (error) => {
            this.toastr.error('Failed to update', 'Failure');
            console.log(error);
          },
        });
    } else {
      return;
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
