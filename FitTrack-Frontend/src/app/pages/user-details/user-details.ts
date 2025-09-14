import { Component, OnInit, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { ActivatedRoute, Router } from '@angular/router';
import { ManageUsersService } from '../../services/manage/users/manage-users.service';
import { GetUserDetailsResponse } from '../../responses/user/get-user-details.response';
import { Store } from '@ngrx/store';
import { filter, switchMap, takeUntil, tap } from 'rxjs';
import { selectUserDetails } from '../../store/user/user.selectors';
import { UnitSystemEnum } from '../../enums/unit-system.enum';
import { AuthenticationResponse } from '../../responses/authentication/authentication.response';
import { UserCard } from '../../shared-components/user-card/user-card';
import { UnitLabelPipe } from '../../helpers/pipes/unit-label.pipe';
import { GenderEnum } from '../../enums/gender.enum';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [MatIconModule, UserCard, UnitLabelPipe],
  templateUrl: './user-details.html',
  styleUrl: './user-details.scss',
})
export class UserDetails extends SelfUnsubscriberBase implements OnInit {
  selectedUserDetails = signal<GetUserDetailsResponse>(
    {} as GetUserDetailsResponse,
  );
  heightFt = signal<number>(0);
  heightIn = signal<number>(0);
  userUnitSystem = signal<UnitSystemEnum | null>(null);
  unitSystem = UnitSystemEnum;
  genderEnum = GenderEnum;
  selectedTab = signal<'overview' | 'workouts' | 'meals' | 'weight-logs'>(
    'overview',
  );

  constructor(
    private manageUsersService: ManageUsersService,
    private store: Store,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    super();
  }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {
    const userId = this.activatedRoute.snapshot.paramMap.get('id');
    if (!userId) {
      this.router.navigate(['/home/manage/users']);
    }

    this.store
      .select(selectUserDetails)
      .pipe(
        takeUntil(this.ngUnsubscribe),
        filter(
          (userDetails): userDetails is AuthenticationResponse => !!userDetails,
        ),
        tap((userDetails) => {
          this.userUnitSystem.set(userDetails.unitSystem);
        }),
        switchMap((userDetails) =>
          this.manageUsersService.getUserDetails(
            userId!,
            userDetails.unitSystem,
          ),
        ),
      )
      .subscribe((selectedUserDetails) => {
        this.selectedUserDetails.set(selectedUserDetails);
        if (this.userUnitSystem() === this.unitSystem.Imperial) {
          const ft = Math.floor(selectedUserDetails.height / 12);
          const inch = Math.round(selectedUserDetails.height % 12);
          this.heightFt.set(ft);
          this.heightIn.set(inch);
        }
      });
  }

  selectTab(tab: 'overview' | 'workouts' | 'meals' | 'weight-logs'): void {
    this.selectedTab.set(tab);
  }
}
