using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WordNet.Data
{
    /// <summary>
    /// Used for migrations by Entity Framework Core CLI
    /// <see href="https://docs.microsoft.com/en-us/ef/core/cli/dotnet"/>
    /// <para/>
    /// 
    /// <example>Create database migration with:
    /// <code>dotnet ef migrations add NameOfMigration --context WordNetDbContext --output-dir Migrations/WordNetDb</code>
    /// </example>
    /// 
    /// <example>Update database with:
    /// <code>dotnet ef database update --context WordNetDbContext</code>
    /// </example>
    /// 
    /// <example> You can specify connection string by appending to above mentioned commands:
    /// <code>-- "connection string"</code>
    /// </example>
    /// </summary>
    public class WordNetDbContextFactory : IDesignTimeDbContextFactory<WordNetDbContext>
    {
        public WordNetDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WordNetDbContext>();
            optionsBuilder.UseSqlite(args.Length == 1 ? args[0] : "Data Source=../Data/WordNet.db");

            return new WordNetDbContext(optionsBuilder.Options);
        }
    }
}