using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WordNet.Wpf.Core;

namespace WordNet.Wpf.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public IRegionManager RegionManager { get; }

        public ShellViewModel(IRegionManager regionManager, ApplicationCommands applicationCommands)
        {
            RegionManager = regionManager;
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
        }

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ??= new DelegateCommand<string>(navigationPath => RegionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath));
    }
}
