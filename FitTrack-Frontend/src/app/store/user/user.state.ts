import { AuthenticationResponse } from '../../responses/authentication/user-authentication.response';

export interface UserState {
  userDetails: AuthenticationResponse | null;
  loggedIn: boolean;
}
