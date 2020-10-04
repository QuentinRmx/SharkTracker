using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharkTracker.LoRDeckCodes;
using SharkTracker.Models;
using SharkTrackerCore.Models;

namespace SharkTracker.ViewModels
{
    public class DeckManagerViewModel : ViewModelBase
    {
        // ATTRIBUTES

        public ICommand ChangeRegionCommand => new RelayCommand<ERegion>(ChangeRegion);

        private List<Deck> _decks;

        public List<Deck> Decks
        {
            get => _decks;
            set
            {
                _decks = value;
                RaisePropertyChanged(nameof(Decks));
            }
        }

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

        private Deck _activeDeck;

        public Deck ActiveDeck
        {
            get => _activeDeck;
            set
            {
                _activeDeck = value;
                RefreshCurrentSelectedDeck();
                RaisePropertyChanged(nameof(ActiveDeck));
            }
        }


        // CONSTRUCTORS

        public DeckManagerViewModel()
        {
            // TODO: Load user's decks from disk.
            List<Deck> toAdd = new List<Deck>
            {
                Deck.NewFromDeckList(
                    LoRDeckEncoder.GetDeckFromCode("CEBQEAQDAMCAIAIECETTINQGAEBQEDAPDYSSQAQBAEBS6AIBAQPQA"), "Burn"),
                Deck.NewFromDeckList(
                    LoRDeckEncoder.GetDeckFromCode("CEBQEAIBAQQAEAQBA4FAMAQGCIOSWLJOHUBACAQGCQBQCAIFDUPACAQCAYMBS"), "Jsp"),
                Deck.NewFromDeckList(
                    LoRDeckEncoder.GetDeckFromCode("CEBACAIEAEFAEBQJBMIBEGRBFMWS4MIBAMBAMBA5EYAQCAQGBA"), "Jsp"),
                Deck.NewFromDeckList(
                    LoRDeckEncoder.GetDeckFromCode("CECAEAQCAMEQEAQAAMEQIAICAICCSOIEAEAA6GRBFIAQCAQAAEBACAICGEAQEAAF"), "Jsp"),
                Deck.NewFromDeckList(
                    LoRDeckEncoder.GetDeckFromCode("CECACAQEBABAEAQBBEBQCAQCBQ4QKAIECANSONBYAEBQCARFEYYQCAIBAQPQ"), "Jsp"),
                Deck.NewFromDeckList(
                    LoRDeckEncoder.GetDeckFromCode("CEBACAQABEFACAABBEFAWDA2DUVS2MYBAIAQAJJHAEBQEAABAUDQ"), "Bannermen")
            };
            Decks = toAdd;
            
            CardControlViewModels = new ObservableCollection<CompactCardControlViewModel>();
            ActiveDeck = toAdd.First();
        }


        // METHODS


        private void ChangeRegion(ERegion obj)
        {
            // TODO: Filter decks.
            RefreshCurrentSelectedDeck();
        }

        public void RefreshCurrentSelectedDeck()
        {
            // TODO: find which one is the actual active deck.
            CardControlViewModels.Clear();
            
            List<CompactCardControlViewModel> toAdd = new List<CompactCardControlViewModel>();
            foreach (CardCodeAndCount cardAndQuantity in ActiveDeck.Decklist)
            {
                Card card = ActiveDeck.Cards.First(c => c.Code == cardAndQuantity.CardCode);
                CompactCardControlViewModel cardControl = new CompactCardControlViewModel
                {
                    Card = card,
                    CardCodeAndCount = cardAndQuantity
                };
                toAdd.Add(cardControl);
            }
            
            toAdd = new List<CompactCardControlViewModel>(toAdd.OrderBy(c => c.Card.Cost));
            foreach (CompactCardControlViewModel compactCardControlView in toAdd)
            {
                CardControlViewModels.Add(compactCardControlView);
            }
        }
    }
}