import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Company } from '../models/company';
import { DateFormatService } from './date-format.service';

@Injectable({
  providedIn: 'root',
})
export class CompanyService {
  baseUrl: string = environment.baseUrl;

  constructor(
    private http: HttpClient,
    private dateFormatter: DateFormatService
  ) {}

  getCompanies(): Observable<Company[]> {
    return this.http.get<Company[]>(this.baseUrl + 'company').pipe(
      map((companies) => {
        companies.forEach((company) => {
          company.establishedAt = this.dateFormatter.formatDate(
            company.establishedAt
          );
          return company;
        });
        return companies;
      })
    );
  }

  getCompany(companyId: string): Observable<Company> {
    return this.http.get<Company>(this.baseUrl + `company/${companyId}`).pipe(
      map((company) => {
        company.establishedAt = this.dateFormatter.formatDate(
          company.establishedAt
        );
        return company;
      })
    );
  }

  addCompany(data: Company): Observable<any> {
    return this.http.post(this.baseUrl + 'company/add', data);
  }

  editCompany(companyId: string, data: Company): Observable<Company> {
    return this.http.put<Company>(
      this.baseUrl + `company/edit/${companyId}`,
      data
    );
  }

  deleteCompany(companyId: string): Observable<any> {
    return this.http.delete(this.baseUrl + `company/delete/${companyId}`);
  }
}
