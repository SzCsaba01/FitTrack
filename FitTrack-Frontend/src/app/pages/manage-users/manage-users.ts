import { Component, OnInit } from '@angular/core';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { CustomTable } from '../../shared-components/custom-table/custom-table';
import { MatIconModule } from '@angular/material/icon';
import { ColumnDefDirective } from '../../helpers/directives/column-def.directive';

@Component({
  selector: 'app-manage-users',
  standalone: true,
  imports: [MatIconModule, CustomTable, ColumnDefDirective],
  templateUrl: './manage-users.html',
  styleUrl: './manage-users.scss',
})
export class ManageUsers extends SelfUnsubscriberBase implements OnInit {
  rows = [
    { id: 1, name: 'Alice', photoUrl: 'assets/alice.png' },
    { id: 2, name: 'Bob', photoUrl: '' },
  ];

  columns = ['name', 'photo', 'action'];

  constructor() {
    super();
  }

  ngOnInit(): void {}

  onRowClick(row: any): void {}

  onEdit(data: any): void {}
}
