namespace ChatApp.Application.DTOs
{
    public class JoinRoomDto
    {
        public Guid UserId { get; set; }
        public Guid ChatRoomId { get; set; }
    }
}