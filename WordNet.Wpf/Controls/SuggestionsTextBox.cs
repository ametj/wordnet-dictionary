/// Based on https://github.com/quicoli/WPF-AutoComplete-TextBox
/// MIT License
/// Copyright (c) 2016 Paulo Roberto Quicoli

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WordNet.Wpf.Core;

namespace WordNet.Wpf.Controls
{
    [TemplatePart(Name = PartEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = PartSelector, Type = typeof(Selector))]
    public class SuggestionsTextBox : CommandSourceControl
    {
        #region "Fields"

        public static readonly DependencyProperty DisplayMemberProperty = DependencyProperty.Register(nameof(DisplayMember), typeof(string), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(null, OnSelectedItemChanged));
        public static readonly DependencyProperty MaxPopupHeightProperty = DependencyProperty.Register(nameof(MaxPopupHeight), typeof(int), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(600));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(SuggestionsTextBox));

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register(nameof(CharacterCasing), typeof(CharacterCasing), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(CharacterCasing.Normal));
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof(Watermark), typeof(string), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(string.Empty));
        
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof(Delay), typeof(int), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(200));
        public static readonly DependencyProperty GetSuggestionsCommandProperty = DependencyProperty.Register(nameof(GetSuggestionsCommand), typeof(ICommand), typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(null));

        private bool _isUpdatingText;

        public const string PartEditor = "PART_Editor";
        public const string PartPopup = "PART_Popup";
        public const string PartSelector = "PART_Selector";

        #endregion "Fields"

        #region "Constructors"

        static SuggestionsTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SuggestionsTextBox), new FrameworkPropertyMetadata(typeof(SuggestionsTextBox)));
        }

        #endregion "Constructors"

        #region "Properties"

        public BindingEvaluator BindingEvaluator { get; set; }

        public TextBox Editor { get; set; }

        public DispatcherTimer FetchTimer { get; set; }
        public Selector ItemsSelector { get; set; }
        public Popup Popup { get; set; }
        public SelectionAdapter SelectionAdapter { get; set; }

        public int MaxPopupHeight
        {
            get => (int)GetValue(MaxPopupHeightProperty);
            set => SetValue(MaxPopupHeightProperty, value);
        }

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public int Delay
        {
            get => (int)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        public string DisplayMember
        {
            get => (string)GetValue(DisplayMemberProperty);
            set => SetValue(DisplayMemberProperty, value);
        }

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        public CharacterCasing CharacterCasing
        {
            get => (CharacterCasing)GetValue(CharacterCasingProperty);
            set => SetValue(CharacterCasingProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            set => SetValue(ItemTemplateSelectorProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public ICommand GetSuggestionsCommand
        {
            get => (ICommand)GetValue(GetSuggestionsCommandProperty);
            set => SetValue(GetSuggestionsCommandProperty, value);
        }

        #endregion "Properties"

        #region "Methods"

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Editor = Template.FindName(PartEditor, this) as TextBox;
            Popup = Template.FindName(PartPopup, this) as Popup;
            ItemsSelector = Template.FindName(PartSelector, this) as Selector;

            BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));

            if (Editor != null)
            {
                Editor.TextChanged += OnEditorTextChanged;
                Editor.PreviewKeyDown += OnEditorKeyDown;
                Editor.LostFocus += OnEditorLostFocus;

                if (SelectedItem != null)
                {
                    _isUpdatingText = true;
                    Editor.Text = BindingEvaluator.Evaluate(SelectedItem);
                    _isUpdatingText = false;
                }
            }

            GotFocus += SuggestionsTextBox_GotFocus;

            if (Popup != null)
            {
                Popup.StaysOpen = false;
                Popup.Opened += OnPopupOpened;
            }
            if (ItemsSelector != null)
            {
                SelectionAdapter = new SelectionAdapter(ItemsSelector);
                SelectionAdapter.SelectionChanged += OnSelectionAdapterSelectionChanged;
                ItemsSelector.PreviewMouseDown += ItemsSelector_PreviewMouseDown;
            }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SuggestionsTextBox stb)
            {
                if (stb.Editor != null && !stb._isUpdatingText)
                {
                    stb._isUpdatingText = true;
                    stb.Editor.Text = stb.BindingEvaluator.Evaluate(e.NewValue);
                    stb._isUpdatingText = false;
                }
            }
        }

        private void OnSelectionAdapterSelectionChanged()
        {
            _isUpdatingText = true;
            Editor.Text = ItemsSelector.SelectedItem == null ? Filter : GetDisplayText(ItemsSelector.SelectedItem);
            Editor.Select(Editor.Text.Length, 0);
            ScrollToSelectedItem();
            _isUpdatingText = false;
        }

        private void ScrollToSelectedItem()
        {
            if (ItemsSelector is ListBox listBox && listBox.SelectedItem != null)
                listBox.ScrollIntoView(listBox.SelectedItem);
        }
       
        private void ItemsSelector_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource as FrameworkElement)?.DataContext == null)
                return;
            if (!ItemsSelector.Items.Contains(((FrameworkElement)e.OriginalSource)?.DataContext))
                return;
            ItemsSelector.SelectedItem = ((FrameworkElement)e.OriginalSource)?.DataContext;
            CommitSelection();
            e.Handled = true;
        }

        private string GetDisplayText(object dataItem)
        {
            if (BindingEvaluator == null)
            {
                BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));
            }
            if (dataItem == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(DisplayMember))
            {
                return dataItem.ToString();
            }
            return BindingEvaluator.Evaluate(dataItem);
        }

        private void SuggestionsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Editor?.Focus();
            Editor?.Select(Editor?.Text.Length ?? 0, 0);
        }
        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (IsDropDownOpen)
                        SelectionAdapter?.IncrementSelection();
                    else
                        GetSuggestions();
                    break;

                case Key.Up:
                    if (IsDropDownOpen)
                        SelectionAdapter?.DecrementSelection();
                    break;

                case Key.Enter:
                    // Always confirm changes made into TextBox
                    SelectedItem = string.IsNullOrEmpty(Editor.Text) ? null : Editor.Text;
                    CommitSelection();
                    break;

                case Key.Escape:
                    // Revert to previously selected, no need to confirm
                    ItemsSelector.SelectedItem = SelectedItem = string.IsNullOrEmpty(Filter) ? null : Filter;
                    if (GetDisplayText(ItemsSelector.SelectedItem) != Filter)
                        ItemsSelector.SelectedItem = null;
                    CommitSelection(false);
                    break;

                case Key.Tab:
                    // Prevent losing focus when in popup and pick selected without commiting
                    if (IsDropDownOpen) e.Handled = true;
                    CommitSelection(false);
                    break;

                default:
                    break;
            }
        }

        private void OnEditorLostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                IsDropDownOpen = false;
            }
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            ItemsSelector.SelectedItem = Text = string.IsNullOrEmpty(Editor.Text) ? null : Editor.Text;

            // Prevent getting suggestions when changing selection
            if (_isUpdatingText)
                return;

            if (!string.IsNullOrEmpty(Text))
            {
                ShouldGetSuggestions();
            }
            else
            {
                IsDropDownOpen = false;
            }
        }

        private void ShouldGetSuggestions()
        {
            if (FetchTimer != null) FetchTimer.IsEnabled = false;

            if (IsKeyboardFocusWithin)
            {
                EnableFetchTimer();
            }
            else
            {
                IsDropDownOpen = false;
            }
        }

        private void EnableFetchTimer()
        {
            if (FetchTimer == null)
            {
                FetchTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(Delay) };
                FetchTimer.Tick += OnFetchTimerTick;
            }
            FetchTimer.IsEnabled = true;
        }

        private void OnFetchTimerTick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Editor.Text))
                GetSuggestions();

            FetchTimer.IsEnabled = false;
        }

        private void GetSuggestions()
        {
            Filter = Editor.Text;
            GetSuggestionsCommand.Execute(Filter);
            IsDropDownOpen = true;
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            ItemsSelector.SelectedItem = SelectedItem;
        }

        private void CommitSelection(bool confirmSelection = true)
        {
            if (IsDropDownOpen && ItemsSelector.SelectedItem != null)
            {
                SelectedItem = ItemsSelector.SelectedItem;
                _isUpdatingText = true;
                Editor.Text = GetDisplayText(ItemsSelector.SelectedItem);
                _isUpdatingText = false;
            }
            else
            {
                ItemsSelector.SelectedItem = SelectedItem;
            }

            IsDropDownOpen = false;
            Editor.Select(Editor.Text.Length, 0);
            Filter = Editor.Text;
            if (FetchTimer != null) FetchTimer.IsEnabled = false;

            if (confirmSelection) ExecuteCommand();
        }
        #endregion "Methods"
    }
}