using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WordNet.Data;
using WordNet.Model;
using WordNet.Wpf.Controls.SuggestionTextBox;

namespace WordNet.Wpf.ViewModel
{
    public class MainWindowViewModel : BaseViewModel, ISuggestionProvider
    {
        private ObservableCollection<LexicalEntry> _lexicalEntries;
        private string _selectedLemma;
        public IWordNetService Service { get; }

        public string SelectedLemma
        {
            get => _selectedLemma;
            set { _selectedLemma = value; OnPropertyChanged(nameof(_selectedLemma)); }
        }

        public ObservableCollection<LexicalEntry> LexicalEntries
        {
            get => _lexicalEntries;
            set { _lexicalEntries = value; OnPropertyChanged(nameof(_selectedLemma)); }
        }

        public MainWindowViewModel(IWordNetService service)
        {
            Service = service;
            LexicalEntries = new ObservableCollection<LexicalEntry>();
        }

        public async void ConfirmSelection(string selectedLemma)
        {
            if (SelectedLemma == selectedLemma)
            {
                return;
            }

            SelectedLemma = selectedLemma;
            var data = await Service.GetByLemma(selectedLemma);

            LexicalEntries.Clear();
            foreach (var item in data)
            {
                LexicalEntries.Add(item);
            }
        }

        public async Task<IEnumerable> GetSuggestionsAsync(string filter)
        {
            return await Service.GetSuggestionsByLemma(filter.ToLower(), 50);
        }
    }
}