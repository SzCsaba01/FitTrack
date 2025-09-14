import { Component, effect, OnInit, signal } from '@angular/core';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { CustomTable } from '../../shared-components/custom-table/custom-table';
import { MatIconModule } from '@angular/material/icon';
import { ColumnDefDirective } from '../../helpers/directives/column-def.directive';
import { ScreenService } from '../../services/screen/screen.service';
import { FilteredUser } from '../../models/filtered-user.model';
import { UserCard } from '../../shared-components/user-card/user-card';
import { ManageUsersService } from '../../services/manage/users/manage-users.service';
import { GetFilteredUsersRequest } from '../../requests/user/get-filtered-users.request';
import { takeUntil } from 'rxjs';
import { RoleEnum } from '../../enums/role.enum';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manage-users',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    CustomTable,
    UserCard,
    ColumnDefDirective,
  ],
  templateUrl: './manage-users.html',
  styleUrl: './manage-users.scss',
})
export class ManageUsers extends SelfUnsubscriberBase implements OnInit {
  isMobile = signal<boolean>(false);
  search = signal<string>('');
  displayedColumns = signal<string[]>([
    'profilePictureUrl',
    'username',
    'registrationDate',
    'firstName',
    'lastName',
    'email',
    'isEmailConfirmed',
    'role',
    'action',
  ]);
  users = signal<FilteredUser[]>([]);
  page = signal<number>(1);
  pageSize = signal<number>(10);
  totalNumberOfPages = signal<number>(0);
  totalNumberOfUsers = signal(0);
  roleEnum = RoleEnum;

  constructor(
    private manageUsersService: ManageUsersService,
    private screenServie: ScreenService,
    private router: Router,
  ) {
    super();
    effect(() => {
      this.isMobile.set(this.screenServie.isMobile());
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {
    const requestBody = {} as GetFilteredUsersRequest;
    requestBody.page = this.page();
    requestBody.pageSize = this.pageSize();
    requestBody.search = this.search();

    this.manageUsersService
      .getFilteredUsers(requestBody)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        this.totalNumberOfPages.set(result.totalNumberOfPages);
        this.totalNumberOfUsers.set(result.totalNumberOfUsers);
        result.users.forEach((u) => {
          u.registrationDate = new Date(u.registrationDate);
        });
        this.users.set(result.users);
      });
  }

  onSetSearch(event: any) {
    this.search.set(event.target.value);
  }

  onSearch(): void {
    this.page.set(1);
    this.loadData();
  }

  onUserClick(user: FilteredUser): void {
    this.router.navigate(['/home/manage/users/user-details', user.id]);
  }

  onDeleteClick(userId: Guid): void {
    console.log(userId);
  }

  onPageChange(newPage: number) {
    this.page.set(newPage);
    this.loadData();
  }
}
