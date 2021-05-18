using System.Collections.Generic;
using System.Threading.Tasks;
using WordNet.Data.Model;

namespace WordNet.Service
{
    public interface IWordNetDataService
    {
        Task<ICollection<LexicalEntry>> GetByLemma(string lemma, string language);

        Task<ICollection<string>> GetSuggestionsByLemma(string lemma, string language, int limit);

        Task<string> GetRandomLemma(string language, ICollection<string> exclude);
    }
}