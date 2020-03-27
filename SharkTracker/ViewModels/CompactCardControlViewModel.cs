using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace SharkTracker.ViewModels
{
    public class CompactCardControlViewModel : ViewModelBase
    {
        private ImageBrush _artworkBrush;

        // ATTRIBUTES

        public ImageBrush ArtworkBrush
        {
            get => _artworkBrush;
            set
            {
                _artworkBrush = value;
                RaisePropertyChanged(nameof(ArtworkBrush));
            }
        }

        // CONSTRUCTORS

        public CompactCardControlViewModel() : base()
        {
//            BitmapImage bitmapImage = new BitmapImage(new Uri("../../../Data/en_us/img/cards/01DE001.png", UriKind.Relative));
            ArtworkBrush = new ImageBrush();
            
        }

        // METHODS


    }
}