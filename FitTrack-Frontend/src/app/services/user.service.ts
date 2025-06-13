import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegistrationRequest } from '../requests/user/registration.request';
import { environment } from '../../environments/environment';
import { ChangePasswordRequest } from '../requests/user/change-password.request';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseUrl = `${environment.apiUrl}/User`;

  constructor(private http: HttpClient) {}

  public register(request: RegistrationRequest) {
    return this.http.post(`${this.baseUrl}/register`, request);
  }

  public verifyEmailVerificationToken(token: string) {
    return this.http.put(
      `${this.baseUrl}/verify-email-verification-token`,
      null,
      {
        params: {
          emailVerificationToken: token,
        },
      },
    );
  }

  public verifyChangePasswordToken(token: string) {
    return this.http.put(`${this.baseUrl}/verify-change-password-token`, null, {
      params: {
        emailVerificationToken: token,
      },
    });
  }

  public sendForgotPasswordEmail(email: string) {
    return this.http.put(`${this.baseUrl}/send-forgot-password-email`, null, {
      params: {
        email: email,
      },
    });
  }

  public changePassword(request: ChangePasswordRequest) {
    return this.http.put(`${this.baseUrl}/change-password`, request);
  }
}
