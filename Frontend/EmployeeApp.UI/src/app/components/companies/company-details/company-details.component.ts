import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Company } from 'src/app/models/companies/company';
import { CompaniesService } from 'src/app/services/companies.service';

@Component({
  selector: 'app-company-details',
  templateUrl: './company-details.component.html',
  styleUrls: ['./company-details.component.css'],
})
export class CompanyDetailsComponent implements OnInit {
  company: Company | undefined;
  constructor(
    private route: ActivatedRoute,
    private companyService: CompaniesService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          const id = params.get('id');
          if (id) {
            this.getCompany(id);
          }
        }
      },
    });
  }

  getCompany(id: string) {
    this.companyService.getCompany(id).subscribe({
      next: (company) => (this.company = company),
      error: (error) => console.log(error),
    });
  }
}
