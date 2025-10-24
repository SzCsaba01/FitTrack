namespace FitTrack.Data.Contract;

public class FoodNutritionImportDto
{
    public int Calories { get; set; }
    public Dictionary<string, FoodNutrientImportDto> Nutrients { get; set; }
}
