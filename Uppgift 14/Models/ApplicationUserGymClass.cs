namespace Uppgift_14.Models
{
    public class ApplicationUserGymClass
    {
        // using a composite key as id with fluent API (see db context class), leaving out defining it here 

        // foreign keys
        public string ApplicationUserId { get; set; }
        public int GymClassId { get; set; }

        // navigation properties to simplify queries with LINQ
        public ApplicationUser ApplicationUser { get; set; }
        public GymClass GymClass { get; set; }
    }
}
