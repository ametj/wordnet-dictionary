using Prism.Mvvm;

namespace WordNet.Wpf.Core
{
    public class ViewModelBase : BindableBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public void LoadingDone()
        {
            IsLoading = false;
        }
    }
}