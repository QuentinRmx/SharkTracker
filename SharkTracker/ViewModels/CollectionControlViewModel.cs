using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharkTracker.Models;
using SharkTracker.Observation;
using SharkTracker.Utils;

namespace SharkTracker.ViewModels
{
    public class CollectionControlViewModel : ViewModelBase, Observer
    {
        // ATTRIBUTES

        public ICommand WindowClosing => new RelayCommand(OnExit);
        
        public ICommand ChangeRegionCommand => new RelayCommand<ERegion>(ChangeRegion);

        private List<Card> _collection = new List<Card>();

        private BindingList<CardCollectionControlViewModel> _cardVms =
            new BindingList<CardCollectionControlViewModel>();

        public BindingList<CardCollectionControlViewModel> CardVms
        {
            get => _cardVms;
            set
            {
                _cardVms = value;
                RaisePropertyChanged(nameof(CardVms));
            }
        }
        
        public List<Card> CurrentRegionCards =>
            _collection.FindAll(c => c.Collectible && c.RegionEnum == SelectedRegion)
                .OrderBy(c => c.Cost).ThenBy(c => c.Name).ToList();
        
        public ERegion SelectedRegion { get; private set; } = ERegion.Bilgewater;


        private bool _resetScrollbar;

        public bool ResetScrollbar
        {
            get => _resetScrollbar;
            set
            {
                _resetScrollbar = value;
                RaisePropertyChanged(nameof(ResetScrollbar));
            }
        }

        // CONSTRUCTORS

        public CollectionControlViewModel()
        {
            CardsManager.Instance.Register(this);
            PrepareViewModels();
        }


        // METHODS


        /// <inheritdoc />
        public void Notify()
        {
            _collection = CardsManager.Instance.GetAllCards();
            PrepareViewModels();
        }
        
        private void SaveCollection()
        {
            CardsManager.Instance.SaveUserCollection(_collection);
        }

        /// <summary>
        /// Saves the collection changes and cleanup everything to release as much memory as possible.
        /// </summary>
        private void OnExit()
        {
            SaveCollection();
            // Clear artwork and delete the list.
            _collection.ForEach(c => c.ClearArtwork());
            _collection = null;
            _cardVms = null;
        }
        
        private void PrepareViewModels()
        {
            if (_collection.Count == 0)
            {
                _collection = new List<Card>(CardsManager.Instance.GetAllCards());
            }
            
            CardVms.Clear();
            CurrentRegionCards.ToList().ForEach(async c => await c.LoadArtwork());
            List<CardCollectionControlViewModel> toAdd = CurrentRegionCards
                .Select(card => new CardCollectionControlViewModel {Card = card}).ToList();
            // CardVms = new BindingList<CardCollectionControlViewModel>(toAdd);
            foreach (CardCollectionControlViewModel model in toAdd)
            {
                CardVms.Add(model);
            }

            ResetScrollbar = true;
        }
        
        private void ChangeRegion(ERegion newRegion)
        {
            ERegion toClear = SelectedRegion;
            SelectedRegion = newRegion;
            PrepareViewModels();
            _collection.Where(c => c.RegionEnum == toClear).ToList().ForEach(c => c.ClearArtwork());
        }
    }
}