import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { AuthenticationResponse } from '../../responses/authentication/user-authentication.response';

export const UserActions = createActionGroup({
  source: 'User',
  events: {
    setUser: props<AuthenticationResponse>(),
    clearUser: emptyProps(),
  },
});
