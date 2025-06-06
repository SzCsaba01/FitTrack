using AutoMapper;
using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Object.Entities;

namespace FitTrack.Data.Contract.Helpers;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<RegistrationRequest, UserEntity>();

        CreateMap<RegistrationRequest, UserProfileEntity>();
    }
}
