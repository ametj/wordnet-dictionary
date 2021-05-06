/// Original file from https://github.com/quicoli/WPF-AutoComplete-TextBox
/// MIT License
/// Copyright (c) 2016 Paulo Roberto Quicoli

using System.Windows.Controls.Primitives;

namespace WordNet.Wpf.Core
{
    public class SelectionAdapter
    {
        public SelectionAdapter(Selector selector)
        {
            SelectorControl = selector;
        }

        public delegate void SelectionChangedEventHandler();

        public event SelectionChangedEventHandler SelectionChanged;

        public Selector SelectorControl { get; set; }

        public void DecrementSelection()
        {
            if (SelectorControl.SelectedIndex == -1)
            {
                SelectorControl.SelectedIndex = SelectorControl.Items.Count - 1;
            }
            else
            {
                SelectorControl.SelectedIndex -= 1;
            }

            SelectionChanged?.Invoke();
        }

        public void IncrementSelection()
        {
            if (SelectorControl.SelectedIndex == SelectorControl.Items.Count - 1)
            {
                SelectorControl.SelectedIndex = -1;
            }
            else
            {
                SelectorControl.SelectedIndex += 1;
            }

            SelectionChanged?.Invoke();
        }
    }
}