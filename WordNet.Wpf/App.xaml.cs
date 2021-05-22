using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using Serilog;
using Serilog.Events;
using System;
using System.Configuration;
using System.Reflection;
using System.Windows;
using Unity;
using Unity.Microsoft.DependencyInjection;
using WordNet.Data;
using WordNet.Service;
using WordNet.Wpf.Core;
using WordNet.Wpf.Service;
using WordNet.Wpf.Views;
using WordNet.Wpf.Views.Dictionary;
using WordNet.Wpf.Views.Settings;

namespace WordNet.Wpf
{
    public partial class App : PrismApplication
    {
        protected override IContainerExtension CreateContainerExtension()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            serviceCollection.AddDbContext<WordNetDbContext>(optionsBuilder =>
                optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["WordNetData"].ConnectionString));

            var connectionString = ConfigurationManager.ConnectionStrings["UserData"].ConnectionString
                .Replace("%APP_DATA%", GetUserAppDataPath());
            serviceCollection.AddDbContext<UserDbContext>(optionsBuilder =>
                optionsBuilder.UseSqlite(connectionString));

            var container = new UnityContainer();
            container.BuildServiceProvider(serviceCollection);

            return new UnityContainerExtension(container);
        }

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterSingleton<IWordNetDataService, WordNetDataService>();
            container.RegisterSingleton<IUserDataService, UserDataService>();
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

        protected override void Initialize()
        {
            InitializeLogging();

            base.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Wpf.Properties.Settings.Default.Save();
        }

        public static string GetUserAppDataPath()
        {
            var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            var companyAttribute = (AssemblyCompanyAttribute)attributes[0];

            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path += @"\" + companyAttribute.Company;

            return path;
        }

        private static void InitializeLogging()
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
#if DEBUG
            loggerConfiguration.WriteTo.Trace();
#else
            loggerConfiguration.WriteTo.File(System.IO.Path.Combine(GetUserAppDataPath(), "Log.txt"));
#endif
            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}