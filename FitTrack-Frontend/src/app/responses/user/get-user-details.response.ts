import { Guid } from 'guid-typescript';
import { GenderEnum } from '../../enums/gender.enum';
import { RoleEnum } from '../../enums/role.enum';

export interface GetUserDetailsResponse {
  id: Guid;
  username: string;
  profilePictureUrl: string;
  registrationDate: Date;
  isEmailConfirmed: boolean;
  firstName: string;
  lastName: string;
  email: string;
  role: RoleEnum;
  gender: GenderEnum;
  height: number;
  weight: number;
}
