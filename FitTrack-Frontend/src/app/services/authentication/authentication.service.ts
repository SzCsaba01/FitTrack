import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginRequest } from '../../requests/authentication/login.request';
import { AuthenticationResponse } from '../../responses/authentication/user-authentication.response';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private baseUrl = `${environment.apiUrl}/Authentication`;

  constructor(private http: HttpClient) {}

  public login(request: LoginRequest): Observable<AuthenticationResponse> {
    return this.http.put<AuthenticationResponse>(
      `${this.baseUrl}/login`,
      request,
    );
  }

  public logout() {
    return this.http.put(`${this.baseUrl}/logout`, null);
  }

  public refreshToken() {
    return this.http.put(`${this.baseUrl}/refresh-token`, null);
  }
}
