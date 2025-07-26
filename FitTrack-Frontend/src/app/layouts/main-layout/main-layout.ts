import { Component, computed, OnInit, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MOBILE_BRAKEPOINT } from '../../constants/sizes.constant';
import { Store } from '@ngrx/store';
import { selectUserDetails } from '../../store/user/user.selectors';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { takeUntil } from 'rxjs';
import { NavbarDropdown } from '../../shared-components/navbar-dropdown/navbar-dropdown';
import { SidebarItem } from '../../models/sidebar-item.model';
import { SIDEBAR_SECTIONS } from '../../constants/sidebar-sections.constant';
import { SidebarSection } from '../../models/sidebar-section.model';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, MatIconModule, RouterModule, NavbarDropdown],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout extends SelfUnsubscriberBase implements OnInit {
  private screenWidth = signal(window.innerWidth);
  userInitials = signal<string | null>(null);
  isDropdownOpen = signal<boolean>(false);
  isMobile = computed<boolean>(() => this.screenWidth() < MOBILE_BRAKEPOINT);
  isSidebarOpen = signal<boolean>(this.screenWidth() >= MOBILE_BRAKEPOINT);
  isSearchOpen = signal<boolean>(false);
  isCollapsed = computed<boolean>(
    () => !this.isMobile() && !this.isSidebarOpen(),
  );
  sidebarSections = signal<SidebarSection[]>([]);
  userPermissions = signal<string[]>([]);

  constructor(private store: Store) {
    super();
  }

  ngOnInit(): void {
    this.store
      .select(selectUserDetails)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((user) => {
        if (user != null) {
          this.userPermissions.set(user.permissions || []);
          const first = user.firstName.charAt(0);
          const last = user.lastName.charAt(0);
          this.userInitials.set(`${first}${last}`.toUpperCase());

          const filteredSections = SIDEBAR_SECTIONS.map((section) => ({
            ...section,
            items: section.items.filter(
              (item) =>
                !item.permission || user.permissions?.includes(item.permission),
            ),
          })).filter((section) => section.items.length > 0);

          this.sidebarSections.set(filteredSections);
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

  onToggleDropdown(): void {
    this.isDropdownOpen.update((x) => !x);
  }

  onNavigate() {
    if (this.isMobile() && this.isSidebarOpen()) {
      this.isSidebarOpen.set(false);
    }
  }
}
