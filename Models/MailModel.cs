namespace MF2024_API.Models
{
    public class MailModel
    {
        public required string Subject { get; set; }

    }

    public class MailModelTemplateReservstion : MailModel
    {
        public required int  ReservationID { get; set; }
    }

}
