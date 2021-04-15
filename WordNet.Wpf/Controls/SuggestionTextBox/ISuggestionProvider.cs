/// Original file from https://github.com/quicoli/WPF-AutoComplete-TextBox
/// MIT License
/// Copyright (c) 2016 Paulo Roberto Quicoli

using System.Collections;
using System.Threading.Tasks;

namespace WordNet.Wpf.Controls.SuggestionTextBox
{
    public interface ISuggestionProvider
    {
        Task<IEnumerable> GetSuggestionsAsync(string filter);
    }
}