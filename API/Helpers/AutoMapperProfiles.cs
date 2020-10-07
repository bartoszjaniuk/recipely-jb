using System.Linq;
using API.Dto;
using API.Dto.User;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // <MapFrom, MapTo>
            CreateMap<AppUser, UserToReturnDto>()
                .ForMember(destination => destination.PhotoUrl, options => options.MapFrom(source => source.UserPhotos
                .FirstOrDefault(p => p.IsMain).Url))
                .ForMember(destination => destination.Age, options => options.MapFrom(source => source.DateOfBirth.CalculateAge()));
                
            CreateMap<UserPhoto, UserPhotoDto>();
        }
    }
}