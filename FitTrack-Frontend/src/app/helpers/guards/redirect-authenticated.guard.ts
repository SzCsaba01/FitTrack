import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { take, map } from 'rxjs';
import { selectLoggedIn } from '../../store/user/user.selectors';

export const redirectAuthenticatedGuard: CanActivateFn = () => {
  const store = inject(Store);
  const router = inject(Router);
  return store.select(selectLoggedIn).pipe(
    take(1),
    map((loggedIn) => {
      const isLoggedIn = !!loggedIn;
      if (isLoggedIn) {
        router.navigate(['/home/dashboard']);
        return false;
      }
      return true;
    }),
  );
};
