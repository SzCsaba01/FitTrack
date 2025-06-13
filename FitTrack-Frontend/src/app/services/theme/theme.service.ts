import { Injectable } from '@angular/core';
import { AppThemeEnum } from '../../enums/app-theme.enum';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly themeKey = 'app-theme';
  private themeLinkId = 'app-theme-css';

  getStoredTheme(): AppThemeEnum | null {
    const storedTheme = localStorage.getItem(this.themeKey);

    if (storedTheme != null) {
      const parsedTheme = parseInt(storedTheme, 10);

      if (parsedTheme in AppThemeEnum) {
        return parsedTheme as AppThemeEnum;
      }
    }

    return null;
  }

  private getDevicePreferredTheme(): AppThemeEnum {
    if (
      window.matchMedia &&
      window.matchMedia('(prefers-color-scheme: dark)').matches
    ) {
      return AppThemeEnum.Dark;
    }
    return AppThemeEnum.Light;
  }

  private getTheme(): AppThemeEnum {
    return this.getStoredTheme() ?? this.getDevicePreferredTheme();
  }

  loadTheme(theme?: AppThemeEnum): Promise<void> {
    const themeEnum = theme ?? this.getTheme();
    const themeName = AppThemeEnum[themeEnum].toLowerCase();
    const requestedHref = `${themeName}.css`;

    const link = document.getElementById(
      this.themeLinkId,
    ) as HTMLLinkElement | null;

    if (link && link.href.endsWith(requestedHref)) {
      return Promise.resolve();
    }

    return new Promise<void>((resolve, reject) => {
      let themeLink = link;

      if (!themeLink) {
        themeLink = document.createElement('link');
        themeLink.id = this.themeLinkId;
        themeLink.rel = 'stylesheet';
        document.head.appendChild(themeLink);
      }

      themeLink.href = requestedHref;

      themeLink.onload = () => resolve();
      themeLink.onerror = (error) => reject(error);

      localStorage.setItem(this.themeKey, themeEnum.toString());
    });
  }
}
