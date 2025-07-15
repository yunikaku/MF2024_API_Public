using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MF2024_API.Models;

public partial class Room
{
    //部屋ID
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoomId { get; set; }
    //部屋名
    public required string RoomName { get; set; }
    //収容人数
    public int RoomCapacity { get; set; }
    //オフィスID
    public int OfficeId { get; set; }
    //部屋状態 0:使用可能 1:使用不可（削除）
    public int RoomState { get; set; }
    //課ID
    public int? SectionId { get; set; }

    public required string RoomAddUserID { get; set; }

    public DateTime RommAddTime { get; set; }

    public required string RoompDateUserID { get; set; }

    public DateTime RoomUpDateTime { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
    public virtual ICollection<RoomImage> Images { get; set; }= new List<RoomImage>();

    //public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual Office Office { get; set; } = null!;

    public virtual Section? Section { get; set; }

    public virtual User AddUser { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;
}
