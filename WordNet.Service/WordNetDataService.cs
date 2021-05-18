using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordNet.Data;
using WordNet.Data.Model;

namespace WordNet.Service
{
    public class WordNetDataService : IWordNetDataService
    {
        private readonly Random _random = new();

        public WordNetDataService(WordNetDbContext wordNetDbContext)
        {
            WordNetDbContext = wordNetDbContext;
        }

        public WordNetDbContext WordNetDbContext { get; }

        public async Task<ICollection<LexicalEntry>> GetByLemma(string lemma, string language)
        {
            lemma = lemma?.Trim();
            if (string.IsNullOrEmpty(lemma)) return Array.Empty<LexicalEntry>();

            var query = WordNetDbContext.LexicalEntries
                .Where(le => le.Lemma == lemma && le.Language == language);

            return await query.ToListAsync();
        }

        public async Task<ICollection<string>> GetSuggestionsByLemma(string lemma, string language, int limit)
        {
            lemma = lemma?.Trim().ToLower();
            if (string.IsNullOrEmpty(lemma)) return Array.Empty<string>();

            var query = WordNetDbContext.LexicalEntries
                .AsNoTracking()
                .Where(le => le.Lemma.ToLower().StartsWith(lemma) && le.Language == language)
                .Select(le => le.Lemma)
                .Distinct()
                .OrderBy(l => l.ToLower())
                .Take(limit);

            return await query.ToListAsync();
        }

        public async Task<string> GetRandomLemma(string language, ICollection<string> exclude)
        {
            var query = WordNetDbContext.LexicalEntries
                .AsNoTracking()
                .Where(le => le.Language == language)
                .Select(le => le.Lemma)
                .Distinct();

            if (exclude != null && exclude.Any())
            {
                query = query.Where(le => !exclude.Contains(le));
            }

            var skip = _random.Next(query.Count());
            query = query.Skip(skip).Take(1);

            return await query.FirstOrDefaultAsync();
        }

    }
}