import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee } from 'src/app/models/employee/employee';
import { CompaniesService } from 'src/app/services/companies.service';
import { EmployeesService } from 'src/app/services/employees.service';

@Component({
  selector: 'app-employee-details',
  templateUrl: './employee-details.component.html',
  styleUrls: ['./employee-details.component.css'],
})
export class EmployeeDetailsComponent implements OnInit {
  employee: Employee | undefined;
  companyName: string = '';

  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeesService,
    private companyService: CompaniesService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          this.getEmployee(id);
        }
      },
    });
  }

  goToEditPage() {
    this.router.navigateByUrl(`employees/edit/${this.employee?.id}`);
  }

  getEmployee(id: string) {
    this.employeeService.getEmployee(id).subscribe({
      next: (data) => {
        this.employee = data;
        this.getCompanyName(data.companyId);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  getCompanyName(companyId: string) {
    this.companyService.getCompany(companyId).subscribe({
      next: (data) => {
        this.companyName = data.companyName;
      },
    });
  }
}
