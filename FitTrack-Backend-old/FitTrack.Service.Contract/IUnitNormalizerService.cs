namespace FitTrack.Service.Contract;

public interface IUnitNormalizerService
{
    public double ConvertLbToKg(double lb);
    public double ConvertKgToLb(double kg);
    public double ConvertInchToCm(double inch);
    public double ConvertCmToInch(double cm);
}
