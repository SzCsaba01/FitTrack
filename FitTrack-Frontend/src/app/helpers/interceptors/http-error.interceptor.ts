import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
  HttpStatusCode,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError, switchMap, tap, throwError } from 'rxjs';
import { NotificationService } from '../../services/notification/notification.service';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { Router } from '@angular/router';

export const httpErrorInterceptor: (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
) => Observable<HttpEvent<unknown>> = (req, next) => {
  const notificationService = inject(NotificationService);
  const authenticationService = inject(AuthenticationService);
  const router = inject(Router);

  return next(req).pipe(
    tap((event: any) => {
      if (
        event.status === HttpStatusCode.Ok &&
        event.body &&
        event.body.message
      ) {
        notificationService.showSuccess(event.body.message);
      }
    }),
    catchError((error: HttpErrorResponse) => {
      if (error.status === HttpStatusCode.Unauthorized) {
        if (error.error?.detail !== 'token_expired') {
          return throwError(() => error);
        }

        return authenticationService.refreshToken().pipe(
          switchMap(() => {
            return next(req);
          }),
          catchError((refreshError) => {
            notificationService.showError(
              'Session expired. Please log in again',
            );
            authenticationService.logout().subscribe({
              next: () => {
                router.navigate(['/login']);
              },
              error: () => {
                router.navigate(['/login']);
              },
            });
            return throwError(() => refreshError);
          }),
        );
      }

      switch (error.status) {
        case HttpStatusCode.BadRequest:
          if (error.error?.detail) {
            const errorMessage = error.error.detail;
            return throwError(() => ({ ...error, errorMessage }));
          }
          break;
        case HttpStatusCode.Forbidden:
          notificationService.showError('Access denied.');
          break;
        case 0:
        case HttpStatusCode.InternalServerError:
        case HttpStatusCode.BadGateway:
        case HttpStatusCode.ServiceUnavailable:
        case HttpStatusCode.GatewayTimeout:
          notificationService.showError('A server error occurred.');
          break;
        default:
          notificationService.showError('An unexpected error occurred.');
      }

      return throwError(() => error);
    }),
  );
};
