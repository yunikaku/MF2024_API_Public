namespace MF2024_API.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        public required string EquipmentName { get; set; }
        public byte[]? EquipmentData { get; set; }
        public int EquipmentFlag { get; set; }
    }
}
