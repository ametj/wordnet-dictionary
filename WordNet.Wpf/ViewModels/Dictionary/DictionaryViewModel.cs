using Prism.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WordNet.Data.Model;
using WordNet.Service;
using WordNet.Wpf.Core;

namespace WordNet.Wpf.ViewModels.Dictionary
{
    public class DictionaryViewModel : ViewModelBase
    {
        public DictionaryViewModel(IWordNetService service)
        {
            Service = service;
        }

        public IWordNetService Service { get; }

        private string _text;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _selectedLemma;

        public string SelectedLemma
        {
            get => _selectedLemma;
            set => SetProperty(ref _selectedLemma, value);
        }

        private IEnumerable<LexicalEntry> _lexicalEntries;

        public IEnumerable<LexicalEntry> LexicalEntries
        {
            get => _lexicalEntries;
            set => SetProperty(ref _lexicalEntries, value);
        }

        private IEnumerable<string> _suggestions;

        public IEnumerable<string> Suggestions
        {
            get => _suggestions;
            set => SetProperty(ref _suggestions, value);
        }

        private ICommand _getSuggestionsCommand = null;
        public ICommand GetSuggestionsCommand => _getSuggestionsCommand ??= new DelegateCommand<string>(GetSuggestions);

        private ICommand _submitCommand = null;
        public ICommand SubmitCommand => _submitCommand ??= new DelegateCommand<string>(Submit);

        private ICommand _getRandomCommand = null;
        public ICommand GetRandomCommand => _getRandomCommand ??= new DelegateCommand(GetRandom);

        public async void GetSuggestions(string filter)
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
            IsLoading = true;
            Text = SelectedLemma = lemma;
            LexicalEntries = await Service.GetByLemma(lemma);
        }

        private async void GetRandom()
        {
            IsLoading = true;
            var lemma = await Service.GetRandomLemma();
            SubmitCommand.Execute(lemma);
        }
    }
}