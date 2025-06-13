import { inject } from '@angular/core';
import { ThemeService } from './services/theme/theme.service';

export function initializeApp() {
  const themeService = inject(ThemeService);
  return themeService.loadTheme();
}
