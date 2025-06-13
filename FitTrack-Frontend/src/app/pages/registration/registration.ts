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
import { passwordFormat, usernameFormat } from '../../formats/formats';
import { RegistrationRequest } from '../../requests/user/registration.request';
import { confirmPasswordValidator } from '../../validators/confirm-password.validator';
import { AppThemeEnum } from '../../enums/app-theme.enum';
import { UnitSystemEnum } from '../../enums/unit-system.enum';
import { Router, RouterModule } from '@angular/router';
import { ThemeService } from '../../services/theme/theme.service';
import { MatIconModule } from '@angular/material/icon';
import { Select } from '../../shared-components/select/select';
import { GENDERS } from '../../constants/genders.constant';
import { DatePicker } from '../../shared-components/date-picker/date-picker';
import { UserService } from '../../services/user.service';
import { takeUntil } from 'rxjs';

@Component({
  selector: 'app-registration',
  imports: [
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    Select,
    DatePicker,
  ],
  templateUrl: './registration.html',
  styleUrl: './registration.scss',
})
export class Registration extends SelfUnsubscriberBase implements OnInit {
  registrationForm: FormGroup = {} as FormGroup;
  appThemeEnum = AppThemeEnum;
  unitSystemEnum = UnitSystemEnum;
  genderOptions = GENDERS;

  constructor(
    private formBuilder: FormBuilder,
    private themeService: ThemeService,
    private router: Router,
    private userService: UserService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.initialzeForm();
    this.getCurrentTheme();
  }

  private initialzeForm(): void {
    this.registrationForm = this.formBuilder.group(
      {
        username: new FormControl('', [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(30),
          Validators.pattern(usernameFormat),
        ]),
        email: new FormControl('', [
          Validators.required,
          Validators.maxLength(50),
          Validators.email,
        ]),
        password: new FormControl('', [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(50),
          Validators.pattern(passwordFormat),
        ]),
        confirmPassword: new FormControl('', [Validators.required]),
        firstName: new FormControl('', [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100),
        ]),
        lastName: new FormControl('', [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100),
        ]),
        gender: new FormControl('', [Validators.required]),
        dateOfBirth: new FormControl(null, [Validators.required]),
        weightKg: new FormControl(null, [
          Validators.required,
          Validators.min(1),
        ]),
        heightCm: new FormControl(null, [
          Validators.required,
          Validators.min(1),
        ]),
        unitSystem: new FormControl(UnitSystemEnum.Metric, [
          Validators.required,
        ]),
        appTheme: new FormControl(AppThemeEnum.Light, [Validators.required]),
      },
      { validators: confirmPasswordValidator },
    );
  }

  get username(): FormControl {
    return this.registrationForm.get('username') as FormControl;
  }

  get email(): FormControl {
    return this.registrationForm.get('email') as FormControl;
  }

  get password(): FormControl {
    return this.registrationForm.get('password') as FormControl;
  }

  get confirmPassword(): FormControl {
    return this.registrationForm.get('confirmPassword') as FormControl;
  }

  get firstName(): FormControl {
    return this.registrationForm.get('firstName') as FormControl;
  }

  get lastName(): FormControl {
    return this.registrationForm.get('lastName') as FormControl;
  }

  get gender(): FormControl {
    return this.registrationForm.get('gender') as FormControl;
  }

  get dateOfBirth(): FormControl {
    return this.registrationForm.get('dateOfBirth') as FormControl;
  }

  get weight(): FormControl {
    return this.registrationForm.get('weightKg') as FormControl;
  }

  get height(): FormControl {
    return this.registrationForm.get('heightCm') as FormControl;
  }

  get unitSystem(): FormControl {
    return this.registrationForm.get('unitSystem') as FormControl;
  }

  get appTheme(): FormControl {
    return this.registrationForm.get('appTheme') as FormControl;
  }

  private getCurrentTheme(): void {
    const currentTheme = this.themeService.getStoredTheme();

    if (currentTheme == null) {
      this.appTheme.setValue(AppThemeEnum.Light);
    } else {
      this.appTheme.setValue(currentTheme);
    }
  }

  setTheme(theme: AppThemeEnum): void {
    this.appTheme.setValue(theme);
    this.themeService.loadTheme(theme);
  }

  onRegistration(registrationRequest: RegistrationRequest): void {
    console.log(registrationRequest);
    this.userService
      .register(registrationRequest)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.router.navigate(['/login']);
      });
  }
}
