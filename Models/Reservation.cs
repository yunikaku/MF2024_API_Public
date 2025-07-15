using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MF2024_API.Models;

public partial class Reservation
{
    //予約ID
    [Key]
    public int ReservationId { get; set; }
    //予約者名
    public required string ReservationName { get; set; }
    //予約者名(カナ)
    public string? ReservationNameKana { get; set; } 
    //予約人数
    public required int ReservationNumberOfPersons { get; set; }
    //要件
    public required string ReservationRequirement { get; set; } 


    //会社名
    public string? ReservationCompanyName { get; set; }
    //会社名(カナ)
    public string? ReservationCompanyNameKana { get; set; }
    //役職
    public string? ReservationCompanyPosition { get; set; }

    //予約日時
    public DateTime ReservationDate { get; set; }
    //sectionID
    public int SectionId { get; set; }

    //メールアドレス
    [EmailAddress]
    public string? ReservationEmail { get; set; }
    //電話番号
    [Phone]
    public string? ReservationPhoneNumber { get; set; }
    //ステータス　個人か法人か　0:個人　1:法人 
    public int ReservationState { get; set; }
    //受付済みか 0:未受付　1:受付済み 2:受付キャンセル
    public int ReservationReception { get; set; }
    //予約したか 0:web予約　1:受付予約作成
    public int ReservationType { get; set; }
    //QRコード
    public required byte[] ReservationQrcode { get; set; }
    //予約コード
    public required string ReservationCode { get; set; }
    //token web予約用
    public string? Token { get; set; }

    //追加ユーザーID
    public required string ReservationAddUserID { get; set; }
    //追加日時
    public required DateTime ReservationAddTime { get; set; }
    //更新ユーザーID
    public required string ReservationUpdateUserID { get; set; }
    //更新日時
    public required DateTime ReservationUpDateTime { get; set; }


    public virtual ICollection<Nfcallotment> Nfcallotments { get; set; } = new List<Nfcallotment>();

    public virtual User AddUser { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;

    public virtual Section Section { get; set; } = null!;
}
