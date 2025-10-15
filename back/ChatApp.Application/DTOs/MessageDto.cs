namespace ChatApp.Application.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public Guid ChatRoomId { get; set; }
        public bool IsRead { get; set; }
    }

    public class SendMessageDto
    {
        public string Content { get; set; } = string.Empty;
        public Guid ChatRoomId { get; set; }
        public Guid SenderId { get; set; }
    }
}
