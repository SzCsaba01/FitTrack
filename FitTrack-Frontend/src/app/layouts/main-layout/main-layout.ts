import { Component, computed, OnInit, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MOBILE_BRAKEPOINT } from '../../constants/sizes.constant';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, MatIconModule, RouterModule],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout implements OnInit {
  private screenWidth = signal(window.innerWidth);

  isMobile = computed<boolean>(() => this.screenWidth() < MOBILE_BRAKEPOINT);
  isSidebarOpen = signal<boolean>(this.screenWidth() >= MOBILE_BRAKEPOINT);
  isSearchOpen = signal<boolean>(false);

  isCollapsed = computed<boolean>(
    () => !this.isMobile() && !this.isSidebarOpen(),
  );

  ngOnInit(): void {
    window.addEventListener('resize', () => {
      const width = window.innerWidth;
      this.screenWidth.set(width);
      if (width >= MOBILE_BRAKEPOINT) {
        this.isSidebarOpen.set(true);
      } else {
        this.isSidebarOpen.set(false);
      }
    });
  }

  toggleSidebar(): void {
    this.isSidebarOpen.update((x) => !x);
  }

  toggleSearch(): void {
    if (this.isMobile()) {
      this.isSearchOpen.update((x) => !x);
    }
  }

  closeSidebar(): void {
    this.isSidebarOpen.set(false);
  }
}
