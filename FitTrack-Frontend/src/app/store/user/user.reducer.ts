import { createFeature, createReducer, on } from '@ngrx/store';
import { UserState } from './user.state';
import { UserActions } from './user.actions';

const initialState: UserState = {
  userDetails: null,
  loggedIn: false,
};

const reducer = createReducer(
  initialState,
  on(UserActions.setUser, (state, authResponse) => ({
    ...state,
    authResponse,
    loggedIn: true,
  })),
  on(UserActions.clearUser, () => initialState),
);

export const userFeature = createFeature({
  name: 'user',
  reducer,
});
