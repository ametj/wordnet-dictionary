using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WordNet.Wpf.Service;

namespace WordNet.Wpf.ViewModels.Settings
{
    public class SettingsViewModel : BindableBase
    {
        private readonly IThemeService ThemeService;

        public SettingsViewModel(IThemeService themeService)
        {
            ThemeService = themeService;

            _accentColors = ThemeService.GetAvailableAccentColors();
            _selectedAccentColor = _accentColors.Where(a => a.Name == ThemeService.AccentColor).First();
        }

        private ICollection<AccentColor> _accentColors;

        public ICollection<AccentColor> AccentColors
        {
            get { return _accentColors; }
            set { SetProperty(ref _accentColors, value); }
        }

        private AccentColor _selectedAccentColor;

        public AccentColor SelectedAccentColor
        {
            get { return _selectedAccentColor; }
            set
            {
                ThemeService.AccentColor = value.Name;
                SetProperty(ref _selectedAccentColor, value);
            }
        }

        public bool UseWindowsAccentColor
        {
            get { return ThemeService.UseWindowsAccentColor; }
            set
            {
                ThemeService.UseWindowsAccentColor = value;
                RaisePropertyChanged(nameof(UseWindowsAccentColor));
            }
        }

        public ThemeMode SelectedThemeMode
        {
            get { return ThemeService.ThemeMode; }
            set
            {
                ThemeService.ThemeMode = value;

                RaisePropertyChanged(nameof(SelectedThemeMode));
                RaisePropertyChanged(nameof(IsThemeModeLight));
                RaisePropertyChanged(nameof(IsThemeModeDark));
                RaisePropertyChanged(nameof(IsThemeModeWindowsDefault));
            }
        }

        public bool IsThemeModeLight
        {
            get { return ThemeService.ThemeMode == ThemeMode.Light; }
            set { SelectedThemeMode = ThemeMode.Light; }
        }

        public bool IsThemeModeDark
        {
            get { return ThemeService.ThemeMode == ThemeMode.Dark; }
            set { SelectedThemeMode = ThemeMode.Dark; }
        }

        public bool IsThemeModeWindowsDefault
        {
            get { return ThemeService.ThemeMode == ThemeMode.WindowsDefault; }
            set { SelectedThemeMode = ThemeMode.WindowsDefault; }
        }

        public bool AlwaysOnTop
        {
            get { return Properties.Settings.Default.AllwaysOnTop; }
            set
            {
                Properties.Settings.Default.AllwaysOnTop = value;
                Application.Current.MainWindow.Topmost = value;
                RaisePropertyChanged(nameof(AlwaysOnTop));
            }
        }
    }
}