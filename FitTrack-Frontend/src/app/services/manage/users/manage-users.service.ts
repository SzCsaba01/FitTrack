import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { GetFilteredUsersRequest } from '../../../requests/user/get-filtered-users.request';
import { FilteredUserResponse } from '../../../responses/user/filtered-user.response';
import { Observable } from 'rxjs';
import { UnitSystemEnum } from '../../../enums/unit-system.enum';
import { GetUserDetailsResponse } from '../../../responses/user/get-user-details.response';

@Injectable()
export class ManageUsersService {
  private baseUrl = `${environment.apiUrl}/User`;

  constructor(private http: HttpClient) {}

  public getFilteredUsers(
    request: GetFilteredUsersRequest,
  ): Observable<FilteredUserResponse> {
    return this.http.post<FilteredUserResponse>(
      `${this.baseUrl}/get-filtered-users`,
      request,
    );
  }

  public getUserDetails(
    userId: string,
    unitSystem: UnitSystemEnum,
  ): Observable<GetUserDetailsResponse> {
    return this.http.get<GetUserDetailsResponse>(
      `${this.baseUrl}/get-user-details`,
      {
        params: {
          userId: userId,
          unitSystem: unitSystem,
        },
      },
    );
  }
}
