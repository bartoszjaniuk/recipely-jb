using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser: IdentityUser<int>
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Introduction { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<UserPhoto> UserPhotos {get; set;}
        public ICollection<Recipe> Recipes {get; set;}
        public ICollection<UserLike> LikedByUsers {get; set;} // who has liked currently logged user
        public ICollection<UserLike> LikedUsers {get; set;} 
        public ICollection<Message> MessagesSent {get; set;} 
        public ICollection<Message> MessageReceived {get; set;} 
        public ICollection<AppUserRole> UserRoles {get; set;}
        
    }
}