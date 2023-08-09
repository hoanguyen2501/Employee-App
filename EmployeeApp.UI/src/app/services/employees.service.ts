import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Employee } from '../models/employee/employee';
import { DateFormatService } from './date-format.service';

@Injectable({
  providedIn: 'root',
})
export class EmployeesService {
  baseUrl: string = environment.baseUrl;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(
    private http: HttpClient,
    private dateFormatter: DateFormatService
  ) {}

  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.baseUrl + 'employee').pipe(
      map((employees) => {
        employees?.forEach((employee) => {
          employee.dateOfBirth = this.dateFormatter.formatDate(
            employee.dateOfBirth
          );
          employee.hiredAt = this.dateFormatter.formatDate(employee.hiredAt);
        });
        return employees;
      })
    );
  }

  getEmployee(id: string): Observable<Employee> {
    return this.http.get<Employee>(this.baseUrl + `employee/${id}`).pipe(
      map((employee) => {
        employee.dateOfBirth = this.dateFormatter.formatDate(
          employee.dateOfBirth
        );
        employee.hiredAt = this.dateFormatter.formatDate(employee.hiredAt);
        return employee;
      })
    );
  }

  addEmployee(data: any): Observable<any> {
    return this.http.post(this.baseUrl + 'employee/add', data);
  }

  updateMember(id: string, data: any): Observable<Employee> {
    return this.http
      .put<Employee>(this.baseUrl + `employee/edit/${id}`, data)
      .pipe(
        map((employee) => {
          employee.dateOfBirth = this.dateFormatter.formatDate(
            employee.dateOfBirth
          );
          employee.hiredAt = this.dateFormatter.formatDate(employee.hiredAt);
          return employee;
        })
      );
  }

  deleteEmployee(id: string): Observable<any> {
    return this.http.delete(this.baseUrl + `employee/delete/${id}`);
  }
}
