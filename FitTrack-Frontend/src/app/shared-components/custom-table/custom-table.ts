import {
  Component,
  ContentChildren,
  input,
  output,
  QueryList,
} from '@angular/core';
import { ColumnDefDirective } from '../../helpers/directives/column-def.directive';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-custom-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './custom-table.html',
  styleUrl: './custom-table.scss',
})
export class CustomTable {
  data = input<any[]>([]);
  displayedColumns = input<string[]>([]);

  rowClick = output<any>();

  @ContentChildren(ColumnDefDirective)
  columnTemplates!: QueryList<ColumnDefDirective>;

  getColumnTemplate = (key: string): ColumnDefDirective | undefined => {
    return this.columnTemplates?.find((col) => col.key === key);
  };

  onRowClick(row: any) {
    this.rowClick.emit(row);
  }
}
