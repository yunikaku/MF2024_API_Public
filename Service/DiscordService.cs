using MF2024_API.Interfaces;
using MF2024_API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Text;

namespace MF2024_API.Service
{
    public class DiscordService : DiscordInterfase
    {
        public async Task<bool> DiscordReservationSend(int sectionID, int ReservationID)
        {
            using (var client = new HttpClient())
            {
                using (var context = new Mf2024apiDbContext())
                {
                    try
                    {
                        if (sectionID == null)
                        {
                            return false;
                        }
                        if (ReservationID == null)
                        {
                            return false;
                        }
                        //sectinを取
                        var section = context.Sections.Where(x => x.SectionId == sectionID).FirstOrDefault();
                        if (section == null)
                        {
                            return false;
                        }
                        var url = section.DiscordURL;
                        if (url == null)
                        {
                            return false;
                        }
                        var reservation = context.Reservations.Where(x => x.ReservationId == ReservationID).FirstOrDefault();
                        if (reservation == null)
                        {
                            return false;
                        }
                        string ReservationText;
                        if (reservation.ReservationState == 0)//個人予約
                        {
                            ReservationText =
                                $"受付した方がいます\n" +
                                $"予約ID:{reservation.ReservationId}\n" +
                                $"予約者名:{reservation.ReservationName}\n" +
                                $"要件：{reservation.ReservationRequirement}\n" +
                                $"人数：{reservation.ReservationNumberOfPersons}\n"
                                ;
                        }
                        else if (reservation.ReservationState == 1)//団体予約
                        {
                            ReservationText =
                                $"受付した方がいます\n" +
                                $"予約ID:{reservation.ReservationId}\n" +
                                $"予約者名:{reservation.ReservationName}\n" +
                                $"要件：{reservation.ReservationRequirement}\n" +
                                $"人数：{reservation.ReservationNumberOfPersons}\n" +
                                $"会社名：{reservation.ReservationCompanyName}\n" +
                                $"役職：{reservation.ReservationCompanyPosition}\n"
                                ;
                        }
                        else
                        {
                            return false;
                        }

                        var json = JsonConvert.SerializeObject(new { content = ReservationText });
                        var contet = new StringContent(json, Encoding.UTF8, "application/json");
                        var reselt = await client.PostAsync(url, contet);
                        if (reselt.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);

                    }
                }
            }
        }
        public async Task<bool> Discordsend(int sectionID,string message)
        {
            using (var client =new HttpClient()) 
            {
                using (var context = new Mf2024apiDbContext())
                {

                    try
                    {
                        if (message == null)
                        {
                            return false;
                        }
                        if (sectionID == null)
                        {
                            return false;
                        }
                        var section = context.Sections.Where(x => x.SectionId == sectionID).FirstOrDefault();
                        if (section == null)
                        {
                            return false;
                        }
                        var url = section.DiscordURL;
                        if (url == null)
                        {
                            return false;
                        }
                        var json = JsonConvert.SerializeObject(new { content=message });
                        var contet = new StringContent(json, Encoding.UTF8, "application/json");
                        var reselt = await client.PostAsync(url,contet);
                        if (reselt.IsSuccessStatusCode) 
                        {
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    } 
                }
            }
        }

    }
}
