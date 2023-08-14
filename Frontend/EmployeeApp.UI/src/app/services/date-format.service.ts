import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DateFormatService {
  constructor() {}

  formatDate(date: Date | string, format: string = 'yyyy-MM-dd'): string {
    const formattedDate = new DatePipe('en-US').transform(date, format) ?? date;
    return formattedDate.toString();
  }
}
