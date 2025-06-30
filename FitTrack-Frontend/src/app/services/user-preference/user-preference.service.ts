import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AppThemeEnum } from '../../enums/app-theme.enum';
import { Observable } from 'rxjs';
import { UnitSystemEnum } from '../../enums/unit-system.enum';

@Injectable({
  providedIn: 'root',
})
export class UserPreferenceService {
  private baseUrl = `${environment.apiUrl}/UserPreference`;

  constructor(private http: HttpClient) {}

  public updateTheme(newTheme: AppThemeEnum): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/update-app-theme`, null, {
      params: {
        newTheme: newTheme,
      },
    });
  }

  public updateUnitSystem(newUnitSystem: UnitSystemEnum): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/update-unit-system`, null, {
      params: {
        newUnitSystem: newUnitSystem,
      },
    });
  }
}
