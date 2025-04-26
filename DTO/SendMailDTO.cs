public class SendMailDTO
{
    public int UserIdLandlord { get; set; }
    public int RoomId { get; set; }
    public string RenterName { get; set; }

    // Thông tin chi tiết về phòng để gửi email
    public string RoomTitle { get; set; }
    public string LocationDetail { get; set; }
    public decimal Price { get; set; }
    public decimal Deposit { get; set; }
    public double Acreage { get; set; }
    public string Furniture { get; set; }
    public int NumberOfBathroom { get; set; }
    public int NumberOfBedroom { get; set; }
}
