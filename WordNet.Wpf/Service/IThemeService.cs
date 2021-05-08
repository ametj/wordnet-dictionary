using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace WordNet.Wpf.Service
{
    public interface IThemeService : ISettingsService
    {
        string AccentColor { get; set; }
        bool UseWindowsAccentColor { get; set; }
        ThemeMode ThemeMode { get; set; }

        ICollection<AccentColor> GetAvailableAccentColors();
    }

    public enum ThemeMode
    {
        Light,
        Dark,
        WindowsDefault
    }

    public class AccentColor
    {
        public string Name { get; set; }

        public Brush ColorBrush { get; set; }
    }

    [Serializable]
    public class ThemeSettings
    {
        public string AccentColor { get; set; }

        public ThemeMode ThemeMode { get; set; }

        public bool UseWindowsAccentColor { get; set; }

        public static ThemeSettings Default()
        {
            return new ThemeSettings
            {
                AccentColor = "Blue",
                ThemeMode = ThemeMode.Light,
                UseWindowsAccentColor = false
            };
        }
    }
}