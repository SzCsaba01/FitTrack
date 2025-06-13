import { Gender } from '../enums/gender.enum';
import { SelectOption } from '../models/select-option.model';

export const GENDERS: SelectOption[] = [
  {
    value: Gender.Male,
    label: Gender[Gender.Male],
  },
  {
    value: Gender.Female,
    label: Gender[Gender.Female],
  },
  {
    value: Gender.Other,
    label: Gender[Gender.Other],
  },
];
