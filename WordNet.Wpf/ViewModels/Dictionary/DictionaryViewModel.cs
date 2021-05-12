using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using WordNet.Data;
using WordNet.Data.Model;

namespace WordNet.Wpf.ViewModels.Dictionary
{
    public class DictionaryViewModel : BindableBase
    {
        public DictionaryViewModel(IWordNetService service)
        {
            Service = service;
        }

        public IWordNetService Service { get; }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _selectedLemma;

        public string SelectedLemma
        {
            get { return _selectedLemma; }
            set { SetProperty(ref _selectedLemma, value); }
        }

        private IEnumerable<LexicalEntry> _lexicalEntries;

        public IEnumerable<LexicalEntry> LexicalEntries
        {
            get { return _lexicalEntries; }
            set { SetProperty(ref _lexicalEntries, value); }
        }

        private IEnumerable<string> _suggestions;

        public IEnumerable<string> Suggestions
        {
            get { return _suggestions; }

            set { SetProperty(ref _suggestions, value); }
        }

        private DelegateCommand<string> _getSuggestionsCommand = null;
        public DelegateCommand<string> GetSuggestionsCommand => _getSuggestionsCommand ??= new DelegateCommand<string>(GetSuggestionsAsync);

        private DelegateCommand<string> _submitCommand = null;
        public DelegateCommand<string> SubmitCommand => _submitCommand ??= new DelegateCommand<string>(Submit);

        public async void GetSuggestionsAsync(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                Suggestions = (await Service.GetLemmaHistory()).Select(le => le.Lemma);
            }
            else
            {
                Suggestions = await Service.GetSuggestionsByLemma(filter);
            }
        }

        private async void Submit(string lemma)
        {
            Text = SelectedLemma = lemma;
            LexicalEntries = await Service.GetByLemma(lemma);
        }
    }
}