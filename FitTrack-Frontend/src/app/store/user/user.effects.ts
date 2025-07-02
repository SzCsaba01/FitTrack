import { inject, Injectable } from '@angular/core';
import { ThemeService } from '../../services/theme/theme.service';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { UserActions } from './user.actions';
import { from, switchMap } from 'rxjs';

@Injectable()
export class UserEffects {
  private actions$ = inject(Actions);
  private themeService = inject(ThemeService);

  loadThemeOnUpdate$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(UserActions.updateAppTheme),
        switchMap(({ theme }) => from(this.themeService.loadTheme(theme))),
      ),
    { dispatch: false },
  );
}
