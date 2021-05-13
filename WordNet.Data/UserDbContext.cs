using Microsoft.EntityFrameworkCore;
using WordNet.Data.Model;

namespace WordNet.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<LexicalEntryHistory> LexicalEntriesHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<LexicalEntryHistory>()
                .HasKey(leh => new { leh.Lemma, leh.Language });
        }
    }
}