using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using Prism.Unity;
using System.Configuration;
using System.Windows;
using WordNet.Data;
using WordNet.Wpf.Views;
using WordNet.Wpf.Views.Dictionary;

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

            container.RegisterForNavigation<Dictionary>();
            container.RegisterForNavigation<Placeholder>();
        }

        protected override Window CreateShell()
        {
            var window = Container.Resolve<Shell>();
            return window;
        }
    }
}