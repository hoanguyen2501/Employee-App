import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Employee } from '../models/employee';
import { DateFormatService } from './date-format.service';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  baseUrl: string = environment.baseUrl;

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

  editEmployee(employeeId: string, data: any): Observable<Employee> {
    return this.http.put<Employee>(
      this.baseUrl + `employee/edit/${employeeId}`,
      data
    );
  }

  deleteEmployee(employeeId: string): Observable<any> {
    return this.http.delete(this.baseUrl + `employee/delete/${employeeId}`);
  }
}
