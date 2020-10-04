using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using SharkTracker.LoRDeckCodes;
using SharkTracker.Models;
using SharkTrackerCore.Models;

namespace SharkTracker.ViewModels
{
    public class CompactCardControlViewModel : ViewModelBase
    {
        // ATTRIBUTES

        private Card _card;

        public Card Card
        {
            get => _card;
            set
            {
                _card = value;
                RaisePropertyChanged(nameof(Card));
            }
        }

        private CardCodeAndCount _cardCodeAndCount;

        public CardCodeAndCount CardCodeAndCount
        {
            get => _cardCodeAndCount;
            set
            {
                _cardCodeAndCount = value;
                RaisePropertyChanged(nameof(CardCodeAndCount));
                RaisePropertyChanged(nameof(CopiesInDeck));
            }
        }

        public string CopiesInDeck => $"x{CardCodeAndCount.Count}";

        // CONSTRUCTORS

        public CompactCardControlViewModel() : base()
        {
        }

        // METHODS
    }
}