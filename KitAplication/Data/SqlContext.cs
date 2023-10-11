using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace KitAplication.Data
{
    public class SqlContext :DbContext
    {
        public SqlContext(DbContextOptions options) : base(options)
        {
           
        }

        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<SystemEntity> Systems { get; set; }
        public DbSet<LinkEntity> Links { get; set; }
        public DbSet<ChatSettings> ChatSettings { get; set; }
    }

}
