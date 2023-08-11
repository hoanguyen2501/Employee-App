import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Company } from 'src/app/models/companies/company';
import { CompaniesService } from 'src/app/services/companies.service';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css'],
})
export class CompaniesListComponent implements OnInit {
  companies: Company[] = [];
  totalPages: number = 1;
  activatedPage: number = 1;

  constructor(
    private companyService: CompaniesService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadAllCompanies();
  }

  paging(pageNumber: number) {}

  numSequence(page: number): Array<number> {
    return Array(page)
      .fill(0)
      .map((value, index) => index + 1);
  }

  loadAllCompanies(): void {
    this.companyService.getAllCompanies().subscribe({
      next: (companies) => {
        this.companies = companies;
        this.toastr.success('Load all companies successfully!', 'Success');
      },
      error: (error) => {
        this.toastr.error('Failed to load companies', 'Failure');
      },
    });
  }

  addCompanyBtn(): void {
    this.router.navigateByUrl('companies/add');
  }

  companyDetailsBtn(id: string): void {
    this.router.navigateByUrl(`companies/${id}`);
  }

  deleteCompany(id: string, $event: any): void {
    $event.stopPropagation();
    const isDeleted = confirm('Are you sure to delete this record?');

    if (isDeleted) {
      this.companyService.deleteEmployee(id).subscribe({
        next: (response) => {
          this.companies = this.companies.filter((item) => {
            return item.id !== id;
          });
          this.toastr.success('Delete successfully!', 'Success');
        },
        error: (error) => {
          this.toastr.error('Failed to delete company!', 'Error');
        },
      });
    }
  }
}
