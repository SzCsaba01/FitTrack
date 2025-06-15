import { Component, OnInit } from '@angular/core';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs';
import { UserService } from '../../services/user/user.service';
import { NotificationService } from '../../services/notification/notification.service';

@Component({
  selector: 'app-verify-email',
  standalone: true,
  template: '',
})
export class VerifyEmail extends SelfUnsubscriberBase implements OnInit {
  constructor(
    private userService: UserService,
    private notificationService: NotificationService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }
  ngOnInit(): void {
    const token = this.activatedRoute.snapshot.queryParamMap.get(
      'token',
    ) as string;
    console.log(token);

    this.userService
      .verifyEmailVerificationToken(token)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: (error) => {
          if (error?.errorMessage) {
            this.notificationService.showError(error.errorMessage, 10000000);
          }
          this.router.navigate(['/login']);
        },
      });
  }
}
