using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using Prism.Unity;
using System.Configuration;
using System.Windows;
using WordNet.Data;
using WordNet.Wpf.Service;
using WordNet.Wpf.Views;
using WordNet.Wpf.Views.Dictionary;
using WordNet.Wpf.Views.Settings;

namespace WordNet.Wpf
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry container)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WordNetDbContext>();
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["Sqlite"].ConnectionString);

            container.RegisterInstance(optionsBuilder.Options);
            container.RegisterSingleton<WordNetDbContext>();
            container.RegisterSingleton<IWordNetService, WordNetService>();
            container.RegisterSingleton<ISettingsService, SettingsService>();
            container.RegisterSingleton<IThemeService, ThemeService>();

            container.RegisterForNavigation<Dictionary>();
            container.RegisterForNavigation<Settings>();
            container.RegisterForNavigation<Placeholder>();
        }

        protected override Window CreateShell()
        {
            var window = Container.Resolve<Shell>();
            return window;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Wpf.Properties.Settings.Default.Save();
        }
    }
}