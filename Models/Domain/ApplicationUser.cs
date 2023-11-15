using Microsoft.AspNetCore.Identity;

namespace MovieStore.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public String Name { get; set; }

        //public String? ProfilePicture { get; set; }
    }
}
