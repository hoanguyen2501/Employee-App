import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Company } from 'src/app/models/company';
import { CompanyService } from 'src/app/services/company.service';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css'],
})
export class CompanyListComponent implements OnInit, AfterViewInit {
  companies: Company[] = [];
  dataSource: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: any = MatPaginator;
  displayedColumns = [
    'companyName',
    'address',
    'establishedAt',
    'email',
    'phoneNumber',
    'action',
  ];
  constructor(
    private companyService: CompanyService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.dataSource = new MatTableDataSource<any>(this.companies);
  }

  ngOnInit(): void {
    this.loadCompanies();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  loadCompanies(): void {
    this.companyService.getCompanies().subscribe({
      next: (companies) => {
        this.companies = companies;
        this.dataSource.data = companies;
      },
      error: (error) => console.log(error),
    });
  }

  gotoAddingPage() {
    this.router.navigateByUrl('/company/add');
  }

  gotoDetail(employeeId: string) {
    this.router.navigateByUrl(`/company/${employeeId}`);
  }

  onAction(action: string, employeeId: string, $event: any) {
    switch (action) {
      case 'update':
        this.router.navigateByUrl(`/company/edit/${employeeId}`);
        break;
      case 'delete':
        $event.stopPropagation();
        const isDeleted = confirm('Are you sure to delete this record?');
        if (isDeleted) {
          this.companyService.deleteCompany(employeeId).subscribe({
            next: (response) => {
              this.dataSource.data = this.companies.filter((item) => {
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
}
