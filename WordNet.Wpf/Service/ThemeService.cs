using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WordNet.Util;

namespace WordNet.Wpf.Service
{
    public class ThemeService : IThemeService
    {
        private ThemeSettings ThemeSettings;

        public string AccentColor
        {
            get { return ThemeSettings.AccentColor; }
            set
            {
                ThemeSettings.AccentColor = value;
                ThemeManager.Current.ChangeThemeColorScheme(Application.Current, value);
            }
        }

        public bool UseWindowsAccentColor
        {
            get { return ThemeSettings.UseWindowsAccentColor; }
            set
            {
                ThemeSettings.UseWindowsAccentColor = value;

                if (value)
                {
                    SyncTheme();
                }
                else
                {
                    ThemeManager.Current.ChangeThemeColorScheme(Application.Current, AccentColor);
                }
            }
        }

        public ThemeMode ThemeMode
        {
            get { return ThemeSettings.ThemeMode; }
            set
            {
                ThemeSettings.ThemeMode = value;
                ApplyThemeMode();
            }
        }

        public ICollection<AccentColor> GetAvailableAccentColors()
        {
            return ThemeManager.Current.Themes
                .GroupBy(x => x.ColorScheme)
                .Select(a => new AccentColor { Name = a.Key, ColorBrush = a.First().ShowcaseBrush })
                .OrderBy(a =>
                {
                    var color = (a.ColorBrush as SolidColorBrush).Color;
                    var (H, S, _) = color.ToHSL();
                    return Math.Round(1 - S) + H;
                })
                .ToList();
        }

        public void LoadSettings()
        {
            ThemeSettings = Properties.Settings.Default.ThemeSettings;

            if (ThemeSettings is null)
            {
                ThemeSettings = Properties.Settings.Default.ThemeSettings = ThemeSettings.Default();
            }

            if (UseWindowsAccentColor || ThemeMode == ThemeMode.WindowsDefault)
            {
                SyncTheme();
            }
            if (!UseWindowsAccentColor)
            {
                ThemeManager.Current.ChangeThemeColorScheme(Application.Current, AccentColor);
            }
            if (ThemeMode != ThemeMode.WindowsDefault)
            {
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, ThemeMode.ToString());
            }
        }

        private void ApplyThemeMode()
        {
            var themeMode = ThemeMode.ToString();

            if (ThemeMode == ThemeMode.WindowsDefault)
            {
                SyncTheme();
            }
            else
            {
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, themeMode);
            }
        }

        private void SyncTheme()
        {
            var mode = ThemeSyncMode.DoNotSync;
            if (UseWindowsAccentColor)
            {
                mode |= ThemeSyncMode.SyncWithAccent;
            }
            if (ThemeMode == ThemeMode.WindowsDefault)
            {
                mode |= ThemeSyncMode.SyncWithAppMode;
            }
            ThemeManager.Current.ThemeSyncMode = mode;
            ThemeManager.Current.SyncTheme();
        }
    }
}