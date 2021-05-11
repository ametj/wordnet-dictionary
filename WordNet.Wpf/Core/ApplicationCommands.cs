using Prism.Commands;

namespace WordNet.Wpf.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand NavigateCommand { get; }
        CompositeCommand ShowWindowCommand { get; }
        CompositeCommand HideWindowCommand { get; }
        CompositeCommand ShowHideWindowCommand { get; }
        CompositeCommand CloseWindowCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand NavigateCommand { get; } = new CompositeCommand();
        public CompositeCommand ShowWindowCommand { get; } = new CompositeCommand();
        public CompositeCommand HideWindowCommand { get; } = new CompositeCommand();
        public CompositeCommand ShowHideWindowCommand { get; } = new CompositeCommand();
        public CompositeCommand CloseWindowCommand { get; } = new CompositeCommand();
    }
}
