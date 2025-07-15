using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MF2024_API.Models;

public partial class Nfcallotment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NfcallotmentId { get; set; }

    public DateTime AllotmentTime { get; set; }

    public required int State { get; set; }

    public required int NfcId { get; set; }

    public int? ReservationId { get; set; }

    public int? NoReservationId { get; set; }

    public string? UserId { get; set; }

    public required string AddUserId { get; set; }

    public required DateTime AddTime { get; set; }

    public required string UpdateUserId { get; set; }

    public required DateTime UpdateTime { get; set; }


    public virtual Nfc Nfc { get; set; } = null!;

    public virtual NoReservation? NoReservation { get; set; }

    public virtual ICollection<OptOut> OptOuts { get; set; } = new List<OptOut>();

    public virtual Reservation? Reservation { get; set; }
    public virtual User? User { get; set; }
    public virtual User AddUser { get; set; }
    public virtual User UpdateUser { get; set; }
    [JsonIgnore]
    public virtual ICollection<Entrants> Entrants { get; set; } = new List<Entrants>();
}
