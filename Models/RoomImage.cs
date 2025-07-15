namespace MF2024_API.Models
{
    public class RoomImage
    {
        public  int RoomImageID { get; set; }
        public required string RoomImageName { get; set; }
        public required byte[] RoomImageData { get; set; }
        public int roomID { get; set; }
        public Room room { get; set; }
    }
}
