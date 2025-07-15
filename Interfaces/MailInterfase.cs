using MF2024_API.Models;

namespace MF2024_API.Interfaces
{
    public interface MailInterfase
    {
        Task<bool> SendMailReservation(MailModelTemplateReservstion mailModelTemplateReservstion);


    }
}
