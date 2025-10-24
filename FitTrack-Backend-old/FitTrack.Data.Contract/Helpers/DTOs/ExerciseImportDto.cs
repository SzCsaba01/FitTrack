namespace FitTrack.Data.Contract.Helpers.DTOs;

public class ExerciseImportDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Force { get; set; }
    public required string Level { get; set; }
    public string? Mechanic { get; set; }
    public string? Equipment { get; set; }
    public List<string>? PrimaryMuscles { get; set; }
    public List<string>? SecondaryMuscles { get; set; }
    public List<string>? Instructions { get; set; }
    public required string Category { get; set; }
    public List<string>? Images { get; set; }
}
