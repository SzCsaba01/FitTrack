import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { UnitSystemEnum } from '../../enums/unit-system.enum';
import { UserProfileResponse } from '../../responses/user-profile/user-profile.response';
import { Observable } from 'rxjs';
import { UpdateUserProfileRequest } from '../../requests/user-profile/update-user-profile.request';
@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  private baseUrl = `${environment.apiUrl}/UserProfile`;

  constructor(private http: HttpClient) {}

  public getUserProfile(
    unitSystem: UnitSystemEnum,
  ): Observable<UserProfileResponse> {
    return this.http.get<UserProfileResponse>(
      `${this.baseUrl}/get-user-profile`,
      {
        params: {
          unitSystem: unitSystem,
        },
      },
    );
  }

  public updateUserProfile(
    unitSystem: UnitSystemEnum,
    request: UpdateUserProfileRequest,
  ): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/update-user-profile`, request, {
      params: {
        unitSystem: unitSystem,
      },
    });
  }
}
