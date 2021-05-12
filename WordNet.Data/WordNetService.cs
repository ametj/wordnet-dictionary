using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordNet.Data.Model;

namespace WordNet.Data
{
    public class WordNetService : IWordNetService
    {
        private readonly int _partOfSpeechCount = Enum.GetNames(typeof(PartOfSpeech)).Length;

        public WordNetService(WordNetDbContext context)
        {
            Context = context;
        }

        public WordNetDbContext Context { get; }

        public async Task<ICollection<LexicalEntry>> GetByLemma(string lemma, string language)
        {
            var query = Context.LexicalEntries.Where(le => le.Lemma == lemma && le.Language == language);

            await query.ForEachAsync(le => le.LastAccessed = DateTime.UtcNow);
            await Context.SaveChangesAsync();

            return await query.ToListAsync();
        }

        public async Task<ICollection<LexicalEntry>> GetLemmaHistory(string language, int limit)
        {
            var query = Context.LexicalEntries.Where(le => le.LastAccessed != null);

            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(le => le.Language == language);
            }

            var result = await query.AsNoTracking()
                .OrderByDescending(le => le.LastAccessed)
                .Take(limit * _partOfSpeechCount)
                .ToListAsync();

            // In Db grouping is too slow.
            result = result.GroupBy(le => new { le.Lemma, le.Language })
                .Select(g => new LexicalEntry { Lemma = g.Key.Lemma, Language = g.Key.Language, LastAccessed = g.Max(le => le.LastAccessed) })
                .OrderByDescending(le => le.LastAccessed)
                .Take(limit).ToList();

            return result;
        }

        public async Task<ICollection<string>> GetSuggestionsByLemma(string lemma, string language, int limit)
        {
            lemma = lemma?.Trim().ToLower();
            if (string.IsNullOrEmpty(lemma)) return Array.Empty<string>();

            var query = Context.LexicalEntries
                .Where(le => le.Lemma.ToLower().StartsWith(lemma) && le.Language == language)
                .Select(le => le.Lemma)
                .Distinct()
                .OrderBy(l => l.ToLower())
                .Take(limit);

            return await query.AsNoTracking().ToListAsync();
        }
    }
}