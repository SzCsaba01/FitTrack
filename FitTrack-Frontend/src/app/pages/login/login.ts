import { Component, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { LoginRequest } from '../../requests/authentication/login.request';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { takeUntil } from 'rxjs';
import { Store } from '@ngrx/store';
import { UserActions } from '../../store/user/user.actions';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login extends SelfUnsubscriberBase implements OnInit {
  loginForm: FormGroup = {} as FormGroup;

  inlineErrorMessage = signal<string | null>(null);

  constructor(
    private authenticationService: AuthenticationService,
    private formBuilder: FormBuilder,
    private router: Router,
    private store: Store,
  ) {
    super();
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.loginForm = this.formBuilder.group({
      credential: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  get credential(): FormControl {
    return this.loginForm.get('credential') as FormControl;
  }

  get password(): FormControl {
    return this.loginForm.get('password') as FormControl;
  }

  onLoginClick() {
    const authenticationRequest = this.loginForm.value as LoginRequest;
    this.authenticationService
      .login(authenticationRequest)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.store.dispatch(UserActions.setUser({ userDetails: response }));
          this.router.navigate(['/home']);
        },
        error: (error) => {
          if (error.errorMessage) {
            this.inlineErrorMessage.set(error.errorMessage);
          }
        },
      });
  }
}
