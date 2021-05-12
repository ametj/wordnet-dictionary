using System.Collections.Generic;
using System.Threading.Tasks;
using WordNet.Data.Model;

namespace WordNet.Data
{
    public interface IWordNetService
    {
        Task<ICollection<LexicalEntry>> GetByLemma(string lemma, string language = "en");

        Task<ICollection<LexicalEntry>> GetLemmaHistory(string language = "en", int limit = 50);

        Task<ICollection<string>> GetSuggestionsByLemma(string lemma, string language = "en", int limit = 50);
    }
}