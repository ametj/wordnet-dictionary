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
            Application.Current.MainWindow.Topmost = Properties.Settings.Default.AllwaysOnTop;

            _themeService.LoadSettings();
        }
    }
}