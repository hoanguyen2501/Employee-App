import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyAddComponent } from './components/companies/company-add/company-add.component';
import { CompanyDetailsComponent } from './components/companies/company-details/company-details.component';
import { CompanyEditComponent } from './components/companies/company-edit/company-edit.component';
import { CompaniesListComponent } from './components/companies/company-list/company-list.component';
import { EmployeeAddComponent } from './components/employees/employee-add/employee-add.component';
import { EmployeeDetailsComponent } from './components/employees/employee-details/employee-details.component';
import { EmployeeEditComponent } from './components/employees/employee-edit/employee-edit.component';
import { EmployeeListComponent } from './components/employees/employee-list/employee-list.component';
import { HomeComponent } from './components/home/home.component';
import { PendingChangesGuard } from './guards/pending-changes.guard';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'employees',
    component: EmployeeListComponent,
  },
  {
    path: 'employees/add',
    component: EmployeeAddComponent,
    canDeactivate: [PendingChangesGuard],
  },
  {
    path: 'employees/edit/:id',
    component: EmployeeEditComponent,
    canDeactivate: [PendingChangesGuard],
  },
  {
    path: 'employees/:id',
    component: EmployeeDetailsComponent,
  },
  {
    path: 'companies',
    component: CompaniesListComponent,
  },
  {
    path: 'companies/add',
    component: CompanyAddComponent,
    canDeactivate: [PendingChangesGuard],
  },
  {
    path: 'companies/:id',
    component: CompanyDetailsComponent,
  },
  {
    path: 'companies/edit/:id',
    component: CompanyEditComponent,
    canDeactivate: [PendingChangesGuard],
  },
  {
    path: '**',
    component: HomeComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
