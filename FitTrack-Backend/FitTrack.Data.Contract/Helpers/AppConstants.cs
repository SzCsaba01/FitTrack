namespace FitTrack.Data.Contract.Helpers;

public static class AppConstants
{
    //URL
    public const string VERIFY_EMAIL_URL = "/verify-email";
    public const string CHANGE_PASSWORD_URL = "/change-password";

    //SUBJECTS
    public const string EMAIL_VERIFICATION_SUBJECT = "FitTrack - Email Verification";
    public const string CHANGE_PASSWORD_SUBJECT = "FitTrack - Change Password";

    //TOKEN
    public const int ACCESS_TOKEN_VALIDATION_TIME_MINUTES = 30;
    public const int REGISTRATION_TOKEN_VALIDATION_TIME_HOURS = 24;
    public const int CHANGE_PASSWORD_TOKEN_VALIDATION_TIME_MINUTES = 30;
    public const int REFRESH_TOKEN_VALIDATION_TIME_DAYS = 7;
}
