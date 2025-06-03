using Microsoft.EntityFrameworkCore;
using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Access.Data;

public class FitTrackContext : DbContext
{
    public FitTrackContext(DbContextOptions<FitTrackContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Enum conversions
        modelBuilder.Entity<UserPreferenceEntity>(entity =>
        {
            entity.Property(e => e.WeightUnit).HasConversion<string>();
            entity.Property(e => e.DistanceUnit).HasConversion<string>();
            entity.Property(e => e.VolumeUnit).HasConversion<string>();
            entity.Property(e => e.AppTheme).HasConversion<string>();

        });

        modelBuilder.Entity<UserProfileEntity>(entity =>
        {
            entity.Property(e => e.Gender).HasConversion<string>();
        });

        modelBuilder.Entity<ExerciseEntity>(entity =>
        {
            entity.Property(e => e.Force).HasConversion<string>();
            entity.Property(e => e.Level).HasConversion<string>();
            entity.Property(e => e.Mechanic).HasConversion<string>();
            entity.Property(e => e.Category).HasConversion<string>();
        });

        modelBuilder.Entity<FoodNutritionEntity>(entity =>
        {
            entity.Property(e => e.Unit).HasConversion<string>();
            entity.Property(e => e.Name).HasConversion<string>();
        });

        modelBuilder.Entity<RecipeNutritionEntity>(entity =>
        {
            entity.Property(e => e.Unit).HasConversion<string>();
            entity.Property(e => e.Name).HasConversion<string>();
        });


        modelBuilder.Entity<UserMealEntity>(entity =>
        {
            entity.Property(e => e.Type).HasConversion<string>();
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.Property(e => e.RoleName).HasConversion<string>();
        });


        //Composite keys
        modelBuilder.Entity<RolePermissionMapping>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermissionMapping>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermissionMapping>()
            .HasOne(rp => rp.Permission)
            .WithMany(r => r.Roles)
            .HasForeignKey(rp => rp.PermissionId);

        modelBuilder.Entity<ExercisePrimaryMuscleMapping>()
            .HasKey(epm => new { epm.ExerciseId, epm.MuscleId });

        modelBuilder.Entity<ExercisePrimaryMuscleMapping>()
            .HasOne(epm => epm.Exercise)
            .WithMany(e => e.PrimaryMuscles)
            .HasForeignKey(epm => epm.ExerciseId);

        modelBuilder.Entity<ExercisePrimaryMuscleMapping>()
            .HasOne(epm => epm.Muscle)
            .WithMany(m => m.PrimaryExercises)
            .HasForeignKey(epm => epm.MuscleId);

        modelBuilder.Entity<ExerciseSecondaryMuscleMapping>()
            .HasKey(epm => new { epm.ExerciseId, epm.MuscleId });

        modelBuilder.Entity<ExerciseSecondaryMuscleMapping>()
            .HasOne(epm => epm.Exercise)
            .WithMany(e => e.SecondaryMuscles)
            .HasForeignKey(epm => epm.ExerciseId);

        modelBuilder.Entity<ExerciseSecondaryMuscleMapping>()
            .HasOne(epm => epm.Muscle)
            .WithMany(m => m.SecondaryExercises)
            .HasForeignKey(epm => epm.MuscleId);

        //Indexes
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.RefreshToken).IsUnique();
        });

        modelBuilder.Entity<ExerciseEntity>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Mechanic);
            entity.HasIndex(e => e.Force);
        });

        modelBuilder.Entity<FoodEntity>(entity =>
        {
            entity.HasIndex(f => f.Name).IsUnique();
        });

        modelBuilder.Entity<RecipeEntity>(entity =>
        {
            entity.HasIndex(r => r.Name).IsUnique();
        });
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserProfileEntity> UserProfiles { get; set; }
    public DbSet<UserPreferenceEntity> UserPreferences { get; set; }
    public DbSet<UserWeightLogEntity> UserWeightLogs { get; set; }
    public DbSet<UserMealEntity> UserMeals { get; set; }
    public DbSet<MealItemEntity> MealItems { get; set; }
    public DbSet<UserWorkoutEntity> UserWorkouts { get; set; }
    public DbSet<WorkoutExerciseSetEntity> WorkoutExerciseSets { get; set; }
    public DbSet<WorkoutCardioExerciseEntity> WorkoutCardioExercises { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<PermissionEntity> Permissions { get; set; }
    public DbSet<RolePermissionMapping> RolePermissionMappings { get; set; }
    public DbSet<ExerciseEntity> Exercises { get; set; }
    public DbSet<ExercisePrimaryMuscleMapping> ExercisePrimaryMuscleMappings { get; set; }
    public DbSet<ExerciseSecondaryMuscleMapping> ExerciseSecondaryMuscleMappings { get; set; }
    public DbSet<EquipmentEntity> Equipments { get; set; }
    public DbSet<FoodEntity> Foods { get; set; }
    public DbSet<FoodCategoryEntity> FoodCategories { get; set; }
    public DbSet<FoodNutritionEntity> FoodNutritions { get; set; }
    public DbSet<IngredientEntity> Ingredients { get; set; }
    public DbSet<IngredientUnitEntity> IngredientUnits { get; set; }
    public DbSet<InstructionEntity> Instructions { get; set; }
    public DbSet<MuscleEntity> Muscles { get; set; }
    public DbSet<RecipeCategoryEntity> RecipeCategories { get; set; }
    public DbSet<RecipeEntity> Recipes { get; set; }
    public DbSet<RecipeIngredientEntity> RecipeIngredients { get; set; }
    public DbSet<RecipeDirectionEntity> RecipeDirections { get; set; }
    public DbSet<RecipeNutritionEntity> RecipeNutritions { get; set; }
}
