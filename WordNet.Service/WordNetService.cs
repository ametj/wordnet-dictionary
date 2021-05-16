using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WordNet.Data.Model;

namespace WordNet.Service
{
    public class WordNetService : IWordNetService
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public WordNetService(IWordNetDataService wordNetDataService, IUserDataService userDataService)
        {
            WordNetDataService = wordNetDataService;
            UserDataService = userDataService;
        }

        public IWordNetDataService WordNetDataService { get; }
        public IUserDataService UserDataService { get; }

        public Task<ICollection<LexicalEntry>> GetByLemma(string lemma, string language)
        {
            return Run(async () =>
            {
                var result = await WordNetDataService.GetByLemma(lemma, language);

                if (result.Any())
                {
                    await UserDataService.UpdateLemmaHistory(lemma, language);
                }

                return result;
            });
        }

        private async Task<T> Run<T>(Func<Task<T>> func)
        {
            try
            {
                await _semaphore.WaitAsync();

                return await Task.Run(func);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public Task<ICollection<string>> GetSuggestionsByLemma(string lemma, string language, int limit)
        {
            return Run(() => WordNetDataService.GetSuggestionsByLemma(lemma, language, limit));
        }

        public Task<ICollection<LexicalEntryHistory>> GetLemmaHistory(string language, int limit)
        {
            return Run(() => UserDataService.GetLemmaHistory(language, limit));
        }
    }
}