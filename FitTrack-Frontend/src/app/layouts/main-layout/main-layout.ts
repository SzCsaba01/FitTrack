import { Component, computed, OnInit, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MOBILE_BRAKEPOINT } from '../../constants/sizes.constant';
import { Store } from '@ngrx/store';
import { selectUserDetails } from '../../store/user/user.selectors';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { takeUntil } from 'rxjs';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, MatIconModule, RouterModule],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout extends SelfUnsubscriberBase implements OnInit {
  private screenWidth = signal(window.innerWidth);
  userInitials = signal<string | null>(null);
  isMobile = computed<boolean>(() => this.screenWidth() < MOBILE_BRAKEPOINT);
  isSidebarOpen = signal<boolean>(this.screenWidth() >= MOBILE_BRAKEPOINT);
  isSearchOpen = signal<boolean>(false);
  isCollapsed = computed<boolean>(
    () => !this.isMobile() && !this.isSidebarOpen(),
  );

  constructor(private store: Store) {
    super();
  }

  ngOnInit(): void {
    this.store
      .select(selectUserDetails)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((user) => {
        if (user != null) {
          const first = user.firstName.charAt(0);
          const last = user.lastName.charAt(0);
          this.userInitials.set(`${first}${last}`.toUpperCase());
        } else {
          this.userInitials.set(null);
        }
      });
    window.addEventListener('resize', () => {
      const width = window.innerWidth;
      this.screenWidth.set(width);
      if (width >= MOBILE_BRAKEPOINT) {
        this.isSidebarOpen.set(true);
        this.isSearchOpen.set(false);
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
