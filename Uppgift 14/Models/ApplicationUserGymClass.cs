namespace Uppgift_14.Models
{
    public class ApplicationUserGymClass
    {
        // using a composite key as id with fluent API (see db context class), leaving out defining it here 

        // foreign keys
        public int ApplicationUserId { get; set; }
        public int GymClassId { get; set; }
    }
}
