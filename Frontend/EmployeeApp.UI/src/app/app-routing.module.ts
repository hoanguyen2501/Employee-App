import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyAddComponent } from './components/company/company-add/company-add.component';
import { CompanyDetailsComponent } from './components/company/company-details/company-details.component';
import { CompanyEditComponent } from './components/company/company-edit/company-edit.component';
import { CompanyListComponent } from './components/company/company-list/company-list.component';
import { EmployeeAddComponent } from './components/employee/employee-add/employee-add.component';
import { EmployeeDetailsComponent } from './components/employee/employee-details/employee-details.component';
import { EmployeeEditComponent } from './components/employee/employee-edit/employee-edit.component';
import { EmployeeListComponent } from './components/employee/employee-list/employee-list.component';
import { PendingChangesGuard } from './guards/pending-changes.guard';

const routes: Routes = [
  { path: 'employee', component: EmployeeListComponent },
  {
    path: 'employee/add',
    component: EmployeeAddComponent,
    canDeactivate: [PendingChangesGuard],
  },
  {
    path: 'employee/edit/:id',
    component: EmployeeEditComponent,
    canDeactivate: [PendingChangesGuard],
  },
  { path: 'employee/:id', component: EmployeeDetailsComponent },
  { path: 'company', component: CompanyListComponent },
  {
    path: 'company/add',
    component: CompanyAddComponent,
    canDeactivate: [PendingChangesGuard],
  },
  {
    path: 'company/edit/:id',
    component: CompanyEditComponent,
    canDeactivate: [PendingChangesGuard],
  },
  {
    path: 'company/:id',
    component: CompanyDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
