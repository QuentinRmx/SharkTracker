using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharkTracker.Communicator;
using SharkTracker.LoRDeckCodes;
using SharkTracker.Models;
using SharkTracker.Utils;

namespace SharkTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
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

        private AbstractLorCommunicator _communicator;
        
        private bool _deckLoaded = false;

        private bool _deckShowed = false;

        private List<CardCodeAndCount> deckAsCardCodeAndCount;

        private List<Card> _allCards;

        // CONSTRUCTORS

        public MainViewModel()
        {
            _communicator = new LorCommunicator(new HttpClient());
            CardControlViewModels = new ObservableCollection<CompactCardControlViewModel>();
            _allCards = new List<Card>();
            LoadAllCardsFromData();
            LoadDeck();
        }

        private void LoadAllCardsFromData()
        {
            string json = File.ReadAllText(Constants.PATH_CARD_SET_1);
            _allCards.AddRange(JsonConvert.DeserializeObject<List<Card>>(json));
            Console.WriteLine(_allCards.Count);
        }


        // METHODS


        private void LoadDeck()
        {
            List<CardCodeAndCount> deckResp = _communicator.GetActiveDeck();
            if (deckResp != null)
            {
                _deckLoaded = true;
                deckAsCardCodeAndCount = deckResp;
                DisplayDeck();
            }
        }
        
        private void DisplayDeck()
        {
            if (_deckShowed)
                return;
            List<CompactCardControlViewModel> toAdd = new List<CompactCardControlViewModel>();
            foreach (CardCodeAndCount cardCodeAndCount in deckAsCardCodeAndCount)
            {
                Card card = _allCards.First(c => c.Code == cardCodeAndCount.CardCode);
                CompactCardControlViewModel cardControl = new CompactCardControlViewModel
                {
                    CardName = card.Name,
                    CardCode = card.Code,
                    CardCost = card.Cost
                };
                toAdd.Add(cardControl);
            }

            toAdd = new List<CompactCardControlViewModel>(toAdd.OrderBy(c => c.CardCost));
            foreach (CompactCardControlViewModel compactCardControlView in toAdd)
            {
                CardControlViewModels.Add(compactCardControlView);
            }
            _deckShowed = true;
        }

    }
}