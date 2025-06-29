import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { AuthenticationResponse } from '../../responses/authentication/authentication.response';

export const UserActions = createActionGroup({
  source: 'User',
  events: {
    setUser: props<{ userDetails: AuthenticationResponse }>(),
    clearUser: emptyProps(),
  },
});
