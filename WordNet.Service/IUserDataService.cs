using System.Collections.Generic;
using System.Threading.Tasks;
using WordNet.Data.Model;

namespace WordNet.Service
{
    public interface IUserDataService
    {
        Task<ICollection<LexicalEntryHistory>> GetLemmaHistory(string language, int limit);

        Task UpdateLemmaHistory(string lemma, string language);
    }
}