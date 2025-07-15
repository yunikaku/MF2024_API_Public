namespace MF2024_API.Interfaces
{
    public interface DiscordInterfase
    {
        Task<bool> Discordsend(int sectionID, string message);
        Task<bool> DiscordReservationSend(int sectionID,int ReservationID);
    }
}
