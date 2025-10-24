namespace FitTrack.Data.Contract.Helpers.Contract;

public class FoodImportDto
{
    public required string Name { get; set; }
    public required string Unit { get; set; }
    public required string Category { get; set; }
    public int Quantity { get; set; }
    public required FoodNutritionImportDto Nutriton { get; set; }
}
