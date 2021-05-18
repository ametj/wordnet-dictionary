using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordNet.Data.Model;

namespace WordNet.Service
{
    public interface IUserDataService
    {
        Task<ICollection<LexicalEntryHistory>> GetLemmaHistory(string language, int limit = -1, DateTime? from = null, DateTime? to = null);

        Task UpdateLemmaHistory(string lemma, string language);
    }
}