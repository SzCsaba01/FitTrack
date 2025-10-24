using FitTrack.Service.Business;
using FitTrack.Service.Contract;

namespace FitTrack.Test.Unit.ServiceTests;
public class UnitNormalizerServiceTests
{
    private readonly IUnitNormalizerService _unitNormalizerService;

    public UnitNormalizerServiceTests()
    {
        _unitNormalizerService = new UnitNormalizerService();
    }

    [Theory(DisplayName = "ConvertToCm returns correct value")]
    [InlineData(0, 0)]
    [InlineData(1, 2.54)]
    [InlineData(5.5, 13.97)]
    [InlineData(72, 182.88)]
    public void ConvertToCm_ValidInches_ReturnsExpectedCm(double inch, double expectedCm)
    {
        // Act
        var result = _unitNormalizerService.ConvertInchToCm(inch);

        // Assert
        Assert.Equal(expectedCm, result, precision: 2);
    }

    [Theory(DisplayName = "ConvertToKg returns correct value")]
    [InlineData(0, 0)]
    [InlineData(1, 0.45)]
    [InlineData(2.2, 1.0)]
    [InlineData(150, 68.04)]
    public void ConvertToKg_ValidPounds_ReturnsExpectedKg(double lb, double expectedKg)
    {
        // Act
        var result = _unitNormalizerService.ConvertLbToKg(lb);

        // Assert
        Assert.Equal(expectedKg, result, precision: 2);
    }
}
