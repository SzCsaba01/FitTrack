import { inject } from '@angular/core';
import { ThemeService } from './services/theme/theme.service';
import { AuthenticationService } from './services/authentication/authentication.service';

export function initializeApp() {
  const themeService = inject(ThemeService);
  const userService = inject(AuthenticationService);
  return themeService.loadTheme();
}
