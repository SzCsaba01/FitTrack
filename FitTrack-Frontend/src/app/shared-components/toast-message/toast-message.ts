import { CommonModule } from '@angular/common';
import { Component, Signal } from '@angular/core';
import { NotificationService } from '../../services/notification/notification.service';
import { ToastMessageModel } from '../../models/toast-message.model';

@Component({
  selector: 'app-toast-message',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast-message.html',
  styleUrl: '/toast-message.scss',
})
export class ToastMessage {
  toasts: Signal<ToastMessageModel[]>;

  constructor(private notificationService: NotificationService) {
    this.toasts = this.notificationService.toasts;
  }

  remove(index: number) {
    this.notificationService.removeAtIndex(index);
  }
}
