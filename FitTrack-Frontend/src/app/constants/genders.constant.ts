import { GenderEnum } from '../enums/gender.enum';
import { SelectOption } from '../models/select-option.model';

export const GENDERS: SelectOption[] = [
  {
    value: GenderEnum.Male,
    label: GenderEnum[GenderEnum.Male],
  },
  {
    value: GenderEnum.Female,
    label: GenderEnum[GenderEnum.Female],
  },
  {
    value: GenderEnum.Other,
    label: GenderEnum[GenderEnum.Other],
  },
];
