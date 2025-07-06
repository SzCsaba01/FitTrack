import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { ToastMessage } from './shared-components/toast-message/toast-message';
import { SelfUnsubscriberBase } from './utils/SelfUnsubscribeBase';
import { finalize, takeUntil } from 'rxjs';
import { Store } from '@ngrx/store';
import { UserActions } from './store/user/user.actions';
import { InitialLoader } from './shared-components/initial-loader/initial-loader';
import { UserService } from './services/user/user.service';
import { ThemeService } from './services/theme/theme.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ToastMessage, InitialLoader],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App extends SelfUnsubscriberBase implements OnInit {
  isInitialLoaderShown = signal<boolean>(true);

  constructor(
    private userService: UserService,
    private themeService: ThemeService,
    private router: Router,
    private store: Store,
  ) {
    super();
  }

  ngOnInit(): void {
    this.userService
      .getUserData()
      .pipe(
        takeUntil(this.ngUnsubscribe),
        finalize(() => {
          this.isInitialLoaderShown.set(false);
        }),
      )
      .subscribe({
        next: (response) => {
          this.store.dispatch(UserActions.setUser({ userDetails: response }));
          if (this.themeService.getTheme() != response.appTheme) {
            this.themeService.loadTheme(response.appTheme);
          }
        },
        error: (_) => {
          this.router.navigate(['/login']);
        },
      });
  }
}
