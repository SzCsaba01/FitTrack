import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectLoggedIn } from '../../store/user/user.selectors';
import { take, map } from 'rxjs';

export const authGuard: CanActivateFn = () => {
  const store = inject(Store);
  const router = inject(Router);
  return store.select(selectLoggedIn).pipe(
    take(1),
    map((loggedIn) => {
      const isLoggedIn = !!loggedIn;
      if (!isLoggedIn) {
        router.navigate(['/login']);
        return false;
      }
      return true;
    }),
  );
};
