using System;
using System.Collections.Generic;
using API.Dto.Recipe;

namespace API.Dto.User
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PhotoUrl { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<UserPhotoDto> UserPhotos { get; set; }
        public ICollection<RecipeForListDto> Recipes { get; set; }
        public ICollection<LikesDto> LikedByUsers { get; set; } // who has liked currently logged user
        public ICollection<LikesDto> LikedUsers { get; set; }

    }
}