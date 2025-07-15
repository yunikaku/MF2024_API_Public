using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MF2024_API.Models;

public partial class NoReservation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NoReservationId { get; set; }
    //名前
    public required string NoReservationName { get; set; }
    //名前カナ
    public string ? NoReservationNameKana { get; set; }
    //人数
    public int NoReservationNumberOfPrsesons { get; set; }
    //要件
    public required string NoReservationRequirement { get; set; }
    //ステータス
    public int NoReservationState { get; set; }
    [EmailAddress]
    //メールアドレス
    public required string NoReservationEmail { get; set; }
    //電話番号
    public required string NoReservationPhoneNumber { get; set; }
    //会社名
    public string? NoReservationCompanyName { get; set; }
    //会社名カナ
    public string? NoReservationCompanyNameKana { get; set; }
    //役職
    public string? NoReservationCompanyPosition { get; set; } 
    //受付時間
    public DateTime NoReservationDate { get; set; }

    public required string NoReservationAddUserID { get; set; }

    public DateTime NoReservationAddTime { get; set; }

    public required string NoReservationUpdateUserID { get; set; }

    public DateTime NoReservationUpDateTime { get; set; }

    public virtual ICollection<Nfcallotment> Nfcallotments { get; set; } = new List<Nfcallotment>();
    public virtual User AddUser { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;
}
