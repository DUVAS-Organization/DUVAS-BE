using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace API.Service
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;

        // Constructor to inject SmtpSettings
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);
        }


        public void SendRentalNotificationToLandlord(
    string landlordEmail,
    string roomTitle,
    string locationDetail,
    decimal price,
    decimal deposit,
    double acreage,
    string furniture,
    int numberOfBathroom,
    int numberOfBedroom,

    string username)
        {
            var subject = "Thông báo thuê phòng";
            var body = $@"
                <p>Chào {landlordEmail},</p>
                <p>Chúng tôi thông báo rằng một người dùng có tên là <strong>{username}</strong> vừa gửi yêu cầu thuê phòng của bạn.</p>
                <p><b>Thông tin phòng:</b></p>
                <ul>
                    <li><b>Tên phòng:</b> {roomTitle}</li>
                    <li><b>Địa chỉ phòng:</b> {locationDetail}</li>
                    <li><b>Giá phòng:</b> {price} VND</li>
                    <li><b>Giá đặt cọc:</b> {deposit} VND</li>
                    <li><b>Diện tích phòng:</b> {acreage} m²</li>
                    <li><b>Nội thất:</b> {furniture}</li>
                    <li><b>Số lượng phòng vệ sinh:</b> {numberOfBathroom}</li>
                    <li><b>Số lượng giường:</b> {numberOfBedroom}</li>
    
                </ul>
                <p>Vui lòng kiểm tra và xử lý yêu cầu của họ.</p>
                <p>Trân trọng,</p>
                <p>DUVAS Team</p>";

            // Gửi email cho chủ phòng
            SendEmail(landlordEmail, subject, body);
        }


    public void SendMonthlyPaymentToUser(string userEmail, string userName, string roomName, string address, decimal price,
                                            decimal deposit, decimal khac, DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            var subject = "Thông báo đóng tiền thuê phòng hằng tháng";
            var body = $@"
            <p>Chúng tôi xin thông báo rằng đã đến thời hạn thanh toán tiền phòng cho tháng này.</p>
            <p><b>Thông tin phòng:</b></p>
            <ul>
                <li><b>Phòng:</b> {roomName}</li>
                <li><b>Địa chỉ:</b> {address}</li>
                <li><b>Giá thuê:</b> {price} VND</li>
                <li><b>Tiền đặt cọc:</b> {deposit} VND</li>
                <li><b>Khoản khác:</b> {khac} VND</li>
                <li><b>Thời gian thuê:</b> {ngayBatDau:dd/MM/yyyy} - {ngayKetThuc:dd/MM/yyyy}</li>
            </ul>
            <p>Vui lòng thanh toán trước ngày {ngayKetThuc:dd/MM/yyyy} để tránh gián đoạn dịch vụ.</p>
            <p>Trân trọng,</p>
            <p>DUVAS Team</p>";
            // Gửi email cho người dùng
            SendEmail(userEmail, subject, body);
        }
        public bool IsEmail(string input)
        {
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(input, emailPattern);
        }
    }
    public class SmtpSettings
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string FromEmail { get; set; }
        public required string FromName { get; set; }
    }
}