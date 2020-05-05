using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using SharkTracker.Models;

namespace SharkTracker.ViewModels
{
    public class CompactCardControlViewModel : ViewModelBase
    {
        // ATTRIBUTES

//        private string _cardName;
//
//        public string CardName
//        {
//            get => _cardName;
//            set
//            {
//                _cardName = value;
//                RaisePropertyChanged(nameof(CardName));
//            }
//        }
//
//        private string _cardCode;
//
//        public string CardCode
//        {
//            get => _cardCode;
//            set
//            {
//                _cardCode = value;
//                RaisePropertyChanged(nameof(CardCode));
//            }
//        }
//
//        private string _cardCost;
//
//        public string CardCost
//        {
//            get => _cardCost;
//            set
//            {
//                _cardCost = value;
//                RaisePropertyChanged(nameof(CardCost));
//            }
//        }
//
//        private BitmapSource _bitmapArtwork;
//
//
//        public BitmapSource BitmapArtwork
//        {
//            get => _bitmapArtwork;
//            set
//            {
//                _bitmapArtwork = value; 
//                RaisePropertyChanged(nameof(BitmapArtwork));
//            }
//        }

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

        public CompactCardControlViewModel() : base()
        {
        }

        // METHODS
    }
}