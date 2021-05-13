using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Configuration;
using System.Reflection;
using System.Windows;
using WordNet.Data;
using WordNet.Wpf.Core;
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
            RegisterDbContext(container);

            container.RegisterSingleton<IWordNetService, WordNetService>();
            container.RegisterSingleton<ISettingsService, SettingsService>();
            container.RegisterSingleton<IThemeService, ThemeService>();
            container.RegisterSingleton<IApplicationCommands, ApplicationCommands>();

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

        private static void RegisterDbContext(IContainerRegistry container)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WordNetDbContext>();
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["WordNetData"].ConnectionString);
            container.RegisterInstance(new WordNetDbContext(optionsBuilder.Options));

            var userOptionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            var connectionString = ConfigurationManager.ConnectionStrings["UserData"].ConnectionString.Replace("%APP_DATA%", GetUserAppDataPath());
            userOptionsBuilder.UseSqlite(connectionString);
            container.RegisterInstance(new UserDbContext(userOptionsBuilder.Options));
        }

        public static string GetUserAppDataPath()
        {
            var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            var companyAttribute = (AssemblyCompanyAttribute)attributes[0];

            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path += @"\" + companyAttribute.Company;

            return path;
        }
    }
}