using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MF2024_API.Models;

public partial class Device
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeviceId { get; set; }

    public required string DeviceName { get; set; }
    //建物内の位置
    public required string DeviceLocation { get; set; }
    //会議室＝1、オフィス＝2、publicスペース＝3
    public required int DeviceCategory { get; set; }

    public required int DeviceFlag { get; set; }

    public required string DeviceUserID { get; set; }

    public required string DeviceAddUserID { get; set; }

    public DateTime DeviceAddTime { get; set; }

    public required string DeviceUpdateUserID { get; set; }

    public DateTime DeviceUpDateTime { get; set; }

    public required int RoomId { get; set; }



    public virtual ICollection<OptOut> OptOuts { get; set; } = new List<OptOut>();

    //public virtual ICollection<Entrants> Entrants { get; set; } = new List<Entrants>();

    public virtual Entrants Entrants { get; set; } = null!;

    public virtual ICollection<ConferenceRoomReservation> ConferenceRoomReservations { get; set; } = new List<ConferenceRoomReservation>();

    public virtual Room Room { get; set; } = null!;

    public virtual User DeviceUser { get; set; } = null!;

    public virtual User AddUser { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;
}
