using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordNet.Data;
using WordNet.Data.Model;

namespace WordNet.Service
{
    public class UserDataService : IUserDataService
    {
        public UserDataService(UserDbContext userDbContext)
        {
            UserDbContext = userDbContext;
        }

        public UserDbContext UserDbContext { get; }

        public async Task<ICollection<LexicalEntryHistory>> GetLemmaHistory(string language, int limit, DateTime? from, DateTime? to)
        {
            var query = UserDbContext.LexicalEntriesHistory.AsNoTracking();

            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(le => le.Language == language);
            }

            if (from != null)
            {
                query = query.Where(le => le.LastAccessed >= from);
            }

            if (to != null)
            {
                query = query.Where(le => le.LastAccessed <= to);
            }

            query = query.OrderByDescending(le => le.LastAccessed).Take(limit);

            return await query.ToListAsync();
        }

        public async Task UpdateLemmaHistory(string lemma, string language)
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
    }
}