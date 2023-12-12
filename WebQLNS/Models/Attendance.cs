namespace WebQLNS.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
}
