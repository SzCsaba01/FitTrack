using AutoMapper;
using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;
using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract.Helpers;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<RegistrationRequest, UserEntity>();

        CreateMap<RegistrationRequest, UserProfileEntity>();

        CreateMap<UserEntity, AuthenticationResponse>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserProfile.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserProfile.LastName))
            .ForMember(dest => dest.UnitSystem, opt => opt.MapFrom(src => src.UserPreference.UnitSystem))
            .ForMember(dest => dest.AppTheme, opt => opt.MapFrom(src => src.UserPreference.AppTheme))
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Role.PermissionMappings.Select(x => x.Permission.Name).ToList()));

        CreateMap<UserProfileEntity, UserProfileResponse>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.WeightKg))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.HeightCm));
    }
}
