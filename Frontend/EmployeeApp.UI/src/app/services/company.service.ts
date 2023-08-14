import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Company } from '../models/company';

@Injectable({
  providedIn: 'root',
})
export class CompanyService {
  baseUrl: string = environment.baseUrl;

  constructor(private http: HttpClient) {}

  getCompanies(): Observable<Company[]> {
    return this.http.get<Company[]>(this.baseUrl + 'company');
  }

  getCompany(companyId: string): Observable<Company> {
    return this.http.get<Company>(this.baseUrl + `company/${companyId}`);
  }
}
