import { Guid } from 'guid-typescript';
import { RoleEnum } from '../enums/role.enum';

export interface FilteredUser {
  id: Guid;
  username: string;
  profilePictureUrl: string;
  registrationDate: Date;
  isEmailConfirmed: boolean;
  firstName: string;
  lastName: string;
  email: string;
  role: RoleEnum;
}
