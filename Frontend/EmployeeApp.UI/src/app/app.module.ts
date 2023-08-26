import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/auth/login/login.component';
import { CompanyAddComponent } from './components/company/company-add/company-add.component';
import { CompanyDetailsComponent } from './components/company/company-details/company-details.component';
import { CompanyEditComponent } from './components/company/company-edit/company-edit.component';
import { CompanyListComponent } from './components/company/company-list/company-list.component';
import { RefreshDialogComponent } from './components/dialog/refresh-dialog/refresh-dialog.component';
import { EmployeeAddComponent } from './components/employee/employee-add/employee-add.component';
import { EmployeeDetailsComponent } from './components/employee/employee-details/employee-details.component';
import { EmployeeEditComponent } from './components/employee/employee-edit/employee-edit.component';
import { EmployeeListComponent } from './components/employee/employee-list/employee-list.component';
import { BadRequestComponent } from './components/errors/bad-request/bad-request.component';
import { InternalServerErrorComponent } from './components/errors/internal-server-error/internal-server-error.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { TextInputComponent } from './components/forms/text-input/text-input.component';
import { HomeComponent } from './components/home/home.component';
import { SideNavComponent } from './components/navigation/side-nav/side-nav.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { LoadingInterceptor } from './interceptors/loading.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    SideNavComponent,
    EmployeeListComponent,
    EmployeeAddComponent,
    EmployeeEditComponent,
    EmployeeDetailsComponent,
    CompanyListComponent,
    CompanyAddComponent,
    CompanyEditComponent,
    CompanyDetailsComponent,
    NotFoundComponent,
    InternalServerErrorComponent,
    HomeComponent,
    LoginComponent,
    BadRequestComponent,
    TextInputComponent,
    RefreshDialogComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      timeOut: 3000,
      easeTime: 500,
      easing: 'ease',
      progressAnimation: 'increasing',
    }),
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatPaginatorModule,
    MatSidenavModule,
    MatCheckboxModule,
    MatListModule,
    MatToolbarModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatMenuModule,
    MatDialogModule,
    ReactiveFormsModule,
    NgxSpinnerModule.forRoot({
      type: 'ball-spin-clockwise',
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
