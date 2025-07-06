import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { AuthenticationResponse } from '../../responses/authentication/authentication.response';
import { UnitSystemEnum } from '../../enums/unit-system.enum';
import { AppThemeEnum } from '../../enums/app-theme.enum';

export const UserActions = createActionGroup({
  source: 'User',
  events: {
    setUser: props<{ userDetails: AuthenticationResponse }>(),
    updateFirstAndLastName: props<{ firstName: string; lastName: string }>(),
    updateAppTheme: props<{ theme: AppThemeEnum }>(),
    updateUnitSystem: props<{ unitSystem: UnitSystemEnum }>(),
    clearUser: emptyProps(),
  },
});
