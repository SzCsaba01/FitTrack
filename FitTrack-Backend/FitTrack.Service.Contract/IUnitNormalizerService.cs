namespace FitTrack.Service.Contract;

public interface IUnitNormalizerService
{
    public double ConvertToKg(double lb);
    public double ConvertToCm(double inch);
}
