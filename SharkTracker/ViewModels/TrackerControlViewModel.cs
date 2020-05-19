using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharkTracker.Communicator;
using SharkTracker.LoRDeckCodes;
using SharkTracker.Models;
using SharkTracker.Utils;
using SharkTracker.Views;

namespace SharkTracker.ViewModels
{
    public class TrackerControlViewModel : ViewModelBase
    {
// ATTRIBUTES

        private ObservableCollection<CompactCardControlViewModel> _cardControlViewModels;

        public ObservableCollection<CompactCardControlViewModel> CardControlViewModels
        {
            get => _cardControlViewModels;
            set
            {
                _cardControlViewModels = value;
                RaisePropertyChanged(nameof(CardControlViewModels));
            }
        }


        private readonly AbstractLorCommunicator _communicator;

        private bool _deckLoaded;

        public bool DeckLoaded
        {
            get => _deckLoaded;
            set
            {
                _deckLoaded = value;
                RaisePropertyChanged(nameof(DeckLoaded));
                RaisePropertyChanged(nameof(ShowNoDeckText));
            }
        }

        public Visibility ShowNoDeckText => (DeckLoaded) ? Visibility.Collapsed : Visibility.Visible;

        public ICommand BtnActive_OnClickCommand => new RelayCommand(ButtonActiveOnClick);

        public object IsActiveText =>
            (_communicator != null && _communicator.IsActive) ? "Stop tracker" : "Start tracker";

        private bool _deckShowed;

        private List<CardCodeAndCount> deckAsCardCodeAndCount;

        private readonly List<Card> _allCards;

        private readonly DispatcherTimer _timer;

        // CONSTRUCTORS

        public TrackerControlViewModel()
        {
            _communicator = new LorCommunicator(new HttpClient());
            _communicator.OnStarting += (sender, args) => CommunicatorActivityChanged();
            _communicator.OnStopping += (sender, args) => CommunicatorActivityChanged();
            CardControlViewModels = new ObservableCollection<CompactCardControlViewModel>();
            _allCards = CardsManager.Instance.GetAllCards();
            _timer = new DispatcherTimer();
            _timer.Tick += LoadDeck;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
        }


        // METHODS


        private void LoadDeck(object sender, EventArgs eventArgs)
        {
            List<CardCodeAndCount> deckResp = _communicator.GetActiveDeck();
            if (deckResp == null)
                return;

            if (deckResp.Count == 0)
            {
                _timer.Stop();
                _communicator.Stop();
            }

            if (deckResp.Count == 0)
            {
                if (DeckLoaded)
                {
                    CardControlViewModels.Clear();
                    DeckLoaded = false;
                    _deckShowed = false;
                }

                return;
            }

            DeckLoaded = true;
            deckAsCardCodeAndCount = deckResp;
            DisplayDeck();
        }

        private void DisplayDeck()
        {
            if (_deckShowed)
                return;
            _deckShowed = true;
            List<CompactCardControlViewModel> toAdd = new List<CompactCardControlViewModel>();
            foreach (CardCodeAndCount cardCodeAndCount in deckAsCardCodeAndCount)
            {
                Card card = _allCards.First(c => c.Code == cardCodeAndCount.CardCode);
                CompactCardControlViewModel cardControl = new CompactCardControlViewModel
                {
                    Card = card,
                    CardCodeAndCount = cardCodeAndCount
                };
                toAdd.Add(cardControl);
            }

            toAdd = new List<CompactCardControlViewModel>(toAdd.OrderBy(c => c.Card.Cost));
            foreach (CompactCardControlViewModel compactCardControlView in toAdd)
            {
                CardControlViewModels.Add(compactCardControlView);
            }
        }


        private void ButtonActiveOnClick()
        {
            if (_communicator.IsActive)
            {
                _communicator.Stop();
                _timer.Stop();
            }
            else
            {
                _communicator.Start();
                _timer.Start();
            }
        }

        private void CommunicatorActivityChanged()
        {
            RaisePropertyChanged(nameof(IsActiveText));
        }
    }
}