using GalaSoft.MvvmLight;

namespace SharkTracker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        // ATTRIBUTES

        private string _currentMenuSelection = "../Controls/TrackerControl.xaml";

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