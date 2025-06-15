export interface ToastMessageModel {
  message: string;
  type: 'success' | 'error';
  duration?: number;
}
