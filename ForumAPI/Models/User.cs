using Microsoft.AspNetCore.Identity;

namespace ForumAPI.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       

        //first name
        //name
        //adresse mail
        //login
    }
}
