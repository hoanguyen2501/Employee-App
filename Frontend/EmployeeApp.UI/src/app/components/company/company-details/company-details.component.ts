import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormGroupDirective,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Company } from 'src/app/models/company';
import { CompanyService } from 'src/app/services/company.service';

@Component({
  selector: 'app-company-details',
  templateUrl: './company-details.component.html',
  styleUrls: ['./company-details.component.css'],
})
export class CompanyDetailsComponent implements OnInit {
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.companyForm.dirty) {
      $event.returnValue = true;
    }
  }
  companyForm: FormGroup = new FormGroup({});
  @ViewChild(FormGroupDirective, { static: false })
  formGroupDirective: any = FormGroupDirective;
  company: Company | undefined;
  companyId: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private companyService: CompanyService
  ) {}

  ngOnInit(): void {
    this.initialForm();
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          this.companyId = id;
          this.loadCompany(id);
        }
      },
    });
  }

  initialForm() {
    this.companyForm = this.formBuilder.group({
      companyName: [{ value: '', disabled: true }, Validators.required],
      establishedAt: [{ value: '', disabled: true }, Validators.required],
      address: [{ value: '', disabled: true }, Validators.required],
      city: [{ value: '', disabled: true }, Validators.required],
      country: [{ value: '', disabled: true }, Validators.required],
      email: [{ value: '', disabled: true }, Validators.required],
      phoneNumber: [{ value: '', disabled: true }, Validators.required],
    });
  }

  loadCompany(employeeId: string): void {
    this.companyService.getCompany(employeeId).subscribe({
      next: (company) => {
        this.company = company;
        this.updateEditForm(company);
      },
      error: (error) => console.log(error),
    });
  }

  updateEditForm(company: Company): void {
    this.companyForm.patchValue({
      companyName: company.companyName,
      establishedAt: company.establishedAt,
      email: company.email,
      phoneNumber: company.phoneNumber,
      address: company.address,
      city: company.city,
      country: company.country,
    });
  }
}
