using System.Collections.Generic;
using System.Threading.Tasks;
using WordNet.Data.Model;

namespace WordNet.Data
{
    public interface IWordNetService
    {
        Task<ICollection<LexicalEntry>> GetByLemma(string lemma);

        Task<ICollection<string>> GetSuggestionsByLemma(string lemma, int limit = 10);
    }
}