using MahApps.Metro.Controls;
using Prism.Regions;
using System.Windows;
using System.Windows.Input;
using WordNet.Wpf.Core;
using WordNet.Wpf.ViewModels;

namespace WordNet.Wpf.Views
{
    public partial class Shell : MetroWindow
    {
        public Shell(ShellViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = MainWindowViewModel = mainWindowViewModel;

            RegionManager.SetRegionName(HamburgerMenuContent, RegionNames.ContentRegion);
            RegionManager.SetRegionManager(HamburgerMenuContent, mainWindowViewModel.RegionManager);
        }

        public ShellViewModel MainWindowViewModel { get; }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.NavigateCommand.Execute(nameof(Dictionary.Dictionary));
        }
    }
}