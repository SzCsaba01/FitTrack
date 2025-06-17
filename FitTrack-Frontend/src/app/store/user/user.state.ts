import { AuthenticationResponse } from '../../responses/authentication/authentication.response';

export interface UserState {
  userDetails: AuthenticationResponse | null;
  loggedIn: boolean;
}
