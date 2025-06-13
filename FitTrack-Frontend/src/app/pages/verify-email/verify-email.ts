import { Component, OnInit } from '@angular/core';
import { SelfUnsubscriberBase } from '../../utils/SelfUnsubscribeBase';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { takeUntil } from 'rxjs';

@Component({
  selector: 'app-verify-email',
  standalone: true,
  template: '',
})
export class VerifyEmail extends SelfUnsubscriberBase implements OnInit {
  constructor(
    private userService: UserService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) {
    super();
  }
  ngOnInit(): void {
    const token = this.activatedRoute.snapshot.paramMap.get('token') as string;

    this.userService
      .verifyEmailVerificationToken(token)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: (_) => {
          this.router.navigate(['/login']);
        },
      });
  }
}
