import { Component, computed, effect, OnInit, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule, RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectUserDetails } from '../../store/user/user.selectors';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { takeUntil } from 'rxjs';
import { NavbarDropdown } from '../../shared-components/navbar-dropdown/navbar-dropdown';
import { SIDEBAR_SECTIONS } from '../../constants/sidebar-sections.constant';
import { SidebarSection } from '../../models/sidebar-section.model';
import { ScreenService } from '../../services/screen/screen.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, MatIconModule, RouterModule, NavbarDropdown],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout extends SelfUnsubscriberBase implements OnInit {
  userInitials = signal<string | null>(null);
  isDropdownOpen = signal<boolean>(false);
  isMobile = signal<boolean>(false);
  isSidebarOpen = signal<boolean>(false);
  isSearchOpen = signal<boolean>(false);
  isCollapsed = computed<boolean>(
    () => !this.isMobile() && !this.isSidebarOpen(),
  );
  sidebarSections = signal<SidebarSection[]>([]);
  userPermissions = signal<string[]>([]);

  constructor(
    private store: Store,
    private screenService: ScreenService,
  ) {
    super();
    effect(() => {
      this.isMobile.set(this.screenService.isMobile());
      this.isSidebarOpen.set(!this.screenService.isMobile());
    });
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
  }

  onToggleSidebarClick(): void {
    this.isSidebarOpen.update((x) => !x);
  }

  onToggleSearchClick(): void {
    if (this.isMobile()) {
      this.isSearchOpen.update((x) => !x);
    }
  }

  onCloseSidebarClick(): void {
    this.isSidebarOpen.set(false);
  }

  onToggleDropdownClick(): void {
    this.isDropdownOpen.update((x) => !x);
  }

  onNavigateClick() {
    if (this.isMobile() && this.isSidebarOpen()) {
      this.isSidebarOpen.set(false);
    }
  }
}
