import { Injectable, signal, computed } from '@angular/core';
import { ToastMessageModel } from '../../models/toast-message.model';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private _toasts = signal<ToastMessageModel[]>([]);

  readonly toasts = computed(() => this._toasts());

  private show(
    message: string,
    type: ToastMessageModel['type'],
    duration = 4500,
  ) {
    const toast: ToastMessageModel = { message, type, duration };
    this._toasts.update((toasts) => [...toasts, toast]);

    setTimeout(() => this.remove(toast), duration);
  }

  private remove(toast: ToastMessageModel) {
    this._toasts.update((toasts) => toasts.filter((t) => t !== toast));
  }

  removeAtIndex(index: number) {
    const all = this._toasts();
    all.splice(index, 1);
    this._toasts.set([...all]);
  }

  showSuccess(msg: string, duration?: number) {
    this.show(msg, 'success', duration);
  }

  showError(msg: string, duration?: number) {
    this.show(msg, 'error', duration);
  }
}
