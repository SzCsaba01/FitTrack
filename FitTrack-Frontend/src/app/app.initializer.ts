import {
  inject,
  runInInjectionContext,
  EnvironmentInjector,
} from '@angular/core';
import { ThemeService } from './services/theme/theme.service';
import { Store } from '@ngrx/store';
import { UserService } from './services/user/user.service';
import { UserActions } from './store/user/user.actions';
import { firstValueFrom } from 'rxjs';

export function initializeApp(): () => Promise<void> {
  return () => {
    const injector = inject(EnvironmentInjector);
    return runInInjectionContext(injector, async () => {
      const themeService = inject(ThemeService);
      const userService = inject(UserService);
      const store = inject(Store);
      let appTheme = undefined;

      try {
        const response = await firstValueFrom(userService.getUserData());

        if (!response) {
          return;
        }

        appTheme = response.appTheme;

        store.dispatch(UserActions.setUser({ userDetails: response }));
      } catch {
      } finally {
        themeService.loadTheme(appTheme);
      }
    });
  };
}
