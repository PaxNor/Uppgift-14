using Microsoft.AspNetCore.Identity;

namespace Uppgift_14.Models
{
    public class ApplicationUser : IdentityUser
    {
        // navigation property, using relational class ApplicationUserGymClass
        public List<ApplicationUserGymClass> AttendedClasses { get; set; } = new List<ApplicationUserGymClass>(); // viktigt!
    }
}
