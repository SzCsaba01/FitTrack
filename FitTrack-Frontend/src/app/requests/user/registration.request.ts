export interface RegistrationRequest {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  weightKg: number;
  heightCm: number;
}
