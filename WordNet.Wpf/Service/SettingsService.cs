using System.Windows;

namespace WordNet.Wpf.Service
{
    public class SettingsService : ISettingsService
    {
        private readonly IThemeService _themeService;

        public SettingsService(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public void LoadSettings()
        {
            Properties.Settings.Default.PropertyChanged += Settings_PropertyChanged;

            Application.Current.MainWindow.Topmost = Properties.Settings.Default.AlwaysOnTop;

            _themeService.LoadSettings();
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var settings = sender as Properties.Settings;

            if (e.PropertyName == nameof(settings.AlwaysOnTop))
                Application.Current.MainWindow.Topmost = settings.AlwaysOnTop;
        }
    }
}