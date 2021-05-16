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
        public WordNetService(WordNetDbContext wordNetDbContext, UserDbContext userDbContext)
        {
            WordNetDbContext = wordNetDbContext;
            UserDbContext = userDbContext;
        }

        public WordNetDbContext WordNetDbContext { get; }
        public UserDbContext UserDbContext { get; }

        public async Task<ICollection<LexicalEntry>> GetByLemma(string lemma, string language)
        {
            lemma = lemma?.Trim();
            if (string.IsNullOrEmpty(lemma)) return Array.Empty<LexicalEntry>();

            var query = WordNetDbContext.LexicalEntries.Where(le => le.Lemma == lemma && le.Language == language);

            UpdateLemmaHistory(lemma, language);

            return await query.ToListAsync();
        }

        public async void UpdateLemmaHistory(string lemma, string language)
        {
            var historyEntity = await UserDbContext.FindAsync<LexicalEntryHistory>(lemma, language);

            if (historyEntity != null)
            {
                historyEntity.LastAccessed = DateTime.UtcNow;
            }
            else
            {
                historyEntity = new LexicalEntryHistory { Lemma = lemma, Language = language, LastAccessed = DateTime.UtcNow };
                UserDbContext.Add(historyEntity);
            }

            await UserDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<LexicalEntryHistory>> GetLemmaHistory(string language, int limit)
        {
            var query = UserDbContext.LexicalEntriesHistory.AsNoTracking();

            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(le => le.Language == language);
            }

            query = query.OrderByDescending(le => le.LastAccessed).Take(limit);

            return await query.ToListAsync();
        }

        public async Task<ICollection<string>> GetSuggestionsByLemma(string lemma, string language, int limit)
        {
            lemma = lemma?.Trim().ToLower();
            if (string.IsNullOrEmpty(lemma)) return Array.Empty<string>();

            var query = WordNetDbContext.LexicalEntries
                .Where(le => le.Lemma.ToLower().StartsWith(lemma) && le.Language == language)
                .Select(le => le.Lemma)
                .Distinct()
                .OrderBy(l => l.ToLower())
                .Take(limit);

            return await query.AsNoTracking().ToListAsync();
        }
    }
}