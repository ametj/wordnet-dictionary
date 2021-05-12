using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using WordNet.Data.Model;

namespace WordNet.Data
{
    public class WordNetDbContext : DbContext
    {
        public WordNetDbContext(DbContextOptions<WordNetDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<LexicalEntry> LexicalEntries { get; set; }
        public DbSet<Sense> Senses { get; set; }
        public DbSet<SenseRelation> SenseRelations { get; set; }
        public DbSet<Synset> Synsets { get; set; }
        public DbSet<SynsetRelation> SynsetRelations { get; set; }
        public DbSet<SyntacticBehaviour> SyntacticBehaviours { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLazyLoadingProxies();
        }

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
                .Property(le => le.Forms)
                .HasConversion(serialize, deserialize, collectionComparer);
            modelBuilder
                .Entity<LexicalEntry>()
                .HasIndex(le => le.Lemma);

            modelBuilder
                .Entity<Sense>()
                .HasMany(s => s.Relations)
                .WithOne(s => s.Source);

            modelBuilder
                .Entity<Synset>()
                .Property(s => s.Definitions)
                .HasConversion(serialize, deserialize, collectionComparer);
            modelBuilder
                .Entity<Synset>()
                .Property(s => s.Examples)
                .HasConversion(serialize, deserialize, collectionComparer);
            modelBuilder
                .Entity<Synset>()
                .HasMany(s => s.Relations)
                .WithOne(s => s.Source);
        }
    }
}