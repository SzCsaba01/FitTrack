import { GenderEnum } from '../../enums/gender.enum';

export interface UserProfileResponse {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  height: number;
  weight: number;
  gender: GenderEnum;
}
