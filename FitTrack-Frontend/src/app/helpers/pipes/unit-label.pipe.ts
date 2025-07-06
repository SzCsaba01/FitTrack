import { Pipe, PipeTransform } from '@angular/core';
import { UnitSystemEnum } from '../../enums/unit-system.enum';

@Pipe({ name: 'unitLabel', standalone: true })
export class UnitLabelPipe implements PipeTransform {
  private readonly labels: Record<string, Record<UnitSystemEnum, string>> = {
    weight: {
      [UnitSystemEnum.Metric]: 'kg',
      [UnitSystemEnum.Imperial]: 'lb',
    },
    height: {
      [UnitSystemEnum.Metric]: 'cm',
      [UnitSystemEnum.Imperial]: 'in',
    },
    distance: {
      [UnitSystemEnum.Metric]: 'km',
      [UnitSystemEnum.Imperial]: 'mi',
    },
    temperature: {
      [UnitSystemEnum.Metric]: '°C',
      [UnitSystemEnum.Imperial]: '°F',
    },
  };

  transform(
    type: keyof typeof this.labels,
    UnitSystemEnum: UnitSystemEnum,
  ): string {
    return this.labels[type]?.[UnitSystemEnum] ?? '';
  }
}
