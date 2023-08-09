import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Employee } from 'src/app/models/employee/employee';
import { EmployeesService } from 'src/app/services/employees.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css'],
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  totalPages: number = 1;
  activatedPage: number = 1;

  constructor(
    private employeeService: EmployeesService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadAllEmployees();
  }

  paging(pageNumber: number) {}

  numSequence(page: number): Array<number> {
    return Array(page)
      .fill(0)
      .map((value, index) => index + 1);
  }

  loadAllEmployees() {
    this.employeeService.getAllEmployees().subscribe({
      next: (employees) => {
        this.employees = employees;
        this.toastr.success('Load all employee successfully', 'Success');
        this.totalPages += Math.floor(employees.length / 10);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  addEmployeeBtn() {
    this.router.navigateByUrl('employees/add');
  }

  employeeDetailsBtn(id: string) {
    this.router.navigateByUrl(`employees/${id}`);
  }

  deleteEmployee(id: string, $event: any) {
    $event.stopPropagation();
    const isDeleted = confirm('Are you are to delete this record?');

    if (isDeleted) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: (response) => {
          this.employees = this.employees.filter((item) => {
            return item.id !== id;
          });
          this.toastr.success('Delete employee successfully', 'Success');
        },
        error: (error) => {
          console.log(error);
          this.toastr.error('Failed to delete employee', 'Error');
        },
      });
    }
  }
}
