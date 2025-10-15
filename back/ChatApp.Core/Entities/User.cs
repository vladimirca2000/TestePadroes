namespace ChatApp.Core.Entities;

public class User
{
     public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSeen { get; set; }
        
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
}
