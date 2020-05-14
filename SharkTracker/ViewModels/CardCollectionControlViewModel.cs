using GalaSoft.MvvmLight;
using SharkTracker.Models;

namespace SharkTracker.ViewModels
{
    public class CardCollectionControlViewModel : ViewModelBase
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

        // CONSTRUCTORS

        // METHODS


    }
}