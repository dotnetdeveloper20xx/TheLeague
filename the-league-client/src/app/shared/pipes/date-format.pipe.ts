import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateFormat',
  standalone: true
})
export class DateFormatPipe implements PipeTransform {
  transform(value: Date | string | null | undefined, format: 'short' | 'medium' | 'long' | 'datetime' | 'time' = 'medium'): string {
    if (!value) return '';

    const date = new Date(value);
    if (isNaN(date.getTime())) return '';

    const options: Intl.DateTimeFormatOptions = {};

    switch (format) {
      case 'short':
        options.day = '2-digit';
        options.month = '2-digit';
        options.year = 'numeric';
        break;
      case 'medium':
        options.day = 'numeric';
        options.month = 'short';
        options.year = 'numeric';
        break;
      case 'long':
        options.day = 'numeric';
        options.month = 'long';
        options.year = 'numeric';
        options.weekday = 'long';
        break;
      case 'datetime':
        options.day = 'numeric';
        options.month = 'short';
        options.year = 'numeric';
        options.hour = '2-digit';
        options.minute = '2-digit';
        break;
      case 'time':
        options.hour = '2-digit';
        options.minute = '2-digit';
        break;
    }

    return date.toLocaleDateString('en-GB', options);
  }
}
