using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharkTracker.Models;
using SharkTracker.Observation;
using SharkTracker.Utils;
using SharkTrackerCore.Models;

namespace SharkTracker.Controls
{
    public partial class CardCollectionControl : UserControl, Observer, IObservable
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
            cardCounter.Register(this);
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
            TransformedBitmap scaledArt =
                new TransformedBitmap(ImageHelper.LoadImageFromFile(SharkTrackerCore.Constants.PATH_CACHE_ARTWORK +
                                                                    CardCollection.Code +
                                                                    SharkTrackerCore.Constants.ARTWORK_SUFFIX),
                    new ScaleTransform(0.5, 0.5));
            artworkFullPopup.Source = scaledArt;
        }

        private void UpdateUI()
        {
            SetCardName();
            SetCounterValue();
            SetArtwork();
            SetPopupArtwork();
        }

        private void SetCounterValue()
        {
            cardCounter.Quantity = CardCollection.QuantityOwned;
        }

        // private void SetOwnedQuantity()
        // {
        //     quantityLabel.Content = CardCollection.QuantityOwned;
        // }

        private void SetArtwork()
        {
            TransformedBitmap scaledArt =
                new TransformedBitmap(ImageHelper.LoadImageFromFile(SharkTrackerCore.Constants.PATH_CACHE_ARTWORK +
                                                                    CardCollection.Code +
                                                                    SharkTrackerCore.Constants.ARTWORK_SUFFIX),
                    new ScaleTransform(0.125, 0.125));
            artworkImage.Source = scaledArt;
        }

        private void SetCardName()
        {
            cardName.Content = CardCollection.Name;
        }

        // private void IncrementOwned(object sender, RoutedEventArgs e)
        // {
        //     if (CardCollection.QuantityOwned == 3) return;
        //     CardCollection.QuantityOwned++;
        //     SetOwnedQuantity();
        // }
        //
        // private void DecrementOwned(object sender, RoutedEventArgs e)
        // {
        //     if (CardCollection.QuantityOwned == 0) return;
        //     CardCollection.QuantityOwned--;
        //     SetOwnedQuantity();
        // }

        /// <inheritdoc />
        public void Notify()
        {
            CardCollection.QuantityOwned = cardCounter.Quantity;
            NotifyAll();
        }

        private readonly List<Observer> _observers = new List<Observer>();

        /// <inheritdoc />
        public void Register(Observer o)
        {
            if (!_observers.Contains(o))
                _observers.Add(o);
        }

        /// <inheritdoc />
        public void Unregister(Observer o)
        {
            if (_observers.Contains(o))
                _observers.Remove(o);
        }

        /// <inheritdoc />
        public void NotifyAll()
        {
            _observers.ForEach(o => o.Notify());
        }
    }
}