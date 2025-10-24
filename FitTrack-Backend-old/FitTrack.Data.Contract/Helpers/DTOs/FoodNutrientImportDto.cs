namespace FitTrack.Data.Contract;

public class FoodNutrientImportDto
{
    public required string Value { get; set; }
    public required string Unit { get; set; }
    public bool LessThan { get; set; }
    public bool MoreThan { get; set; }
}
