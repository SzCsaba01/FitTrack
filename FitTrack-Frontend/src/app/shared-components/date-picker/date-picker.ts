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
  year = signal<number>(this.todayDate.getFullYear());
  month = signal<number>(this.todayDate.getMonth());
  selectedDate = signal<Date | null>(null);
  currentDates = signal<Date[]>([]);
  currentMonthName = signal<string>('');
  isSelectDateShown = signal<boolean>(false);
  isSelectMonthShown = signal<boolean>(false);
  isSelectYearShown = signal<boolean>(false);
  isDisabled = signal<boolean>(false);
  daysOfWeek: string[] = [];
  months: string[] = [];
  private onChange: (_: any) => void = () => {};
  private onTouched: () => void = () => {};

  years = computed(() => {
    if (!this.year()) {
      return [];
    }
    const years: number[] = [];
    for (let i = this.year() - 6; i <= this.year() + 5; i++) {
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
      (this.isSelectYearShown() ||
        this.isSelectMonthShown() ||
        this.isSelectDateShown())
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
    const y = this.year();
    const m = this.month();
    const firstDayOfMonth = new Date(y, m, 1);
    const startDay = firstDayOfMonth.getDay();
    const startDate = new Date(y, m, 1 - startDay);

    this.currentMonthName.set(
      firstDayOfMonth.toLocaleString('default', { month: 'long' }),
    );

    const dates: Date[] = [];
    const current = new Date(startDate);
    for (let i = 0; i < 42; i++) {
      dates.push(new Date(current));
      current.setDate(current.getDate() + 1);
    }

    this.currentDates.set(dates);
  }

  writeValue(value: any): void {
    let date: Date | null = null;

    if (typeof value === 'string') {
      const parsedDate = new Date(value);
      if (!isNaN(parsedDate.getTime())) {
        date = parsedDate;
      }
    }

    if (date) {
      this.selectedDate.set(date);
      this.year.set(date.getFullYear());
      this.month.set(date.getMonth());
      this.generateDates();
    } else if (value === null) {
      this.selectedDate.set(null);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled.set(isDisabled);
  }

  onToggleSelectDate(): void {
    this.isSelectYearShown.set(false);
    this.isSelectMonthShown.set(false);
    this.isSelectDateShown.update((x) => !x);
  }

  onToggleSelectMonth(): void {
    this.isSelectDateShown.set(false);
    this.isSelectYearShown.set(false);
    this.isSelectMonthShown.update((x) => !x);
  }

  onToggleSelectYear(): void {
    this.isSelectMonthShown.set(false);
    this.isSelectDateShown.set(false);
    this.isSelectYearShown.update((x) => !x);
  }

  onHideDatepicker(): void {
    this.isSelectDateShown.set(false);
    this.isSelectYearShown.set(false);
    this.isSelectMonthShown.set(false);
  }

  onSelectDate(date: Date): void {
    if (this.isDisabled()) {
      return;
    }

    const selected = this.selectedDate();

    const newDate = new Date(
      Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()),
    );

    if (
      selected &&
      selected.getUTCDate() === newDate.getUTCDate() &&
      selected.getUTCMonth() === newDate.getUTCMonth() &&
      selected.getUTCFullYear() === newDate.getUTCFullYear()
    ) {
      return;
    }

    this.selectedDate.set(newDate);
    this.onChange(newDate); // emits normalized UTC date
    this.onTouched();
    this.onHideDatepicker();
  }

  onSelectMonth(month: number): void {
    if (this.isDisabled()) {
      return;
    }
    this.month.set(month);
    const existing = this.selectedDate();
    if (existing) {
      const newDate = new Date(
        existing.getFullYear(),
        month,
        existing.getDate(),
      );
      this.selectedDate.set(newDate);
      this.onChange(newDate);
      this.onTouched();
    }
    this.generateDates();
    this.onToggleSelectDate();
  }

  onSelectYear(year: number): void {
    if (this.isDisabled()) {
      return;
    }
    this.year.set(year);
    const existing = this.selectedDate();
    if (existing) {
      const newDate = new Date(year, existing.getMonth(), existing.getDate());
      this.selectedDate.set(newDate);
      this.onChange(newDate);
      this.onTouched();
    }
    this.generateDates();
    this.onToggleSelectDate();
  }

  isDateSelected(date: Date): boolean {
    const selected = this.selectedDate();
    return (
      selected !== null &&
      selected.getDate() === date.getDate() &&
      selected.getMonth() === date.getMonth() &&
      selected.getFullYear() === date.getFullYear()
    );
  }

  isMonthSelected(month: number): boolean {
    return this.selectedDate()?.getMonth() === month;
  }

  isYearSelected(year: number): boolean {
    return this.selectedDate()?.getFullYear() === year;
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
      this.year() !== date.getFullYear() || this.month() !== date.getMonth()
    );
  }

  onChangeMonth(direction: number): void {
    const m = this.month();
    const y = this.year();

    if (direction > 0) {
      this.month.set(m === 11 ? 0 : m + 1);
      if (m === 11) this.year.set(y + 1);
    } else {
      this.month.set(m === 0 ? 11 : m - 1);
      if (m === 0) this.year.set(y - 1);
    }

    this.generateDates();
  }

  onChangeYear(direction: number): void {
    this.year.update((y) => y + direction);
    this.generateDates();
  }

  onChangeOnlyYear(direction: number): void {
    this.year.update((y) => y + direction);
  }
}
