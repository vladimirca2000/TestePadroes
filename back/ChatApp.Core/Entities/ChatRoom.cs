namespace ChatApp.Core.Entities;

public class ChatRoom
{
    public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
}
