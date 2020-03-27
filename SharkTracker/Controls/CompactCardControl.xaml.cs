using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharkTracker.Utils;

namespace SharkTracker.Controls
{
    public partial class CompactCardControl : UserControl
    {
        public static readonly DependencyProperty ArtworkPathProperty =
            DependencyProperty.Register("ArtworkPath", typeof(string), typeof(CompactCardControl));

        public static readonly DependencyProperty CardNameProperty = DependencyProperty.Register("CardName",
            typeof(string), typeof(CompactCardControl), new PropertyMetadata(default(string)));

        public string ArtworkPath
        {
            get => GetValue(ArtworkPathProperty) as string;
            set
            {
                SetValue(ArtworkPathProperty, value);
                SetArtworkBackground();
            }
        }

        public string CardName
        {
            get => GetValue(CardNameProperty) as string;
            set
            {
                SetValue(CardNameProperty, value);
                SetCardName();
            }
        }

        private void SetCardName()
        {
            cardName.Text = CardName;
        }

        public CompactCardControl()
        {
            InitializeComponent();
            Loaded += (sender, args) => SetArtworkBackground();
            Loaded += (sender, args) => SetCardName();
            MouseEnter += (sender, args) => ShowFullCard();
            MouseLeave += (sender, args) => HideFullCard();
        }

        private void ShowFullCard()
        {
            popupArtwork.IsOpen = true;
        }

        private void HideFullCard()
        {
            popupArtwork.IsOpen = false;
        }

        private void SetArtworkBackground()
        {
            if (string.IsNullOrEmpty(ArtworkPath) ||
                !Uri.IsWellFormedUriString(Constants.PATH_IMG_PREFIX + ArtworkPath, UriKind.Relative))
            {
                return;
            }

            CroppedBitmap cb = new CroppedBitmap(
                new BitmapImage(new Uri(Constants.PATH_IMG_PREFIX + ArtworkPath, UriKind.Relative)),
                new Int32Rect(30, 200, 620, 200));
            TransformedBitmap tb = new TransformedBitmap(cb, new ScaleTransform(0.25, 0.25));
            artwork.ImageSource = tb;
            BitmapImage fullImg = new BitmapImage(new Uri(Constants.PATH_IMG_PREFIX + ArtworkPath, UriKind.Relative));
            TransformedBitmap scaledArt = new TransformedBitmap(fullImg, new ScaleTransform(0.5, 0.5));
            artworkFull.Source = scaledArt;
        }
    }
}