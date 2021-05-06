using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using WordNet.Data;
using WordNet.Data.Model;

namespace WordNet.Wpf.ViewModels.Dictionary
{
    public class DictionaryViewModel : BindableBase
    {
        public IWordNetService Service { get; }

        private string _text;
        public string Text
        {
            get => _text;
            set { SetProperty(ref _text, value); }
        }

        private string _selectedLemma;
        public string SelectedLemma
        {
            get => _selectedLemma;
            set { SetProperty(ref _selectedLemma, value); }
        }

        private IEnumerable<LexicalEntry> _lexicalEntries;
        public IEnumerable<LexicalEntry> LexicalEntries
        {
            get => _lexicalEntries;
            set { SetProperty(ref _lexicalEntries, value); }
        }

        private IEnumerable<string> _suggestions;
        public IEnumerable<string> Suggestions
        {
            get => _suggestions;
            set { SetProperty(ref _suggestions, value); }
        }

        public DictionaryViewModel(IWordNetService service)
        {
            Service = service;
        }

        private DelegateCommand<string> _getSuggestionsCommand = null;
        public DelegateCommand<string> GetSuggestionsCommand => _getSuggestionsCommand ??= new DelegateCommand<string>(GetSuggestionsAsync);

        private DelegateCommand<string> _submitCommand = null;
        public DelegateCommand<string> SubmitCommand =>_submitCommand ??= new DelegateCommand<string>(Submit);

        public async void GetSuggestionsAsync(string filter)
        {
            Suggestions = await Service.GetSuggestionsByLemma(filter, 50);
        }

        private async void Submit(string lemma)
        {
            Text = SelectedLemma = lemma;
            LexicalEntries = await Service.GetByLemma(lemma);
        }
    }
}