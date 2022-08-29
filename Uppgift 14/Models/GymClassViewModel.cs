namespace Uppgift_14.Models
{
    public class GymClassViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty; // = "";
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; } = "";
        public bool Attending { get; set; }

        public List<string> AttendingMemberNames { get; set; } = new List<string>();
    }
}
