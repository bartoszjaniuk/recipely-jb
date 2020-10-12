using System.Linq;
using API.Dto;
using API.Dto.Recipe;
using API.Dto.Recipe.RecipePhoto;
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
            CreateMap<AppUser, MemberDto>()
                .ForMember(destination => destination.PhotoUrl, options => options.MapFrom(source => source.UserPhotos
                .FirstOrDefault(p => p.IsMain).Url))
                .ForMember(destination => destination.Age, options => options.MapFrom(source => source.DateOfBirth.CalculateAge()));
                
            CreateMap<UserPhoto, UserPhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();


            CreateMap<Recipe, RecipeForListDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt
                .MapFrom(src => src.RecipePhotos
                    .FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Category , opt => opt
            .MapFrom(src => src.Category.Name));

             CreateMap<Recipe, RecipeForDetailDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt
                .MapFrom(src => src.RecipePhotos
                    .FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Ingredients , opt => opt
            .MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.Author , opt => opt
            .MapFrom(src => src.AppUser.UserName))
            .ForMember(dest => dest.Category , opt => opt
            .MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.KitchenOrigin , opt => opt
            .MapFrom(src => src.KitchenOrigin.Name));
            
            CreateMap<Ingredient, IngredientDto>();
            CreateMap<IngredientDto, Ingredient>();



            CreateMap<Recipe, RecipeForCreateDto>()
            .ForMember(dest => dest.Ingredients , opt => opt
            .MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.AuthorId , opt => opt
            .MapFrom(src => src.AppUser.Id))
            .ForMember(dest => dest.CategoryId , opt => opt
            .MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.KitchenOriginId , opt => opt
            .MapFrom(src => src.KitchenOriginId));

            CreateMap<RecipeForCreateDto, Recipe>();


            CreateMap<RecipePhoto, RecipePhotoForDetailDto>(); 
             

           


            
        }
    }
}