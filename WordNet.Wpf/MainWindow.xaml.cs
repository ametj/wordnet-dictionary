using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WordNet.Wpf.ViewModel;

namespace WordNet.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = MainWindowViewModel = mainWindowViewModel;
        }

        public MainWindowViewModel MainWindowViewModel { get; }

        private void ConfirmSelection(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock)
            {
                var text = (sender as TextBlock).Text;
                MainWindowViewModel.ConfirmSelection(text);
                suggestionTextBox.Text = text;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
    }
}