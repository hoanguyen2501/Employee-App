import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { CompanyAddComponent } from './components/company/company-add/company-add.component';
import { CompanyDetailsComponent } from './components/company/company-details/company-details.component';
import { CompanyEditComponent } from './components/company/company-edit/company-edit.component';
import { CompanyListComponent } from './components/company/company-list/company-list.component';
import { EmployeeAddComponent } from './components/employee/employee-add/employee-add.component';
import { EmployeeDetailsComponent } from './components/employee/employee-details/employee-details.component';
import { EmployeeEditComponent } from './components/employee/employee-edit/employee-edit.component';
import { EmployeeListComponent } from './components/employee/employee-list/employee-list.component';
import { BadRequestComponent } from './components/errors/bad-request/bad-request.component';
import { InternalServerErrorComponent } from './components/errors/internal-server-error/internal-server-error.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { HomeComponent } from './components/home/home.component';
import { SideNavComponent } from './components/navigation/side-nav/side-nav.component';
import { AuthGuard } from './guards/auth.guard';
import { PendingChangesGuard } from './guards/pending-changes.guard';

const routes: Routes = [
  {
    path: '',
    component: SideNavComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
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
      {
        path: 'not-found',
        component: NotFoundComponent,
      },

      {
        path: 'server-error',
        component: InternalServerErrorComponent,
      },
      {
        path: 'bad-request',
        component: BadRequestComponent,
      },
      { path: '', component: HomeComponent },
    ],
  },
  { path: 'login', component: LoginComponent },
  { path: '**', pathMatch: 'full', redirectTo: '/login' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
