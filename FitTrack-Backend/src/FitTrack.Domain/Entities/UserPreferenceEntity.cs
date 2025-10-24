using FitTrack.Common.Enums;

namespace FitTrack.Domain.Entities;

public class UserPreferenceEntity : BaseEntity
{
	public Guid UserId { get; set; }
	public UnitSystemEnum UnitSystem { get; set; }
	public AppThemeEnum AppTheme { get; set; }
	public UserEntity? User { get; set; }
}
