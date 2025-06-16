import { AppThemeEnum } from '../../enums/app-theme.enum';
import { UnitSystemEnum } from '../../enums/unit-system.enum';

export interface AuthenticationResponse {
  firstName: string;
  lastName: string;
  permissions: string[];
  unitSystem: UnitSystemEnum;
  appTheme: AppThemeEnum;
}
