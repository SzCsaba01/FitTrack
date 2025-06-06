using FitTrack.Service.Contract;

namespace FitTrack.Service.Business;

public class UnitNormalizerService : IUnitNormalizerService
{
    public double ConvertToCm(double inch)
    {
        return Math.Round(inch * 2.54, 2);
    }

    public double ConvertToKg(double lb)
    {
        return Math.Round(lb * 0.453592, 2);
    }
}
