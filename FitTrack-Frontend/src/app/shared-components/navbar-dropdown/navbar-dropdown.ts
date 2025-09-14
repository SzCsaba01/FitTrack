import {
  Component,
  ElementRef,
  HostListener,
  OnInit,
  output,
  signal,
} from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterModule } from '@angular/router';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { Store } from '@ngrx/store';
import { AppThemeEnum } from '../../enums/app-theme.enum';
import { UnitSystemEnum } from '../../enums/unit-system.enum';
import { selectUserDetails } from '../../store/user/user.selectors';
import { takeUntil } from 'rxjs';
import { UserPreferenceService } from '../../services/user-preference/user-preference.service';
import { UserActions } from '../../store/user/user.actions';
import { AuthenticationService } from '../../services/authentication/authentication.service';

@Component({
  selector: 'app-navbar-dropdown',
  standalone: true,
  imports: [RouterModule, MatIconModule],
  templateUrl: './navbar-dropdown.html',
  styleUrl: './navbar-dropdown.scss',
})
export class NavbarDropdown extends SelfUnsubscriberBase implements OnInit {
  onCloseDropdown = output<void>();
  theme = signal<AppThemeEnum | null>(null);
  unitSystem = signal<UnitSystemEnum | null>(null);
  isSelectThemeShown = signal<boolean>(false);
  isSelectUnitSystemShown = signal<boolean>(false);
  appThemeEnum = AppThemeEnum;
  unitSystemEnum = UnitSystemEnum;

  constructor(
    private authenticationService: AuthenticationService,
    private userPreferenceService: UserPreferenceService,
    private router: Router,
    private store: Store,
    private elementRef: ElementRef<HTMLElement>,
  ) {
    super();
  }

  ngOnInit(): void {
    this.store
      .select(selectUserDetails)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((user) => {
        if (user) {
          this.theme.set(user.appTheme);
          this.unitSystem.set(user.unitSystem);
        }
      });
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const clickedInside = this.elementRef.nativeElement.contains(
      event.target as Node,
    );
    if (!clickedInside) {
      this.onCloseDropdown.emit();
    }
  }

  toggleSelectTheme(): void {
    this.isSelectThemeShown.update((x) => !x);
  }

  toggleSelectUnitSystem(): void {
    this.isSelectUnitSystemShown.update((x) => !x);
  }

  setTheme(theme: AppThemeEnum): void {
    if (this.theme() === theme) {
      return;
    }

    this.userPreferenceService
      .updateTheme(theme)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.store.dispatch(UserActions.updateAppTheme({ theme: theme }));
      });
  }

  setUnitSystem(unitSystem: UnitSystemEnum): void {
    if (this.unitSystem() === unitSystem) {
      return;
    }

    this.userPreferenceService
      .updateUnitSystem(unitSystem)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.store.dispatch(
          UserActions.updateUnitSystem({ unitSystem: unitSystem }),
        );
      });
  }

  onLogout(): void {
    this.authenticationService
      .logout()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.store.dispatch(UserActions.clearUser());
        this.router.navigate(['/login']);
      });
  }

  closeDropdown(): void {
    this.onCloseDropdown.emit();
  }
}
