using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MF2024_API.Models;

public partial class Nfc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NfcId { get; set; }

    public required int NfcState { get; set; }

    public required string NfcUid { get; set; }

    public DateTime NfcAddTime { get; set; }

    public required string NfcAddUserID { get; set; }

    public DateTime NfcUpdateTime { get; set; }

    public required string NfcUpdateUserID { get; set; }

    public virtual ICollection<Nfcallotment> Nfcallotments { get; set; } = new List<Nfcallotment>();

    public virtual User AddUser { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;
}
