
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MF2024_API.Models;

public partial class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DepartmentId { get; set; }

    public required string DepartmentName { get; set; }

    public required string DepartmentNameKana { get; set; }

    public string? DiscordURL { get; set; }

    public int DepartmentFlag { get; set; }

    public required string DepartmentAddUserID { get; set; }

    public required int OfficeId { get; set; }

    public DateTime DepartmentAddTime { get; set; }

    public required string DepartmentUpDateUserID { get; set; }

    public DateTime DepartmentUpDateTime { get; set; }

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual User AddUser { get; set; }

    public virtual User UpdateUser { get; set; }

    public virtual ICollection<Office> Offices { get; set; } = new List<Office>();
}
