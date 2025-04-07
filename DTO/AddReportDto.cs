namespace DTO;

public class AddReportDto
{
    public int RoomId { get; set; }
    public required string ReportContent { get; set; }
    public required string Image { get; set; }
}