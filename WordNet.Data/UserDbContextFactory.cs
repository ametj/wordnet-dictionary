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
    /// <code>dotnet ef migrations add NameOfMigration --context UserDbContext --output-dir Migrations/UserDb</code>
    /// </example>
    /// </summary>
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.UseSqlite(args.Length == 1 ? args[0] : "Data Source=../Data/User.db");

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}