namespace ChatApp.Core.Entities;

public class Message
{
    public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public Guid SenderId { get; set; }
        public Guid ChatRoomId { get; set; }
        public bool IsRead { get; set; }
        
        public virtual User Sender { get; set; } = null!;
        public virtual ChatRoom ChatRoom { get; set; } = null!;
}
