﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordNet.Model;

namespace WordNet.Data
{
    public class WordNetService : IWordNetService
    {
        public WordNetService(WordNetDbContext context)
        {
            Context = context;
        }

        public WordNetDbContext Context { get; }

        public async Task<ICollection<LexicalEntry>> GetByLemma(string lemma)
        {
            var query = Context.LexicalEntries.Where(le => le.Lemma == lemma);
            return await query.ToListAsync();
        }

        public async Task<ICollection<string>> GetSuggestionsByLemma(string lemma, int limit)
        {
            var query = Context.LexicalEntries
                .Where(le => le.Lemma.ToLower().StartsWith(lemma))
                .Select(le => le.Lemma)
                .Distinct()
                .OrderBy(l => l.ToLower())
                .Take(limit);

            return await query.AsNoTracking().ToListAsync();
        }
    }
}