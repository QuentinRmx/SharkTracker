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
        /// <summary>
        /// Allow to set the path to the artwork from the XAML as a property.
        /// </summary>
        public static readonly DependencyProperty CardCodeProperty =
            DependencyProperty.Register("CardCode", typeof(string), typeof(CompactCardControl));

        /// <summary>
        /// Allow to set the card's name from the XAML as a property.
        /// </summary>
        public static readonly DependencyProperty CardNameProperty = DependencyProperty.Register("CardName",
            typeof(string), typeof(CompactCardControl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty CardCostProperty = DependencyProperty.Register("CardCost",
            typeof(string), typeof(CompactCardControl), new PropertyMetadata(default(string)));

        /// <summary>
        /// Path to the artwork picture.
        /// </summary>
        public string CardCode
        {
            get => GetValue(CardCodeProperty) as string;
            set
            {
                SetValue(CardCodeProperty, value);
                SetArtworkBackground();
                SetPopupArtwork();
            }
        }

        /// <summary>
        /// Card's name as written on the card.
        /// </summary>
        public string CardName
        {
            get => GetValue(CardNameProperty) as string;
            set
            {
                SetValue(CardNameProperty, value);
                SetCardName();
            }
        }

        public string CardCost
        {
            get => (string) GetValue(CardCostProperty);
            set
            {
                SetValue(CardCostProperty, value);
                SetCardCost();
            }
        }


        // CONSTRUCTOR

        public CompactCardControl()
        {
            InitializeComponent();
            Loaded += (sender, args) => SetArtworkBackground();
            Loaded += (sender, args) => SetPopupArtwork();
            Loaded += (sender, args) => SetCardName();
            Loaded += (sender, args) => SetCardCost();
            MouseEnter += (sender, args) => SetPopupDisplayState(true);
            MouseLeave += (sender, args) => SetPopupDisplayState(false);
        }


        // METHODS

        private void SetPopupDisplayState(bool shouldDisplay)
        {
            popupArtwork.IsOpen = shouldDisplay;
        }

        private void SetCardName()
        {
            cardNameTb.Text = CardName;
        }

        private void SetCardCost()
        {
            cardCostTb.Text = CardCost;
        }

        /// <summary>
        /// Crop the artwork and set it as the background.
        /// </summary>
        private void SetArtworkBackground()
        {
            if (string.IsNullOrEmpty(CardCode) ||
                !Uri.IsWellFormedUriString(Constants.PATH_IMG_PREFIX + CardCode + ".png", UriKind.Relative))
            {
                return;
            }

            CroppedBitmap cb = new CroppedBitmap(
                new BitmapImage(new Uri(Constants.PATH_IMG_PREFIX + CardCode + ".png", UriKind.Relative)),
                new Int32Rect(30, 200, 620, 200));
            TransformedBitmap tb = new TransformedBitmap(cb, new ScaleTransform(0.25, 0.25));
            artworkImageBrush.ImageSource = tb;
        }

        /// <summary>
        /// Scale down the artwork and pass it to the popup.
        /// </summary>
        private void SetPopupArtwork()
        {
            if (string.IsNullOrEmpty(CardCode) ||
                !Uri.IsWellFormedUriString(Constants.PATH_IMG_PREFIX + CardCode+ ".png", UriKind.Relative))
            {
                return;
            }

            BitmapImage fullImg = new BitmapImage(new Uri(Constants.PATH_IMG_PREFIX + CardCode + ".png", UriKind.Relative));
            TransformedBitmap scaledArt = new TransformedBitmap(fullImg, new ScaleTransform(0.5, 0.5));
            artworkFullPopup.Source = scaledArt;
        }
    }
}