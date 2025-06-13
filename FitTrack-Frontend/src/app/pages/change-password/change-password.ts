import { Component, OnInit } from '@angular/core';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { takeUntil } from 'rxjs';
import { ChangePasswordRequest } from '../../requests/user/change-password.request';
import { confirmPasswordValidator } from '../../validators/confirm-password.validator';
import { passwordFormat } from '../../formats/formats';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.scss',
})
export class ChangePassword extends SelfUnsubscriberBase implements OnInit {
  private token = '';
  changePasswordForm: FormGroup = {} as FormGroup;

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    super();
  }
  ngOnInit(): void {
    // this.verifyToken();
    this.initializeForm();
  }

  private initializeForm(): void {
    this.changePasswordForm = this.formBuilder.group(
      {
        password: new FormControl('', [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(50),
          Validators.pattern(passwordFormat),
        ]),
        confirmPassword: new FormControl('', [Validators.required]),
      },
      { validators: confirmPasswordValidator },
    );
  }

  get password(): FormControl {
    return this.changePasswordForm.get('password') as FormControl;
  }

  get confirmPassword(): FormControl {
    return this.changePasswordForm.get('confirmPassword') as FormControl;
  }

  private verifyToken(): void {
    this.token = this.activatedRoute.snapshot.paramMap.get('token') as string;

    this.userService
      .verifyChangePasswordToken(this.token)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        error: (_) => {
          this.router.navigate(['/login']);
        },
      });
  }

  onChangePassword(changePasswordRequest: ChangePasswordRequest): void {
    changePasswordRequest.changePasswordToken = this.token;

    this.userService
      .changePassword(changePasswordRequest)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.router.navigate(['/login']);
      });
  }
}
