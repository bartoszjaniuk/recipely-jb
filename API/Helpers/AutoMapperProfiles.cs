using System;
using System.Linq;
using API.Dto;
using API.Dto.Comment;
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
            .ForMember(dest => dest.Category, opt => opt
           .MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.KitchenOrigin, opt => opt
           .MapFrom(src => src.KitchenOrigin.Name));

            CreateMap<FavouriteRecipe, FavouriteRecipeDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt
                 .MapFrom(src => src.Recipe.RecipePhotos
                     .FirstOrDefault(p => p.IsMain).Url))
             .ForMember(dest => dest.Name, opt => opt
                 .MapFrom(src => src.Recipe.Name));



            CreateMap<Recipe, RecipeWithPhotosDto>();



            CreateMap<Recipe, RecipeForDetailDto>()
           .ForMember(dest => dest.PhotoUrl, opt => opt
               .MapFrom(src => src.RecipePhotos
                   .FirstOrDefault(p => p.IsMain).Url))
           .ForMember(dest => dest.Ingredients, opt => opt
          .MapFrom(src => src.Ingredients))
           .ForMember(dest => dest.AuthorId, opt => opt
          .MapFrom(src => src.AppUser.Id))
           .ForMember(dest => dest.CategoryId, opt => opt
          .MapFrom(src => src.Category.Id))
           .ForMember(dest => dest.KitchenOriginId, opt => opt
          .MapFrom(src => src.KitchenOrigin.Id))
          .ForMember(dest => dest.Author, opt => opt
          .MapFrom(src => src.AppUser.KnownAs))
          .ForMember(dest => dest.AuthorPhotoUrl, opt => opt
          .MapFrom(src => src.AppUser.UserPhotos
          .FirstOrDefault(p => p.IsMain).Url))
          .ForMember(dest => dest.AuthorUserName, opt => opt
          .MapFrom(src => src.AppUser.UserName))
          .ForMember(dest => dest.CategoryName, opt => opt
          .MapFrom(src => src.Category.Name))
          .ForMember(dest => dest.KitchenOriginName, opt => opt
          .MapFrom(src => src.KitchenOrigin.Name));


            // Add photo TODO

            CreateMap<Ingredient, IngredientDto>();
            CreateMap<IngredientDto, Ingredient>();
            CreateMap<RecipeForCreateDto, Recipe>();
            CreateMap<RecipeForUpdateDto, Recipe>();
            CreateMap<Category, CategoryDto>();
            CreateMap<KitchenOrigin, KitchenOriginDto>();
            CreateMap<UserLike, LikeDto>();
            CreateMap<UserLike, LikesDto>()
            .ForMember(dest => dest.LikedUserId, opt => opt
          .MapFrom(src => src.LikedUser.Id))
          .ForMember(dest => dest.LikerId, opt => opt
          .MapFrom(src => src.SourceUser.Id));
            CreateMap<RecipePhoto, RecipePhotoForDetailDto>();



            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt =>
                 opt.MapFrom(src => src.Sender.UserPhotos
                .FirstOrDefault(photo => photo.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt =>
                 opt.MapFrom(src => src.Recipient.UserPhotos
                .FirstOrDefault(photo => photo.IsMain).Url));


            CreateMap<Comment, CommentToCreateDto>();
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.KnownAs))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.UserPhotos
                .FirstOrDefault(p => p.IsMain).Url));


        }


    }
}
