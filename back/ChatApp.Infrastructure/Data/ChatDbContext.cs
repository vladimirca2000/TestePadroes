using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Data
{
    public static class ChatDbContextSeed
    {
        public static void EnsureSeeded(this ChatDbContext context)
        {
            var defaultRoomId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            if (!context.ChatRooms.Any(r => r.Id == defaultRoomId))
            {
                context.ChatRooms.Add(new ChatRoom
                {
                    Id = defaultRoomId,
                    Name = "Geral",
                    Description = "Sala padrão do sistema",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.Empty
                });
                context.SaveChanges();
            }
        }
    }

    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // ChatRoom Configuration
            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Seed: Sala padrão
            var defaultRoomId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            modelBuilder.Entity<ChatRoom>().HasData(new ChatRoom
            {
                Id = defaultRoomId,
                Name = "Geral",
                Description = "Sala padrão do sistema",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = Guid.Empty
            });

            // Message Configuration
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                entity.HasOne(e => e.Sender)
                    .WithMany(u => u.SentMessages)
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.ChatRoom)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(e => e.ChatRoomId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Many-to-Many: User <-> ChatRoom
            modelBuilder.Entity<User>()
                .HasMany(u => u.ChatRooms)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserChatRooms"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
