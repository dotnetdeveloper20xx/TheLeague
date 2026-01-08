import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currencyFormat',
  standalone: true
})
export class CurrencyFormatPipe implements PipeTransform {
  transform(value: number | null | undefined, currencyCode: string = 'GBP'): string {
    if (value === null || value === undefined) return '';

    return new Intl.NumberFormat('en-GB', {
      style: 'currency',
      currency: currencyCode
    }).format(value);
  }
}
