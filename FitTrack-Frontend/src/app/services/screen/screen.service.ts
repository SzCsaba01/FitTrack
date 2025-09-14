import { computed, Injectable, signal } from '@angular/core';
import { MOBILE_BREAKPOINT } from '../../constants/sizes.constant';

@Injectable({
  providedIn: 'root',
})
export class ScreenService {
  private screenWidth = signal<number>(window.innerWidth);
  isMobile = computed(() => this.screenWidth() < MOBILE_BREAKPOINT);

  constructor() {
    window.addEventListener('resize', () => {
      this.screenWidth.set(window.innerWidth);
    });
  }
}
