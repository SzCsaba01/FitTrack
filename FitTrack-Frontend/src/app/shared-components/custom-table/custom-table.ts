import {
  Component,
  computed,
  ContentChildren,
  input,
  output,
  QueryList,
} from '@angular/core';
import { ColumnDefDirective } from '../../helpers/directives/column-def.directive';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-custom-table',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './custom-table.html',
  styleUrl: './custom-table.scss',
})
export class CustomTable {
  data = input<any[]>([]);
  totalNumberOfElements = input<number>(0);
  page = input<number>(1);
  totalNumberOfPages = input<number>(1);
  displayedColumns = input<string[]>([]);
  clickableRows = input<boolean>(false);
  rowClick = output<any>();
  pageChange = output<number>();
  @ContentChildren(ColumnDefDirective)
  columnTemplates!: QueryList<ColumnDefDirective>;

  getColumnTemplate = (key: string): ColumnDefDirective | undefined => {
    return this.columnTemplates?.find((col) => col.key === key);
  };

  numberOfElements = computed(() => this.data().length);
  fromNumberOfElements = computed(
    () => (this.page() - 1) * this.numberOfElements() + 1,
  );
  toNumberOfElements = computed(() => this.page() * this.numberOfElements());
  paginationRange = computed(() => {
    const total = this.totalNumberOfPages();
    const current = this.page();
    const range: (number | '...')[] = [];

    if (total <= 7) {
      return Array.from({ length: total }, (_, i) => i + 1);
    }

    if (current <= 3) {
      for (let i = 1; i <= current + 1; i++) {
        range.push(i);
      }
      range.push('...');
      range.push(total);
      return range;
    }

    if (current >= total - 2) {
      range.push(1);
      range.push('...');
      for (let i = current - 1; i <= total; i++) {
        range.push(i);
      }
      return range;
    }

    range.push(1);
    range.push('...');
    for (let i = current - 1; i <= current + 1; i++) {
      range.push(i);
    }
    range.push('...');
    range.push(total);

    return range;
  });

  onRowClick(row: any): void {
    this.rowClick.emit(row);
  }

  onPageChange(newPage: number): void {
    if (newPage < 1 || newPage > this.totalNumberOfPages()) {
      return;
    }

    this.pageChange.emit(newPage);
  }
}
