import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegistrationRequest } from '../../requests/user/registration.request';
import { environment } from '../../../environments/environment';
import { ChangePasswordRequest } from '../../requests/user/change-password.request';
import { AuthenticationResponse } from '../../responses/authentication/authentication.response';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseUrl = `${environment.apiUrl}/User`;

  constructor(private http: HttpClient) {}

  public getUserData(): Observable<AuthenticationResponse> {
    return this.http.get<AuthenticationResponse>(
      `${this.baseUrl}/get-user-data`,
    );
  }

  public register(request: RegistrationRequest): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/register`, request);
  }

  public verifyEmailVerificationToken(token: string): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/verify-email-verification-token`,
      null,
      {
        params: {
          token: token,
        },
      },
    );
  }

  public verifyChangePasswordToken(token: string): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/verify-change-password-token`,
      null,
      {
        params: {
          token: token,
        },
      },
    );
  }

  public sendForgotPasswordEmail(email: string): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/send-forgot-password-email`,
      null,
      {
        params: {
          email: email,
        },
      },
    );
  }

  public changePassword(request: ChangePasswordRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/change-password`, request);
  }
}
