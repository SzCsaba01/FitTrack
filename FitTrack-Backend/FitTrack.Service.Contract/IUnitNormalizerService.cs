namespace FitTrack.Service.Contract;

public interface IUnitNormalizerService
{
    public double ConvertToKg(double lb);
    public double ConvertToLb(double kg);
    public double ConvertToCm(double inch);
    public double ConvertToInch(double cm);
}
