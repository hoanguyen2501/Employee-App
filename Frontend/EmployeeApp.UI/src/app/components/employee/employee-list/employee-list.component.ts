import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css'],
})
export class EmployeeListComponent implements OnInit, AfterViewInit {
  employees: any[] = [];

  displayedColumns = [
    'fullName',
    // 'lastName',
    'dateOfBirth',
    'gender',
    'address',
    // 'city',
    // 'country',
    'email',
    'phoneNumber',
    'hiredAt',
    'action',
  ];
  dataSource: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator: any = MatPaginator;

  constructor(
    private employeeService: EmployeeService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.dataSource = new MatTableDataSource<any>(this.employees);
  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  loadEmployees(): void {
    this.employeeService.getAllEmployees().subscribe({
      next: (employees) => {
        this.employees = employees.map((employee) => {
          return {
            ...employee,
            gender: employee.gender ? 'Female' : 'Male',
          };
        });
        this.dataSource.data = this.employees;
      },
      error: (error) => console.log(error),
    });
  }

  gotoAddingPage() {
    this.router.navigateByUrl('/employee/add');
  }

  gotoDetail(employeeId: string) {
    this.router.navigateByUrl(`/employee/${employeeId}`);
  }

  onAction(action: string, employeeId: string, $event: any) {
    switch (action) {
      case 'update':
        this.router.navigateByUrl(`/employee/edit/${employeeId}`);
        break;
      case 'delete':
        $event.stopPropagation();
        const isDeleted = confirm('Are you sure to delete this record?');
        if (isDeleted) {
          this.employeeService.deleteEmployee(employeeId).subscribe({
            next: (response) => {
              this.dataSource.data = this.employees.filter((item) => {
                return item.id !== employeeId;
              });
              this.toastr.success('Delete successfully', 'Success');
            },
            error: (error) => {
              console.log(error);
              this.toastr.error('Failed to delete', 'Error');
            },
          });
        }
        break;

      default:
        break;
    }
  }

  camelCaseToWords(s: string) {
    const result = s.replace(/([A-Z])/g, ' $1');
    return result.charAt(0).toUpperCase() + result.slice(1);
  }

  openDialog(action: string, row: any) {}
}
