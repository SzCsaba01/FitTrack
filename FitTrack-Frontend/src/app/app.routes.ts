import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  {
    path: 'home',
    loadComponent: () =>
      import('./layouts/main-layout/main-layout').then((x) => x.MainLayout),
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard',
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./pages/dashboard/dashboard').then((x) => x.Dashboard),
      },
      {
        path: 'edit-profile',
        loadComponent: () =>
          import('./pages/edit-profile/edit-profile').then(
            (x) => x.EditProfile,
          ),
      },
    ],
  },
  {
    path: '',
    loadComponent: () =>
      import('./layouts/public-layout/public-layout').then(
        (x) => x.PublicLayout,
      ),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'login' },
      {
        path: 'login',
        loadComponent: () => import('./pages/login/login').then((x) => x.Login),
      },
      {
        path: 'registration',
        loadComponent: () =>
          import('./pages/registration/registration').then(
            (x) => x.Registration,
          ),
      },
      {
        path: 'forgot-password',
        loadComponent: () =>
          import('./pages/forgot-password/forgot-password').then(
            (x) => x.ForgotPassword,
          ),
      },
      {
        path: 'verify-email',
        loadComponent: () =>
          import('./pages/verify-email/verify-email').then(
            (x) => x.VerifyEmail,
          ),
      },
      {
        path: 'change-password',
        loadComponent: () =>
          import('./pages/change-password/change-password').then(
            (x) => x.ChangePassword,
          ),
      },
    ],
  },
  {
    path: 'not-found',
    loadComponent: () =>
      import('./pages/not-found/not-found').then((x) => x.NotFound),
  },
  {
    path: '**',
    redirectTo: 'not-found',
  },
];
