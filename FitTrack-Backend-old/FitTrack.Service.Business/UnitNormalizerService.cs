using FitTrack.Service.Contract;

namespace FitTrack.Service.Business;

public class UnitNormalizerService : IUnitNormalizerService
{
    public double ConvertInchToCm(double inch)
    {
        return Math.Round(inch * 2.54, 2);
    }

    public double ConvertCmToInch(double cm)
    {
        return Math.Round(cm / 2.54, 2);
    }

    public double ConvertLbToKg(double lb)
    {
        return Math.Round(lb * 0.453592, 2);
    }

    public double ConvertKgToLb(double kg)
    {
        return Math.Round(kg / 0.453592, 2);
    }
}
