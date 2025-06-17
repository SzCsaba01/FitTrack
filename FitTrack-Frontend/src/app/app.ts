import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { ToastMessage } from './shared-components/toast-message/toast-message';
import { SelfUnsubscriberBase } from './utils/SelfUnsubscribeBase';
import { AuthenticationService } from './services/authentication/authentication.service';
import { takeUntil } from 'rxjs';
import { Store } from '@ngrx/store';
import { UserActions } from './store/user/user.actions';
import { InitialLoader } from './shared-components/initial-loader/initial-loader';

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
    private authenticationService: AuthenticationService,
    private router: Router,
    private store: Store,
  ) {
    super();
  }

  ngOnInit(): void {
    console.log(this.isInitialLoaderShown());
    this.authenticationService
      .getUserData()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.store.dispatch(UserActions.setUser(response));
          this.router.navigate(['/home']);
          this.isInitialLoaderShown.set(false);
        },
        error: (_) => {
          this.router.navigate(['/login']);
          this.isInitialLoaderShown.set(false);
        },
      });
  }
}
