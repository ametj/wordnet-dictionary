using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordNet.Data.Model;

namespace WordNet.Service
{
    public class WordNetService : IWordNetService
    {
        public WordNetService(IWordNetDataService wordNetDataService, IUserDataService userDataService)
        {
            WordNetDataService = wordNetDataService;
            UserDataService = userDataService;
        }

        public IWordNetDataService WordNetDataService { get; }
        public IUserDataService UserDataService { get; }

        public async Task<ICollection<LexicalEntry>> GetByLemma(string lemma, string language)
        {
            var result = await WordNetDataService.GetByLemma(lemma, language);

            if (result.Any())
            {
                await UserDataService.UpdateLemmaHistory(lemma, language);
            }

            return result;
        }

        public Task<ICollection<string>> GetSuggestionsByLemma(string lemma, string language, int limit)
        {
            return WordNetDataService.GetSuggestionsByLemma(lemma, language, limit);
        }

        public Task<ICollection<LexicalEntryHistory>> GetLemmaHistory(string language, int limit)
        {
            return UserDataService.GetLemmaHistory(language, limit);
        }
    }
}