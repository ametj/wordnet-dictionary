using MahApps.Metro.Controls;
using Prism.Commands;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WordNet.Wpf.Core;
using WordNet.Wpf.ViewModels;

namespace WordNet.Wpf.Views
{
    public partial class Shell : MetroWindow
    {
        public Shell(IRegionManager regionManager, IApplicationCommands commands)
        {
            InitializeComponent();

            RegionManager.SetRegionName(HamburgerMenuContent, RegionNames.ContentRegion);
            RegionManager.SetRegionManager(HamburgerMenuContent, regionManager);

            InitializeCommands(commands);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized && Properties.Settings.Default.ShowNotificationAreaIcon && Properties.Settings.Default.MinimizeToNotificationArea) 
                Hide();

            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Properties.Settings.Default.ShowNotificationAreaIcon && Properties.Settings.Default.CloseToNotificationArea)
            {
                e.Cancel = true;
                Hide();
            }
            base.OnClosing(e);
        }

        private void InitializeCommands(IApplicationCommands commands)
        {
            var showWindowCommand = new DelegateCommand(() => ShowWindow());
            commands.ShowWindowCommand.RegisterCommand(showWindowCommand);

            var hideWindowCommand = new DelegateCommand(() => HideWindow());
            commands.HideWindowCommand.RegisterCommand(hideWindowCommand);

            var showHideWindowCommand = new DelegateCommand(() =>
            {
                if (IsVisible && WindowState != WindowState.Minimized)
                    HideWindow();
                else
                    ShowWindow();
            });
            commands.ShowHideWindowCommand.RegisterCommand(showHideWindowCommand);

            var closeWindowCommand = new DelegateCommand(() => Application.Current.Shutdown());
            commands.CloseWindowCommand.RegisterCommand(closeWindowCommand);
        }

        private void ShowWindow()
        {
            Show();
            Activate();
            WindowState = WindowState.Normal;
        }

        private void HideWindow()
        {
            if (Properties.Settings.Default.ShowNotificationAreaIcon && Properties.Settings.Default.MinimizeToNotificationArea)
                Application.Current.MainWindow.Hide();
            else
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
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