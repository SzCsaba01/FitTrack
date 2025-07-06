import { createFeature, createReducer, on } from '@ngrx/store';
import { UserState } from './user.state';
import { UserActions } from './user.actions';

const initialState: UserState = {
  userDetails: null,
  loggedIn: false,
};

const reducer = createReducer(
  initialState,
  on(UserActions.setUser, (state, { userDetails }) => ({
    ...state,
    userDetails,
    loggedIn: true,
  })),
  on(UserActions.updateFirstAndLastName, (state, { firstName, lastName }) => ({
    ...state,
    userDetails: state.userDetails
      ? { ...state.userDetails, firstName: firstName, lastName: lastName }
      : null,
  })),
  on(UserActions.updateAppTheme, (state, { theme }) => ({
    ...state,
    userDetails: state.userDetails
      ? { ...state.userDetails, appTheme: theme }
      : null,
  })),
  on(UserActions.updateUnitSystem, (state, { unitSystem }) => ({
    ...state,
    userDetails: state.userDetails
      ? { ...state.userDetails, unitSystem: unitSystem }
      : null,
  })),
  on(UserActions.clearUser, () => initialState),
);

export const userFeature = createFeature({
  name: 'user',
  reducer,
});
