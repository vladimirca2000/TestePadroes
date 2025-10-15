using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ChatApp.Core.Entities;

namespace ChatApp.Infrastructure.Data
{
    public class ChatDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
    {
        public ChatDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();
            // ATENÇÃO: Ajuste a connection string conforme seu ambiente
            optionsBuilder.UseMySql(
                "server=192.168.2.168;database=ChatAppDB;user=root;password=secret;",
                new MySqlServerVersion(new Version(10, 5, 0))
            );
            return new ChatDbContext(optionsBuilder.Options);
        }
    }
}
