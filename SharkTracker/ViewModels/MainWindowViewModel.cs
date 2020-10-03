using GalaSoft.MvvmLight;

namespace SharkTracker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        // ATTRIBUTES
#if DEBUG
        private string _currentMenuSelection = "../Controls/TrackerControl.xaml";
#elif RELEASE
        private string _currentMenuSelection = "../Controls/TrackerControl.xaml";
#endif
        public string CurrentMenuSelection
        {
            get => _currentMenuSelection;
            set
            {
                _currentMenuSelection = value;

                RaisePropertyChanged(nameof(CurrentMenuSelection));
            }
        }

        // CONSTRUCTORS

        // METHODS


    }
}