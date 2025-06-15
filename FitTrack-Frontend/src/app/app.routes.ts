import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  // {
  //   path: 'home',
  //   loadComponent: () =>
  //     import('./layouts/main-layout/main-layout').then((m) => m.MainLayout),
  // },
  {
    path: '',
    loadComponent: () =>
      import('./layouts/public-layout/public-layout').then(
        (m) => m.PublicLayout,
      ),
    children: [
      { path: '', pathMatch: 'full', redirectTo: '/login' },
      {
        path: 'login',
        loadComponent: () => import('./pages/login/login').then((m) => m.Login),
      },
      {
        path: 'registration',
        loadComponent: () =>
          import('./pages/registration/registration').then(
            (m) => m.Registration,
          ),
      },
      {
        path: 'forgot-password',
        loadComponent: () =>
          import('./pages/forgot-password/forgot-password').then(
            (m) => m.ForgotPassword,
          ),
      },
      {
        path: 'verify-email',
        loadComponent: () =>
          import('./pages/verify-email/verify-email').then(
            (m) => m.VerifyEmail,
          ),
      },
      {
        path: 'change-password',
        loadComponent: () =>
          import('./pages/change-password/change-password').then(
            (m) => m.ChangePassword,
          ),
      },
    ],
  },
  {
    path: 'not-found',
    loadComponent: () =>
      import('./pages/not-found/not-found').then((m) => m.NotFound),
  },
  {
    path: '**',
    redirectTo: 'not-found',
  },
];
