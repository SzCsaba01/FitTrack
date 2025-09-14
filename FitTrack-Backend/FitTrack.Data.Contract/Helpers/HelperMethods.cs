namespace FitTrack.Data.Contract.Helpers;

public static class HelperMethods
{
    public static T ParseEnum<T>(string value, T defaultValue = default) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        return Enum.TryParse<T>(value, true, out var result)
            ? result
            : defaultValue;
    }
}

