import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root',
})
export class DateFormatService {
  constructor() {}

  formatDate(date: Date | string, format: string = 'yyyy-MM-dd'): string {
    const formattedDate = new DatePipe('en-US').transform(date, format) ?? date;
    return formattedDate.toString();
  }

  setDate(date: Date | string) {
    date = new Date(date);
    const timeOffset = Math.abs(date.getTimezoneOffset());
    const hoursOffset = timeOffset / 60;
    const minutesOffset = timeOffset % 60;

    const localTime = moment(date).add({
      hours: hoursOffset,
      minutes: minutesOffset,
    });

    return (
      new DatePipe('en-US').transform(
        localTime.toDate(),
        'yyyy-MM-dd',
        `+${hoursOffset}`
      ) ?? localTime.toLocaleString()
    );
  }
}
