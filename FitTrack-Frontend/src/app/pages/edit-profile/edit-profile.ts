import { Component, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { GENDERS } from '../../constants/genders.constant';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { Store } from '@ngrx/store';
import { filter, switchMap, takeUntil, tap } from 'rxjs';
import { UserProfileResponse } from '../../responses/user-profile/user-profile.response';
import { selectUserDetails } from '../../store/user/user.selectors';
import { AuthenticationResponse } from '../../responses/authentication/authentication.response';
import { MatIconModule } from '@angular/material/icon';
import { Select } from '../../shared-components/select/select';
import { DatePicker } from '../../shared-components/date-picker/date-picker';
import { UnitSystemEnum } from '../../enums/unit-system.enum';
import { UnitLabelPipe } from '../../helpers/pipes/unit-label.pipe';
import { UpdateUserProfileRequest } from '../../requests/user-profile/update-user-profile.request';
import { UserProfileService } from '../../services/user-profile/user-profile.service';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    Select,
    DatePicker,
    UnitLabelPipe,
  ],
  templateUrl: './edit-profile.html',
  styleUrl: './edit-profile.scss',
})
export class EditProfile extends SelfUnsubscriberBase implements OnInit {
  editProfileForm: FormGroup = {} as FormGroup;
  genderOptions = GENDERS;
  userUnitSystem = signal<UnitSystemEnum | null>(null);
  unitSystem = UnitSystemEnum;
  username = signal<string>('');
  email = signal<string>('');

  constructor(
    private userProfileService: UserProfileService,
    private store: Store,
    private formBuilder: FormBuilder,
  ) {
    super();
  }

  ngOnInit(): void {
    this.initializeForm();
    this.loadData();
  }

  private loadData(): void {
    this.store
      .select(selectUserDetails)
      .pipe(
        takeUntil(this.ngUnsubscribe),
        filter(
          (userDetails): userDetails is AuthenticationResponse => !!userDetails,
        ),
        tap((userDetails) => {
          this.userUnitSystem.set(userDetails.unitSystem);

          if (this.userUnitSystem() === UnitSystemEnum.Metric) {
            this.height.setValidators([Validators.required, Validators.min(1)]);
            this.heightFt.clearValidators();
            this.heightIn.clearValidators();
          } else {
            this.height.clearValidators();
            this.heightFt.setValidators([
              Validators.required,
              Validators.min(1),
            ]);
            this.heightIn.setValidators([
              Validators.required,
              Validators.min(0),
              Validators.max(11),
            ]);
          }

          this.height.updateValueAndValidity();
          this.heightFt.updateValueAndValidity();
          this.heightIn.updateValueAndValidity();
        }),
        switchMap((userDetails) =>
          this.userProfileService.getUserProfile(userDetails.unitSystem),
        ),
      )
      .subscribe((userProfile) => {
        this.patchData(userProfile);
      });
  }

  private initializeForm(): void {
    this.editProfileForm = this.formBuilder.group({
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
      weight: new FormControl(null, [Validators.required, Validators.min(1)]),
      height: new FormControl(null),
      heightFt: new FormControl(null),
      heightIn: new FormControl(null),
    });
  }

  private patchData(userProfile: UserProfileResponse): void {
    this.username.set(userProfile.username);
    this.email.set(userProfile.email);

    if (this.userUnitSystem() === UnitSystemEnum.Imperial) {
      const ft = Math.floor(userProfile.height / 12);
      const inch = Math.round(userProfile.height % 12);
      this.editProfileForm.patchValue({
        heightFt: ft,
        heightIn: inch,
      });
    } else {
      this.editProfileForm.patchValue({
        height: userProfile.height,
      });
    }

    this.editProfileForm.patchValue({
      firstName: userProfile.firstName,
      lastName: userProfile.lastName,
      gender: userProfile.gender,
      dateOfBirth: userProfile.dateOfBirth,
      weight: userProfile.weight,
    });
  }

  get firstName(): FormControl {
    return this.editProfileForm.get('firstName') as FormControl;
  }

  get lastName(): FormControl {
    return this.editProfileForm.get('lastName') as FormControl;
  }

  get gender(): FormControl {
    return this.editProfileForm.get('gender') as FormControl;
  }

  get dateOfBirth(): FormControl {
    return this.editProfileForm.get('dateOfBirth') as FormControl;
  }

  get weight(): FormControl {
    return this.editProfileForm.get('weight') as FormControl;
  }

  get height(): FormControl {
    return this.editProfileForm.get('height') as FormControl;
  }

  get heightIn(): FormControl {
    return this.editProfileForm.get('heightIn') as FormControl;
  }

  get heightFt(): FormControl {
    return this.editProfileForm.get('heightFt') as FormControl;
  }

  onEditProfile(): void {
    const formData = this.editProfileForm.value;

    const requestData: UpdateUserProfileRequest = {
      ...formData,
      height:
        this.userUnitSystem() === UnitSystemEnum.Imperial
          ? Number(formData.heightFt || 0) * 12 + Number(formData.heightIn || 0)
          : Number(formData.height),
    };

    this.userProfileService
      .updateUserProfile(this.userUnitSystem()!, requestData)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.editProfileForm.markAsPristine();
      });
  }
}
