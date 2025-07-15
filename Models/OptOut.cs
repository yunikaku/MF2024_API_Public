using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MF2024_API.Models;

public partial class OptOut
{
    [Key]
    public int OptOutId { get; set; }

    public int DeviceId { get; set; }

    public int NfcallotmentId { get; set; }

    public int OptOutState { get; set; }

    public DateTime OptOutTime { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual Nfcallotment Nfcallotment { get; set; } = null!;
}
