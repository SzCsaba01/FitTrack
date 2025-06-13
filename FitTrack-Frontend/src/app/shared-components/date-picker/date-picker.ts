import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  ElementRef,
  HostListener,
  OnInit,
  signal,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [MatIconModule, CommonModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: DatePicker,
      multi: true,
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,

  templateUrl: './date-picker.html',
  styleUrl: './date-picker.scss',
})
export class DatePicker implements ControlValueAccessor, OnInit {
  todayDate: Date = new Date();
  yearSignal = signal<number>(this.todayDate.getFullYear());
  monthSignal = signal<number>(this.todayDate.getMonth());
  selectedDateSignal = signal<Date | null>(null);
  currentDatesSignal = signal<Date[]>([]);
  currentMonthNameSignal = signal<string>('');
  isSelectDateShownSignal = signal<boolean>(false);
  isSelectMonthShownSignal = signal<boolean>(false);
  isSelectYearShownSignal = signal<boolean>(false);
  isDisabledSignal = signal<boolean>(false);
  daysOfWeek: string[] = [];
  months: string[] = [];
  private onChange: (_: any) => void = () => {};
  private onTouched: () => void = () => {};

  yearsSignal = computed(() => {
    if (!this.yearSignal()) {
      return [];
    }
    const years: number[] = [];
    for (let i = this.yearSignal() - 6; i <= this.yearSignal() + 5; i++) {
      years.push(i);
    }
    return years;
  });

  constructor(private elementRef: ElementRef<HTMLElement>) {}

  ngOnInit(): void {
    this.daysOfWeek = this.getLocalizedDaysOfWeek();
    this.months = this.getLocalizedMonthNames();
    this.generateDates();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (
      !this.elementRef.nativeElement.contains(event.target as Node) &&
      (this.isSelectYearShownSignal() ||
        this.isSelectMonthShownSignal() ||
        this.isSelectDateShownSignal())
    ) {
      this.onHideDatepicker();
    }
  }

  private getLocalizedDaysOfWeek(): string[] {
    const formatter = new Intl.DateTimeFormat('default', { weekday: 'short' });
    return Array.from({ length: 7 }, (_, i) =>
      formatter.format(new Date(Date.UTC(2021, 0, 3 + i))).charAt(0),
    );
  }

  private getLocalizedMonthNames(): string[] {
    const formatter = new Intl.DateTimeFormat('default', { month: 'short' });
    return Array.from({ length: 12 }, (_, i) => {
      const name = formatter.format(new Date(2000, i, 1));
      return name.charAt(0).toUpperCase() + name.slice(1, 3);
    });
  }

  private generateDates(): void {
    const y = this.yearSignal();
    const m = this.monthSignal();
    const firstDayOfMonth = new Date(y, m, 1);
    const startDay = firstDayOfMonth.getDay();
    const startDate = new Date(y, m, 1 - startDay);

    this.currentMonthNameSignal.set(
      firstDayOfMonth.toLocaleString('default', { month: 'long' }),
    );

    const dates: Date[] = [];
    const current = new Date(startDate);
    for (let i = 0; i < 42; i++) {
      dates.push(new Date(current));
      current.setDate(current.getDate() + 1);
    }

    this.currentDatesSignal.set(dates);
  }

  writeValue(value: any): void {
    if (value instanceof Date && !isNaN(value.getTime())) {
      this.selectedDateSignal.set(new Date(value));
      this.yearSignal.set(value.getFullYear());
      this.monthSignal.set(value.getMonth());
      this.generateDates();
    } else if (value === null) {
      this.selectedDateSignal.set(null);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.isDisabledSignal.set(isDisabled);
  }

  onToggleSelectDate(): void {
    this.isSelectYearShownSignal.set(false);
    this.isSelectMonthShownSignal.set(false);
    this.isSelectDateShownSignal.update((x) => !x);
  }

  onToggleSelectMonth(): void {
    this.isSelectDateShownSignal.set(false);
    this.isSelectYearShownSignal.set(false);
    this.isSelectMonthShownSignal.update((x) => !x);
  }

  onToggleSelectYear(): void {
    this.isSelectMonthShownSignal.set(false);
    this.isSelectDateShownSignal.set(false);
    this.isSelectYearShownSignal.update((x) => !x);
  }

  onHideDatepicker(): void {
    this.isSelectDateShownSignal.set(false);
    this.isSelectYearShownSignal.set(false);
    this.isSelectMonthShownSignal.set(false);
  }

  onSelectDate(date: Date): void {
    if (this.isDisabledSignal()) {
      return;
    }
    const selected = this.selectedDateSignal();
    if (
      selected &&
      selected.getDate() === date.getDate() &&
      selected.getMonth() === date.getMonth() &&
      selected.getFullYear() === date.getFullYear()
    ) {
      return;
    }

    this.selectedDateSignal.set(new Date(date));
    this.onChange(date);
    this.onTouched();
    this.onHideDatepicker();
  }

  onSelectMonth(month: number): void {
    if (this.isDisabledSignal()) {
      return;
    }
    this.monthSignal.set(month);
    const existing = this.selectedDateSignal();
    if (existing) {
      const newDate = new Date(
        existing.getFullYear(),
        month,
        existing.getDate(),
      );
      this.selectedDateSignal.set(newDate);
      this.onChange(newDate);
      this.onTouched();
    }
    this.generateDates();
    this.onToggleSelectDate();
  }

  onSelectYear(year: number): void {
    if (this.isDisabledSignal()) {
      return;
    }
    this.yearSignal.set(year);
    const existing = this.selectedDateSignal();
    if (existing) {
      const newDate = new Date(year, existing.getMonth(), existing.getDate());
      this.selectedDateSignal.set(newDate);
      this.onChange(newDate);
      this.onTouched();
    }
    this.generateDates();
    this.onToggleSelectDate();
  }

  isDateSelected(date: Date): boolean {
    const selected = this.selectedDateSignal();
    return (
      selected !== null &&
      selected.getDate() === date.getDate() &&
      selected.getMonth() === date.getMonth() &&
      selected.getFullYear() === date.getFullYear()
    );
  }

  isMonthSelected(month: number): boolean {
    return this.selectedDateSignal()?.getMonth() === month;
  }

  isYearSelected(year: number): boolean {
    return this.selectedDateSignal()?.getFullYear() === year;
  }

  isToday(date: Date): boolean {
    return (
      this.todayDate.getFullYear() === date.getFullYear() &&
      this.todayDate.getMonth() === date.getMonth() &&
      this.todayDate.getDate() === date.getDate()
    );
  }

  isDateDisabled(date: Date): boolean {
    return (
      this.yearSignal() !== date.getFullYear() ||
      this.monthSignal() !== date.getMonth()
    );
  }

  onChangeMonth(direction: number): void {
    const m = this.monthSignal();
    const y = this.yearSignal();

    if (direction > 0) {
      this.monthSignal.set(m === 11 ? 0 : m + 1);
      if (m === 11) this.yearSignal.set(y + 1);
    } else {
      this.monthSignal.set(m === 0 ? 11 : m - 1);
      if (m === 0) this.yearSignal.set(y - 1);
    }

    this.generateDates();
  }

  onChangeYear(direction: number): void {
    this.yearSignal.update((y) => y + direction);
    this.generateDates();
  }

  onChangeOnlyYear(direction: number): void {
    this.yearSignal.update((y) => y + direction);
  }
}
