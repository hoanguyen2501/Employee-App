import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Company } from '../models/companies/company';
import { DateFormatService } from './date-format.service';

@Injectable({
  providedIn: 'root',
})
export class CompaniesService {
  baseUrl: string = environment.baseUrl;

  constructor(
    private http: HttpClient,
    private dateFormater: DateFormatService
  ) {}

  getAllCompanies(): Observable<Company[]> {
    return this.http.get<Company[]>(this.baseUrl + 'company').pipe(
      map((companies) => {
        companies?.forEach((company) => {
          company.establishedAt = this.dateFormater.formatDate(
            company.establishedAt
          );
        });
        return companies;
      })
    );
  }

  getCompany(id: string): Observable<Company> {
    return this.http.get<Company>(this.baseUrl + `company/${id}`).pipe(
      map((company) => {
        company.establishedAt = this.dateFormater.formatDate(
          company.establishedAt
        );
        return company;
      })
    );
  }

  createCompany(data: Company): Observable<any> {
    return this.http.post(this.baseUrl + 'company/add', data);
  }

  updateCompany(id: string, data: Company): Observable<Company> {
    return this.http
      .put<Company>(this.baseUrl + `company/edit/${id}`, data)
      .pipe(
        map((company) => {
          company.establishedAt = this.dateFormater.formatDate(
            company.establishedAt
          );
          return company;
        })
      );
  }

  deleteEmployee(id: string): Observable<any> {
    return this.http.delete(this.baseUrl + `company/delete/${id}`);
  }
}
