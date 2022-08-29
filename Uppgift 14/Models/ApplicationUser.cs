using Microsoft.AspNetCore.Identity;

namespace Uppgift_14.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";
        public DateTime TimeOfRegistration { get; private set; }

        // navigation property, using relational class ApplicationUserGymClass
        public List<ApplicationUserGymClass> AttendedClasses { get; set; } = new List<ApplicationUserGymClass>(); // viktigt!

        public ApplicationUser() {
            TimeOfRegistration = DateTime.Now;
        }
    }
}
