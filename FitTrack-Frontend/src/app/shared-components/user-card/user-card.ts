import { Component, computed, input, output } from '@angular/core';
import { RoleEnum } from '../../enums/role.enum';
import { Guid } from 'guid-typescript';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-card.html',
  styleUrl: './user-card.scss',
})
export class UserCard {
  isDeleteButtonShown = input<boolean>(false);
  userId = input.required<Guid>();
  profilePictureUrl = input.required<string>();
  registrationDate = input.required<Date>();
  firstName = input.required<string>();
  lastName = input.required<string>();
  username = input.required<string>();
  email = input.required<string>();
  isEmailConfirmed = input.required<boolean>();
  role = input.required<RoleEnum>();
  deleteClick = output<Guid>();
  roleEnum = RoleEnum;

  onDeleteClick(): void {
    if (!this.isDeleteButtonShown()) {
      return;
    }

    this.deleteClick.emit(this.userId());
  }
}
