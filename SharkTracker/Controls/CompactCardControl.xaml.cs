using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharkTracker.Models;

namespace SharkTracker.Controls
{
    public partial class CompactCardControl
    {
        public static readonly DependencyProperty CardCompactProperty = DependencyProperty.Register("CardCompact", typeof(Card),
            typeof(CompactCardControl), new PropertyMetadata(default(Card)));


        public Card CardCompact
        {
            get => (Card) GetValue(CardCompactProperty);
            set => SetValue(CardCompactProperty, value);
        }


        // CONSTRUCTOR

        public CompactCardControl()
        {
            InitializeComponent();
            Loaded += (sender, args) => UpdateUI();
            MouseEnter += (sender, args) => SetPopupDisplayState(true);
            MouseLeave += (sender, args) => SetPopupDisplayState(false);
        }

        private async void UpdateUI()
        {
            SetCardCost();
            SetCardName();
            await CardCompact.LoadArtwork();
            SetArtworkBackground();
            SetPopupArtwork();
        }

        // METHODS

        private void SetPopupDisplayState(bool shouldDisplay)
        {
            popupArtwork.IsOpen = shouldDisplay;
        }

        private void SetCardName()
        {
            cardNameTb.Text = CardCompact.Name;
        }

        private void SetCardCost()
        {
            cardCostTb.Text = CardCompact.Cost.ToString();
        }

        /// <summary>
        /// Crop the artwork and set it as the background.
        /// </summary>
        private void SetArtworkBackground()
        {
            if (CardCompact.BitmapArtwork == null)
                return;
            CroppedBitmap cb = new CroppedBitmap(
                CardCompact.BitmapArtwork,
                new Int32Rect(30, 200, 620, 200));
            TransformedBitmap tb = new TransformedBitmap(cb, new ScaleTransform(0.25, 0.25));
            artworkImageBrush.ImageSource = tb;
        }


        /// <summary>
        /// Scale down the artwork and pass it to the popup.
        /// </summary>
        private void SetPopupArtwork()
        {
            if (CardCompact.BitmapArtwork == null)
                return;
            TransformedBitmap scaledArt = new TransformedBitmap(CardCompact.BitmapArtwork, new ScaleTransform(0.5, 0.5));
            artworkFullPopup.Source = scaledArt;
        }
    }
}