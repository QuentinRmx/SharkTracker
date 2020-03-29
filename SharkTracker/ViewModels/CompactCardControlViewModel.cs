using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace SharkTracker.ViewModels
{
    public class CompactCardControlViewModel : ViewModelBase
    {
        // ATTRIBUTES

        private string _cardName;

        public string CardName
        {
            get => _cardName;
            set
            {
                _cardName = value;
                RaisePropertyChanged(nameof(CardName));
            }
        }

        private string _cardCode;

        public string CardCode
        {
            get => _cardCode;
            set
            {
                _cardCode = value;
                RaisePropertyChanged(nameof(CardCode));
            }
        }

        private string _cardCost;

        public string CardCost
        {
            get => _cardCost;
            set
            {
                _cardCost = value;
                RaisePropertyChanged(nameof(CardCost));
            }
        }

        // CONSTRUCTORS

        public CompactCardControlViewModel() : base()
        {
        }

        // METHODS
    }
}