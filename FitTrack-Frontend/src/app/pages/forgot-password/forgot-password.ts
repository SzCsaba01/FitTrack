import { Component, OnInit, signal } from '@angular/core';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { UserService } from '../../services/user/user.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { takeUntil } from 'rxjs';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.scss',
})
export class ForgotPassword extends SelfUnsubscriberBase implements OnInit {
  forgotPasswordForm: FormGroup = {} as FormGroup;
  inlineErrorMessageSignal = signal<string | null>('test');

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
  ) {
    super();
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.forgotPasswordForm = this.formBuilder.group({
      email: new FormControl('', [
        Validators.required,
        Validators.email,
        Validators.maxLength(50),
      ]),
    });
  }

  get email(): FormControl {
    return this.forgotPasswordForm.get('email') as FormControl;
  }

  onSendEmail(data: { email: string }): void {
    this.userService
      .sendForgotPasswordEmail(data.email)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.forgotPasswordForm.reset();
        },
        error: (error) => {
          if (error?.errorMessage) {
            this.inlineErrorMessageSignal = error.errorMessage;
          }
        },
      });
  }
}
