using System;

namespace DTO
{
    public class ConversationDTO
    {
        /// <summary>
        /// Id của đối phương (nếu currentUser là người gửi, thì đối phương là người nhận, ngược lại)
        /// </summary>
        public int UserGetID { get; set; }

        /// <summary>
        /// Nội dung tin nhắn mới nhất trong cuộc trò chuyện
        /// </summary>
        public string LatestMessageContent { get; set; }

        /// <summary>
        /// Thời gian gửi tin nhắn mới nhất
        /// </summary>
        public DateTime LatestMessageDateTime { get; set; }
    }
}