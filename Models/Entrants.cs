using System.Text.Json.Serialization;

namespace MF2024_API.Models
{
    public class Entrants
    {
        public int EntrantsID { get; set; }
        public int DeviceID { get; set; }

        public virtual Device Device { get; set; }
        public virtual ICollection<Nfcallotment>? Nfcallotments { get; set; } 
    }
}
