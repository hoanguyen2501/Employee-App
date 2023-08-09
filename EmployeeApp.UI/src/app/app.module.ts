import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CompanyAddComponent } from './components/companies/company-add/company-add.component';
import { CompanyDetailsComponent } from './components/companies/company-details/company-details.component';
import { CompanyEditComponent } from './components/companies/company-edit/company-edit.component';
import { CompaniesListComponent } from './components/companies/company-list/company-list.component';
import { EmployeeAddComponent } from './components/employees/employee-add/employee-add.component';
import { EmployeeDetailsComponent } from './components/employees/employee-details/employee-details.component';
import { EmployeeEditComponent } from './components/employees/employee-edit/employee-edit.component';
import { EmployeeListComponent } from './components/employees/employee-list/employee-list.component';
import { TextInputComponent } from './components/forms/text-input/text-input.component';
import { HomeComponent } from './components/home/home.component';
import { PendingChangesGuard } from './guards/pending-changes.guard';
@NgModule({
  declarations: [
    AppComponent,
    EmployeeListComponent,
    EmployeeDetailsComponent,
    EmployeeAddComponent,
    EmployeeEditComponent,
    CompaniesListComponent,
    CompanyDetailsComponent,
    CompanyAddComponent,
    CompanyEditComponent,
    HomeComponent,
    TextInputComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      timeOut: 3000,
      easeTime: 500,
      easing: 'ease',
      progressAnimation: 'increasing',
    }),
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [PendingChangesGuard],
  bootstrap: [AppComponent],
})
export class AppModule {}
