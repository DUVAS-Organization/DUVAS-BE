using Microsoft.AspNetCore.SignalR;
using DUVAS;  // Giả sử Message được định nghĩa ở đây hoặc dùng namespace DTO nếu cần

public class ChatHub : Hub
{
    // Phương thức nhận tin nhắn từ client và phát đến tất cả client
    public async Task SendMessage(Message message)
    {
        // Bạn có thể thực hiện lưu dữ liệu, xử lý hoặc kiểm tra dữ liệu tại đây nếu cần
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}