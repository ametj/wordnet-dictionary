using Microsoft.EntityFrameworkCore;
using Mono.Options;
using System;
using System.Linq;
using System.Reflection;
using WordNet.Data;
using WordNet.Import.Parsers;

namespace WordNet.Import
{
    internal class Program
    {
        private static string Name => Assembly.GetExecutingAssembly().GetName().Name;

        private static void Main(string[] args)
        {
            bool shouldShowHelp = false;
            bool loadRelations = true;
            string wordNetFile = string.Empty;
            string connectionString = string.Empty;

            var options = new OptionSet
            {
                { "f|file=", "WordNet file to process,", f => wordNetFile = f },
                { "c|connection=", "connection string,", cs => connectionString = cs },
                { "nr|no-relations", "relations will not be imported,", _ => loadRelations = false },
                { "h|help", "show this message and exit.", h => shouldShowHelp = h != null },
            };

            try
            {
                var extra = options.Parse(args);

                if (shouldShowHelp || extra.Any())
                {
                    ShowHelp(options);
                    return;
                }

                var message = string.Empty;
                if (string.IsNullOrEmpty(wordNetFile))
                    message += "Missing required option '-f'. ";

                if (string.IsNullOrEmpty(connectionString))
                    message += "Missing required option '-c'";

                if (!string.IsNullOrEmpty(message))
                    throw new InvalidOperationException(message);

                var parser = new XmlWordNetParser();
                var result = parser.Parse(wordNetFile, loadRelations);

                var optionsBuilder = new DbContextOptionsBuilder<WordNetDbContext>();
                optionsBuilder.UseSqlite(connectionString);

                using var db = new WordNetDbContext(optionsBuilder.Options);
                Console.WriteLine("Saving into db. This will take a while.");

                db.AddRange(result.LexicalEntries);

                if (loadRelations)
                {
                    db.AddRange(result.SenseRelations);
                    db.AddRange(result.SynsetRelations);
                }

                var entries = db.SaveChanges();
                Console.WriteLine($"Number of entries written to db: {entries}.");
            }
            catch (Exception e)
            {
                Console.Write($"{Name}: ");
                Console.WriteLine(e.Message);
                Console.WriteLine($"\nTry `{Name} --help' for more information.");
                return;
            }
        }

        private static void ShowHelp(OptionSet options)
        {
            Console.WriteLine("Imports WordNet file into a database.\n");
            Console.WriteLine($"USAGE:\n  {Name} [OPTIONS]+\n");
            Console.WriteLine("OPTIONS:");
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}