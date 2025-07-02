import {
  ApplicationConfig,
  isDevMode,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { initializeApp } from './app.initializer';
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
} from '@angular/common/http';
import { httpErrorInterceptor } from './helpers/interceptors/http-error.interceptor';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideState, provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { userFeature } from './store/user/user.reducer';
import { httpRequestInterceptor } from './helpers/interceptors/http-request.interceptor';
import { UserEffects } from './store/user/user.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAppInitializer(initializeApp),
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(
      withFetch(),
      withInterceptors([httpRequestInterceptor, httpErrorInterceptor]),
    ),
    provideStore(),
    provideState(userFeature),
    provideEffects(UserEffects),
    provideStoreDevtools({
      maxAge: 25,
      logOnly: !isDevMode(),
      autoPause: true,
    }),
  ],
};
