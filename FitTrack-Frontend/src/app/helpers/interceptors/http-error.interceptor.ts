import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
  HttpStatusCode,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { NotificationService } from '../../services/notification/notification.service';

export const httpErrorInterceptor: (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
) => Observable<HttpEvent<unknown>> = (req, next) => {
  const notificationService = inject(NotificationService);

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
      switch (error.status) {
        case HttpStatusCode.BadRequest:
          if (error.error?.message) {
            const errorMessage = error.error.message;
            return throwError(() => ({ ...error, errorMessage }));
          }
          break;
        case HttpStatusCode.Unauthorized:
          notificationService.showError('You are not authorized.');
          break;
        case HttpStatusCode.Forbidden:
          notificationService.showError('Access denied.');
          break;
        case HttpStatusCode.NotFound:
          notificationService.showError('Resource not found.');
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
