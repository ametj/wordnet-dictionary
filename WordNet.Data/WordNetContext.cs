using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using WordNet.Model;

namespace WordNet.Data
{
    public class WordNetContext : DbContext
    {
        private readonly string _connectionString;

        public WordNetContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public DbSet<LexicalEntry> LexicalEntries { get; set; }
        public DbSet<Sense> Senses { get; set; }
        public DbSet<SenseRelation> SenseRelations { get; set; }
        public DbSet<Synset> Synsets { get; set; }
        public DbSet<SynsetRelation> SynsetRelations { get; set; }
        public DbSet<SyntacticBehaviour> SyntacticBehaviours { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Expression<Func<IList<string>, string>> serialize = f => JsonSerializer.Serialize(f, null);
            Expression<Func<string, IList<string>>> deserialize = f => JsonSerializer.Deserialize<List<string>>(f, null);
            var collectionComparer = new ValueComparer<IEnumerable<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            modelBuilder
                .Entity<LexicalEntry>()
                .Property(e => e.Forms)
                .HasConversion(serialize, deserialize, collectionComparer);

            modelBuilder
                .Entity<Synset>()
                .Property(e => e.Definitions)
                .HasConversion(serialize, deserialize, collectionComparer);
            modelBuilder
                .Entity<Synset>()
                .Property(e => e.Examples)
                .HasConversion(serialize, deserialize, collectionComparer);
        }
    }
}
