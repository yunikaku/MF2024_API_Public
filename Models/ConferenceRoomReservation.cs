namespace MF2024_API.Models
{
    public class ConferenceRoomReservation
    {
        public int ConferenceRoomReservationId { get; set; }

        public string ConferenceRoomReservationRequirement { get; set; }

        public int DeviceId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Device Device { get; set; }

    }
}
