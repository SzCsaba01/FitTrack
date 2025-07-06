import { GenderEnum } from '../../enums/gender.enum';

export interface UpdateUserProfileRequest {
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  height: number;
  weight: number;
  gender: GenderEnum;
}
