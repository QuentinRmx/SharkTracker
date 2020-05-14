using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharkTracker.Models;

namespace SharkTracker.Controls
{
    public partial class CardCollectionControl : UserControl
    {
        public static readonly DependencyProperty CardCollectionProperty = DependencyProperty.Register("CardCollection",
            typeof(Card),
            typeof(CardCollectionControl), new PropertyMetadata(default(Card)));


        public Card CardCollection
        {
            get => (Card) GetValue(CardCollectionProperty);
            set => SetValue(CardCollectionProperty, value);
        }

        public CardCollectionControl()
        {
            InitializeComponent();
            Loaded += (sender, args) => UpdateUI();
            artworkImage.MouseEnter += (sender, args) => SetPopupDisplayState(true);
            artworkImage.MouseLeave += (sender, args) => SetPopupDisplayState(false);
        }

        private void SetPopupDisplayState(bool shouldDisplay)
        {
            popupArtwork.IsOpen = shouldDisplay;
        }

        /// <summary>
        /// Scale down the artwork and pass it to the popup.
        /// </summary>
        private void SetPopupArtwork()
        {
            if (CardCollection.BitmapArtwork == null)
                return;
            TransformedBitmap scaledArt =
                new TransformedBitmap(CardCollection.BitmapArtwork, new ScaleTransform(0.5, 0.5));
            artworkFullPopup.Source = scaledArt;
        }

        private async void UpdateUI()
        {
            SetCardName();
            SetOwnedQuantity();
            await CardCollection.LoadArtwork();
            SetArtwork();
            SetPopupArtwork();
        }

        private void SetOwnedQuantity()
        {
            quantityLabel.Content = CardCollection.QuantityOwned;
        }

        private void SetArtwork()
        {
            if (CardCollection.BitmapArtwork == null)
                return;
            TransformedBitmap scaledArt =
                new TransformedBitmap(CardCollection.BitmapArtwork, new ScaleTransform(0.125, 0.125));
            artworkImage.Source = scaledArt;
        }

        private void SetCardName()
        {
            cardName.Content = CardCollection.Name;
        }

        private void IncrementOwned(object sender, RoutedEventArgs e)
        {
            if (CardCollection.QuantityOwned == 3) return;
            CardCollection.QuantityOwned++;
            SetOwnedQuantity();
        }

        private void DecrementOwned(object sender, RoutedEventArgs e)
        {
            if (CardCollection.QuantityOwned == 0) return;
            CardCollection.QuantityOwned--;
            SetOwnedQuantity();
        }
    }
}