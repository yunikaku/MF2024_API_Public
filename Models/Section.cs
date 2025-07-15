using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MF2024_API.Models;

public partial class Section
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SectionId { get; set; }

    public required string SectionName { get; set; }

    public required string SectionNameKana { get; set; }
    public string? DiscordURL { get; set; }

    public int SectionFlag { get; set; }

    public int DepartmentId { get; set; }

    public required string SectionAddUserID { get; set; }

    public DateTime SectionAddTime { get; set; }

    public required string SectionUpDateUserID { get; set; }

    public DateTime SectionUpDateTime { get; set; }


    public virtual Department Department { get; set; } = null!;

    public virtual User AddUser { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
