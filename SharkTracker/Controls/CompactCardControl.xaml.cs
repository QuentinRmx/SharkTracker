using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharkTracker.Models;

namespace SharkTracker.Controls
{
    public partial class CompactCardControl
    {
        public static readonly DependencyProperty CardProperty = DependencyProperty.Register("Card", typeof(Card),
            typeof(CompactCardControl), new PropertyMetadata(default(Card)));


        public Card Card
        {
            get => (Card) GetValue(CardProperty);
            set => SetValue(CardProperty, value);
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
            await Card.GetArtworkFromInternet();
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
            cardNameTb.Text = Card.Name;
        }

        private void SetCardCost()
        {
            cardCostTb.Text = Card.Cost.ToString();
        }

        /// <summary>
        /// Crop the artwork and set it as the background.
        /// </summary>
        private void SetArtworkBackground()
        {
            if (Card.BitmapArtwork == null)
                return;
            CroppedBitmap cb = new CroppedBitmap(
                Card.BitmapArtwork,
                new Int32Rect(30, 200, 620, 200));
            TransformedBitmap tb = new TransformedBitmap(cb, new ScaleTransform(0.25, 0.25));
            artworkImageBrush.ImageSource = tb;
        }


        /// <summary>
        /// Scale down the artwork and pass it to the popup.
        /// </summary>
        private void SetPopupArtwork()
        {
            if (Card.BitmapArtwork == null)
                return;
            TransformedBitmap scaledArt = new TransformedBitmap(Card.BitmapArtwork, new ScaleTransform(0.5, 0.5));
            artworkFullPopup.Source = scaledArt;
        }
    }
}