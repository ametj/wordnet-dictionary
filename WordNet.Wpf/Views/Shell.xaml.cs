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
        public Shell(IRegionManager regionManager)
        {
            InitializeComponent();

            RegionManager.SetRegionName(HamburgerMenuContent, RegionNames.ContentRegion);
            RegionManager.SetRegionManager(HamburgerMenuContent, regionManager);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ShellViewModel)?.NavigateCommand.Execute(nameof(Dictionary.Dictionary));
        }
    }
}