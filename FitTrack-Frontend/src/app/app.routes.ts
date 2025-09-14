import { Routes } from '@angular/router';
import { permissionGuard } from './helpers/guards/permission.guard';
import { ManageUsersService } from './services/manage/users/manage-users.service';
import { ManageFoodsService } from './services/manage/foods/manage-foods.service';
import { ManageExercisesService } from './services/manage/exercises/manage-exercises.service';
import { ManageRecipesService } from './services/manage/recipes/manage-recipes.service';
import { authGuard } from './helpers/guards/auth.guard';
import { redirectAuthenticatedGuard } from './helpers/guards/redirect-authenticated.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  {
    path: 'home',
    canActivate: [authGuard],
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
      {
        path: 'manage',
        providers: [
          ManageUsersService,
          ManageFoodsService,
          ManageExercisesService,
          ManageRecipesService,
        ],
        children: [
          {
            path: 'users',
            loadComponent: () =>
              import('./pages/manage-users/manage-users').then(
                (x) => x.ManageUsers,
              ),
            canActivate: [permissionGuard(['user:manage'])],
          },
          {
            path: 'users/user-details/:id',
            loadComponent: () =>
              import('./pages/user-details/user-details').then(
                (x) => x.UserDetails,
              ),
            canActivate: [permissionGuard(['user:manage'])],
          },
          {
            path: 'exercises',
            loadComponent: () =>
              import('./pages/manage-exercises/manage-exercises').then(
                (x) => x.ManageExercises,
              ),
            canActivate: [permissionGuard(['exercises:manage'])],
          },
          {
            path: 'foods',
            loadComponent: () =>
              import('./pages/manage-foods/manage-foods').then(
                (x) => x.ManageFoods,
              ),
            canActivate: [permissionGuard(['foods:manage'])],
          },
          {
            path: 'recipes',
            loadComponent: () =>
              import('./pages/manage-recipes/manage-recipes').then(
                (x) => x.ManageRecipes,
              ),
            canActivate: [permissionGuard(['recipes:manage'])],
          },
        ],
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
        canActivate: [redirectAuthenticatedGuard],
        loadComponent: () => import('./pages/login/login').then((x) => x.Login),
      },
      {
        path: 'registration',
        canActivate: [redirectAuthenticatedGuard],
        loadComponent: () =>
          import('./pages/registration/registration').then(
            (x) => x.Registration,
          ),
      },
      {
        path: 'forgot-password',
        canActivate: [redirectAuthenticatedGuard],
        loadComponent: () =>
          import('./pages/forgot-password/forgot-password').then(
            (x) => x.ForgotPassword,
          ),
      },
      {
        path: 'verify-email',
        canActivate: [redirectAuthenticatedGuard],
        loadComponent: () =>
          import('./pages/verify-email/verify-email').then(
            (x) => x.VerifyEmail,
          ),
      },
      {
        path: 'change-password',
        canActivate: [redirectAuthenticatedGuard],
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
