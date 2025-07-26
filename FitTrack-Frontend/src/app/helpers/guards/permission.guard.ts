import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { take, map } from 'rxjs';
import { selectUserDetails } from '../../store/user/user.selectors';

export const permissionGuard = (
  requiredPermissions: string[],
): CanActivateFn => {
  return () => {
    const store = inject(Store);
    const router = inject(Router);
    return store.select(selectUserDetails).pipe(
      take(1),
      map((user) => {
        const userPerms = user?.permissions || [];
        const hasAccess = requiredPermissions.every((p) =>
          userPerms.includes(p),
        );
        if (!hasAccess) {
          router.navigate(['/home/dashboard']);
        }
        return hasAccess;
      }),
    );
  };
};
