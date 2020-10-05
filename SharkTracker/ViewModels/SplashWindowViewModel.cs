#nullable enable
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharkTrackerCore;

namespace SharkTracker.ViewModels
{
    public class SplashWindowViewModel : ViewModelBase
    {

        // ATTRIBUTES

        private string _textProgress = "Updating Data from Riot...";

        public string TextProgress
        {
            get => _textProgress;
            set
            {
                _textProgress = value;
                RaisePropertyChanged(nameof(TextProgress));
            }
        }

        public ICommand UpdateCommand => new RelayCommand(async () => await UpdateAsync());

        // Dummy value so when it appears it doesn't look full already.
        private int _progressBarMax = 100;

        public int ProgressBarMax
        {
            get => _progressBarMax;
            set
            {
                _progressBarMax = value;
                RaisePropertyChanged(nameof(ProgressBarMax));
            }
        }

        private int _currentProgress = 0;

        public int CurrentProgress
        {
            get => _currentProgress;
            set
            {
                _currentProgress = value;
                RaisePropertyChanged(nameof(CurrentProgress));
            }
            
        }

        private bool _isProgressIndeterminate = true;

        public bool IsProgressIndeterminate
        {
            get => _isProgressIndeterminate;
            set
            {
                _isProgressIndeterminate = value;
                RaisePropertyChanged(nameof(IsProgressIndeterminate));
            }
        }


        // CONSTRUCTORS

        public SplashWindowViewModel()
        {
            IsProgressIndeterminate = true;
        }

        // METHODS
        
        private async Task UpdateAsync()
        {
            await Task.Run(() => App.SharkTracker.UpdateFromRiot(UpdateProgressHandler));
        }

        private void UpdateProgressHandler(object? sender, EventArgs e)
        {
            if (!(sender is RiotDownloader dl))
                return;
            TextProgress = $"Updating Data from Riot : {dl.ProgressPercent:F2}% ({dl.Progress}/{dl.TotalProgress} files)";
            CurrentProgress = (int) dl.Progress;
            ProgressBarMax = (int) dl.TotalProgress;
            IsProgressIndeterminate = !(ProgressBarMax > 0);
            if (Math.Abs(dl.Progress - dl.TotalProgress) < .00001)
            {
                // TODO: Signal we can go to next window.
                TextProgress = "Ready";
            }
        }

    }
}