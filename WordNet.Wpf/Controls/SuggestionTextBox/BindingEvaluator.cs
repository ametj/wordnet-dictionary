/// Original file from https://github.com/quicoli/WPF-AutoComplete-TextBox
/// MIT License
/// Copyright (c) 2016 Paulo Roberto Quicoli

using System.Windows;
using System.Windows.Data;

namespace WordNet.Wpf.Controls.SuggestionTextBox
{
    public class BindingEvaluator : FrameworkElement
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(BindingEvaluator), new FrameworkPropertyMetadata(string.Empty));

        public BindingEvaluator(Binding binding)
        {
            ValueBinding = binding;
        }

        public string Value
        {
            get => (string)GetValue(ValueProperty);

            set => SetValue(ValueProperty, value);
        }

        public Binding ValueBinding { get; set; }

        public string Evaluate(object dataItem)
        {
            DataContext = dataItem;
            SetBinding(ValueProperty, ValueBinding);
            return Value;
        }
    }
}