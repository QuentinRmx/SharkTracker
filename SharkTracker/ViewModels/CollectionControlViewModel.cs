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
                UpdateStats();
            }
        }

        public List<Card> CurrentRegionCards => _collection
            .FindAll(c => c.Collectible && c.IsSelectedRegion(SelectedRegion))
            .OrderBy(c => c.Cost).ThenBy(c => c.Name).ToList();

        private ERegion _selectedRegion;

        public ERegion SelectedRegion
        {
            get => _selectedRegion;
            private set
            {
                _selectedRegion = value;
                RaisePropertyChanged(nameof(SelectedRegion));
                RaisePropertyChanged(nameof(SelectedRegionName));
            }
        }


        public string SelectedRegionName => SelectedRegion.ToLongName();

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

        private int _maxChampionsPerRegion;

        private int _nbChampions;

        public int NbChampions
        {
            get => _nbChampions;
            set
            {
                _nbChampions = value;
                RaisePropertyChanged(nameof(NbChampions));
                RaisePropertyChanged(nameof(TextNbChampions));
                RaisePropertyChanged(nameof(ProgressChampions));
                RaisePropertyChanged(nameof(TextShardsChampions));

                RaisePropertyChanged(nameof(TextNbTotal));
            }
        }

        public string TextNbChampions => $"{NbChampions}+({NbWildcardsChampions})/{_maxChampionsPerRegion}";

        public string ProgressChampions => $"{(NbChampions * 100) / _maxChampionsPerRegion}%";

        private int _shardsChampions => (_maxChampionsPerRegion * 3000) - (NbChampions * 3000);

        public string TextShardsChampions => $"{_shardsChampions}";

        private int _maxEpicsPerRegion;

        private int _nbEpics;

        public int NbEpics
        {
            get => _nbEpics;
            set
            {
                _nbEpics = value;
                RaisePropertyChanged(nameof(NbEpics));
                RaisePropertyChanged(nameof(TextNbEpics));
                RaisePropertyChanged(nameof(ProgressEpics));
                RaisePropertyChanged(nameof(TextShardsEpics));

                RaisePropertyChanged(nameof(TextNbTotal));
            }
        }

        public string TextNbEpics => $"{NbEpics}/{_maxEpicsPerRegion}";

        public string ProgressEpics => $"{(NbEpics * 100) / _maxEpicsPerRegion}%";

        private int _shardsEpics => (_maxEpicsPerRegion * 1200) - (NbEpics * 1200);

        public string TextShardsEpics => $"{_shardsEpics}";

        private int _maxRaresPerRegion;

        private int _nbRares;

        public int NbRares
        {
            get => _nbRares;
            set
            {
                _nbRares = value;
                RaisePropertyChanged(nameof(NbRares));
                RaisePropertyChanged(nameof(TextNbRares));
                RaisePropertyChanged(nameof(ProgressRares));
                RaisePropertyChanged(nameof(TextShardsRares));

                RaisePropertyChanged(nameof(TextNbTotal));
            }
        }

        public string TextNbRares => $"{NbRares}/{_maxRaresPerRegion}";

        public string ProgressRares => $"{(NbRares * 100) / _maxRaresPerRegion}%";

        private int _shardsRares => (_maxRaresPerRegion * 300) - (NbRares * 300);

        public string TextShardsRares => $"{_shardsRares}";

        private int _maxCommonsPerRegion;

        private int _nbCommons;

        public int NbCommons
        {
            get => _nbCommons;
            set
            {
                _nbCommons = value;
                RaisePropertyChanged(nameof(NbCommons));
                RaisePropertyChanged(nameof(TextNbCommons));
                RaisePropertyChanged(nameof(ProgressCommons));
                RaisePropertyChanged(nameof(TextShardsCommons));

                RaisePropertyChanged(nameof(TextNbTotal));
                RaisePropertyChanged(nameof(ProgressTotal));
                RaisePropertyChanged(nameof(ShardsTotal));
            }
        }

        public string TextNbCommons => $"{NbCommons}/{_maxCommonsPerRegion}";

        public string ProgressCommons => $"{(NbCommons * 100) / _maxCommonsPerRegion}%";

        private int _shardsCommons => (_maxCommonsPerRegion * 300) - (NbCommons * 300);

        public string TextShardsCommons => $"{_shardsCommons}";

        public string TextNbTotal => $"{NbChampions + NbEpics + NbRares + NbCommons}/{CurrentRegionCards.Count * 3}";

        public string ProgressTotal =>
            $"{((NbChampions + NbEpics + NbRares + NbCommons) * 100) / (CurrentRegionCards.Count * 3)}%";

        public string ShardsTotal =>
            $"{_shardsChampions + _shardsEpics + _shardsRares + _shardsCommons}";
        
        // User resources

        private int _nbWildcardsChampions;

        public int NbWildcardsChampions
        {
            get => _nbWildcardsChampions;
            set
            {
                _nbWildcardsChampions = value;
                RaisePropertyChanged(nameof(NbWildcardsChampions));
                RaisePropertyChanged(nameof(TextNbChampions));
            }
        }


        // CONSTRUCTORS

        public CollectionControlViewModel()
        {
            SelectedRegion = ERegion.ALL;
            CardsManager.Instance.Register(this);
            UpdateCollection();
            PrepareViewModels();
            UpdateStats();
        }


        // METHODS


        /// <inheritdoc />
        public void Notify()
        {
            UpdateCollection();
            UpdateStats();
            PrepareViewModels();
        }

        private void UpdateCollection()
        {
            _collection = CardsManager.Instance.GetAllCards();
            UpdateStats();
        }

        private void SaveCollection()
        {
            CardsManager.Instance.SaveUserCollection(_collection);
            UpdateStats();
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
                UpdateCollection();
            }

            CardVms.Clear();
            CurrentRegionCards.ToList().ForEach(async c => await c.LoadArtwork());
            List<CardCollectionControlViewModel> toAdd = CurrentRegionCards
                .Select(card => new CardCollectionControlViewModel {Card = card}).ToList();
            foreach (CardCollectionControlViewModel model in toAdd)
            {
                model.Register(this);
                CardVms.Add(model);
            }

            ResetScrollbar = true;
        }

        private void ChangeRegion(ERegion newRegion)
        {
            SaveCollection();
            ERegion toClear = SelectedRegion;
            SelectedRegion = newRegion;
            UpdateStats();
            PrepareViewModels();
            _collection.Where(c => c.RegionEnum == toClear).ToList().ForEach(c => c.ClearArtwork());
        }

        private void UpdateStats()
        {
            _maxChampionsPerRegion =
                CurrentRegionCards.Count(c => c.RarityEnum == ERarity.Champion) * 3;
            NbChampions = CurrentRegionCards.Where(c => c.RarityEnum == ERarity.Champion).Select(c => c.QuantityOwned)
                .Sum();
            RaisePropertyChanged(nameof(ProgressChampions));
            RaisePropertyChanged(nameof(TextNbChampions));
            RaisePropertyChanged(nameof(TextShardsChampions));

            _maxEpicsPerRegion =
                CurrentRegionCards.Count(c => c.RarityEnum == ERarity.Epic) * 3;
            NbEpics = CurrentRegionCards.Where(c => c.RarityEnum == ERarity.Epic).Select(c => c.QuantityOwned)
                .Sum();
            RaisePropertyChanged(nameof(TextNbEpics));
            RaisePropertyChanged(nameof(ProgressEpics));
            RaisePropertyChanged(nameof(TextShardsEpics));

            _maxRaresPerRegion =
                CurrentRegionCards.Count(c => c.RarityEnum == ERarity.Rare) * 3;
            NbRares = CurrentRegionCards.Where(c => c.RarityEnum == ERarity.Rare).Select(c => c.QuantityOwned)
                .Sum();
            RaisePropertyChanged(nameof(TextNbRares));
            RaisePropertyChanged(nameof(ProgressRares));
            RaisePropertyChanged(nameof(TextShardsRares));

            _maxCommonsPerRegion =
                CurrentRegionCards.Count(c => c.RarityEnum == ERarity.Common) * 3;
            NbCommons = CurrentRegionCards.Where(c => c.RarityEnum == ERarity.Common).Select(c => c.QuantityOwned)
                .Sum();
            RaisePropertyChanged(nameof(TextNbCommons));
            RaisePropertyChanged(nameof(ProgressCommons));
            RaisePropertyChanged(nameof(TextShardsCommons));

            RaisePropertyChanged(nameof(TextNbTotal));
            RaisePropertyChanged(nameof(ProgressTotal));
            RaisePropertyChanged(nameof(ShardsTotal));
        }
    }
}