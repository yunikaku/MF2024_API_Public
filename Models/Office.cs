using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MF2024_API.Models;

public partial class Office
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OfficeId { get; set; }

    public required string OfficeName { get; set; }

    public required string OfficeNameKana { get; set; }

    public required string OfficeLocation { get; set; }

    public required int OfficeFlag { get; set; }

    public required string OfficeAddUserID { get; set; }

    public DateTime OfficeAddTime { get; set; }

    public required string OfficeUpDateUserID { get; set; }

    public DateTime OfficeUpDateTime { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual User AddUser { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

}
