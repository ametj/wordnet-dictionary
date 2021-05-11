using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;
using WordNet.Wpf.Core;
using WordNet.Wpf.Service;

namespace WordNet.Wpf.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public ShellViewModel(ISettingsService settingsService, IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            settingsService.LoadSettings();

            RegionManager = regionManager;
            
            ApplicationCommands = applicationCommands;
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
        }

        public IApplicationCommands ApplicationCommands { get; }

        public IRegionManager RegionManager { get; }

        private ICommand _navigateCommand;

        public ICommand NavigateCommand =>
            _navigateCommand ??= new DelegateCommand<string>(navigationPath => RegionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath));
    }
}