namespace Uppgift_14.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty; // = "";
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } }
        public string Description { get; set; } = "";

        // navigation property, using relational class ApplicationUserGymClass
        public List<ApplicationUserGymClass> AttendingMembers { get; set; } = new List<ApplicationUserGymClass>(); // viktigt!

    }
}
