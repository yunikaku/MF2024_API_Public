using Microsoft.AspNetCore.Identity;

namespace MF2024_API.Models
{
    public class User : IdentityUser
    {
        public DateTime UserPasswoedUpdataTime { get; set; }
        //パスワード更新日時
        public DateTime UserDateOfBirth { get; set; }
        //生年月日
        public string UserGender { get; set; }
        //性別
        public string UserAddress { get; set; }
        //住所
        public DateTime UserDateOfEntry { get; set; }
        //入社日
        public DateTime ? UserDateOfRetirement { get; set; }
        //退職日
        public DateTime UserUpdataDate { get; set; }
        //更新日時
        public string? UserUpdataUser { get; set; }
        //更新者
        public string? RefreshToken { get; set; }
        //リフレッシュトークン(デバイス用)
        public virtual ICollection<Department> DepartmentAdd { get; set; } = new List<Department>();

        public virtual ICollection<Department> DepartmentUpDate { get; set; } = new List<Department>();

        public virtual ICollection<Device> Device { get; set; } = new List<Device>();

        public virtual ICollection<Device> DeviceAdd { get; set; } = new List<Device>();

        public virtual ICollection<Device> DeviceUpdate { get; set; } = new List<Device>();

        public virtual ICollection<Nfc> NfcAdd { get; set; } = new List<Nfc>();

        public virtual ICollection<Nfc> NfcUpdate { get; set; } = new List<Nfc>();

        public virtual ICollection<Nfcallotment> Nfcallotments { get; set; } = new List<Nfcallotment>();

        public virtual ICollection<Nfcallotment> NfcallotmentAdd { get; set; } = new List<Nfcallotment>();

        public virtual ICollection<Nfcallotment> NfcallotmentUpDate { get; set; } = new List<Nfcallotment>();

        public virtual ICollection<Office> OfficeAdd { get; set; } = new List<Office>();

        public virtual ICollection<Office> OfficeUpdate { get; set; } = new List<Office>();

        public virtual ICollection<Room> RoomAdd { get; set; } = new List<Room>();

        public virtual ICollection<Room> RoomUpdate { get; set; } = new List<Room>();

        public virtual ICollection<Section> SectionAdd { get; set; } = new List<Section>();

        public virtual ICollection<Section> SectionUpdate { get; set; } = new List<Section>();

        public virtual ICollection<NoReservation> NoReservationAdd { get; set; } = new List<NoReservation>();

        public virtual ICollection<NoReservation> NoReservationUpdate { get; set; } = new List<NoReservation>();

        public virtual ICollection<Reservation> ReservationAdd { get; set; } = new List<Reservation>();

        public virtual ICollection<Reservation> ReservationUpdate { get; set; } = new List<Reservation>();

    }
}
