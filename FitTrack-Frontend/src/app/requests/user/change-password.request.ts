export interface ChangePasswordRequest {
  changePasswordToken: string;
  password: string;
  confirmPassword: string;
}
