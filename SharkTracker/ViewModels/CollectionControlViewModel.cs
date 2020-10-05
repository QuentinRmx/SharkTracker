using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharkTracker.Models.Filters;
using SharkTrackerCore.Models;
using SharkTrackerCore.Observation;
using ERegion = SharkTrackerCore.Models.ERegion;

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

        public List<Card> CurrentRegionCards => GetCurrentRegionCards();

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

        public string TextNbChampions => $"{NbChampions}/{_maxChampionsPerRegion}";

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

        public string TextNbTotal =>
            $"{NbChampions + NbEpics + NbRares + NbCommons}/{GetCurrentRegionCards(false).Count * 3}";

        public string ProgressTotal =>
            $"{((NbChampions + NbEpics + NbRares + NbCommons) * 100) / (GetCurrentRegionCards(false).Count * 3)}%";

        public string ShardsTotal =>
            $"{_shardsChampions + _shardsEpics + _shardsRares + _shardsCommons}";

        // User resources

        public int NbWildcardsChampions
        {
            get => UserResources.ChampionWcs;
            set
            {
                UserResources.ChampionWcs = value;
                RaisePropertyChanged(nameof(NbWildcardsChampions));
                RaisePropertyChanged(nameof(TextNbChampions));
            }
        }

        public int NbWildcardsEpics
        {
            get => UserResources.EpicWcs;
            set
            {
                UserResources.EpicWcs = value;
                RaisePropertyChanged(nameof(NbWildcardsEpics));
                RaisePropertyChanged(nameof(TextNbEpics));
            }
        }

        private UserResources _userResources;

        public UserResources UserResources
        {
            get => _userResources;
            set
            {
                _userResources = value;
                RaisePropertyChanged(nameof(UserResources));
                RaisePropertyChanged(nameof(NbWildcardsChampions));
                RaisePropertyChanged(nameof(TextNbChampions));
            }
        }

        public static IEnumerable<ECollectionFilter> FilterTypes => new[]
        {
            ECollectionFilter.ALL, ECollectionFilter.Champions, ECollectionFilter.Epics, ECollectionFilter.Rares,
            ECollectionFilter.Commons, ECollectionFilter.Incomplete
        };

        private ECollectionFilter _selectedFilter = ECollectionFilter.ALL;

        public ECollectionFilter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                RaisePropertyChanged(nameof(SelectedFilter));
                PrepareViewModels();
            }
        }


        // CONSTRUCTORS

        public CollectionControlViewModel()
        {
            SelectedRegion = ERegion.ALL;
            // TODO: Fix registering.
            App.SharkTracker.RegisterObserverToCardManager(this);
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

        /// <summary>
        /// If the parameter withFilter is false, all the cards of the selected region will be returned.
        /// Otherwise, the additional filters selected by the user will be applied before returning the list.
        /// </summary>
        /// <param name="withFilter">Indicates if the additional user filters must be applied.</param>
        /// <returns>The cards belonging to the current region, filtered or not.</returns>
        private List<Card> GetCurrentRegionCards(bool withFilter = true)
        {
            // TODO: Filters abstraction
            List<Card> currentCards = _collection
                .FindAll(c => c.Collectible && c.IsSelectedRegion(SelectedRegion))
                .OrderBy(c => c.Cost).ThenBy(c => c.Name).ToList();
            // If the filters are disabled we return now so we don't apply any.
            if (!withFilter) return currentCards;
            // Otherwise we apply the filters.
            switch (SelectedFilter)
            {
                case ECollectionFilter.Champions:
                    currentCards = currentCards.Where(c => c.RarityEnum == ERarity.Champion).ToList();
                    break;
                case ECollectionFilter.Epics:
                    currentCards = currentCards.Where(c => c.RarityEnum == ERarity.Epic).ToList();
                    break;
                case ECollectionFilter.Rares:
                    currentCards = currentCards.Where(c => c.RarityEnum == ERarity.Rare).ToList();
                    break;
                case ECollectionFilter.Commons:
                    currentCards = currentCards.Where(c => c.RarityEnum == ERarity.Common).ToList();
                    break;
                case ECollectionFilter.Incomplete:
                    currentCards = currentCards.Where(c => c.QuantityOwned != 3).ToList();
                    break;
                case ECollectionFilter.ALL:
                    break;
                default:
                    break;
            }

            return currentCards;
        }

        private void UpdateCollection()
        {
            _collection = App.SharkTracker.GetAllCards();
            // UserResources = App.SharkTracker.GetUserResources();
            UpdateStats();
        }

        private void SaveCollection()
        {
            //TODO: Fix save.
            App.SharkTracker.SaveUserCollection(_collection);
            // CardsManager.Instance.SaveUserResources(UserResources);
            UpdateStats();
        }

        /// <summary>
        /// Saves the collection changes and cleanup everything to release as much memory as possible.
        /// </summary>
        private void OnExit()
        {
            SaveCollection();
            // Clear artwork and delete the list.
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
        }

        private void UpdateStats()
        {
            List<Card> currentCards = GetCurrentRegionCards(false);
            _maxChampionsPerRegion =
                currentCards.Count(c => c.RarityEnum == ERarity.Champion) * 3;
            NbChampions = currentCards.Where(c => c.RarityEnum == ERarity.Champion).Select(c => c.QuantityOwned)
                .Sum();
            RaisePropertyChanged(nameof(ProgressChampions));
            RaisePropertyChanged(nameof(TextNbChampions));
            RaisePropertyChanged(nameof(TextShardsChampions));

            _maxEpicsPerRegion =
                currentCards.Count(c => c.RarityEnum == ERarity.Epic) * 3;
            NbEpics = currentCards.Where(c => c.RarityEnum == ERarity.Epic).Select(c => c.QuantityOwned)
                .Sum();
            RaisePropertyChanged(nameof(TextNbEpics));
            RaisePropertyChanged(nameof(ProgressEpics));
            RaisePropertyChanged(nameof(TextShardsEpics));

            _maxRaresPerRegion =
                currentCards.Count(c => c.RarityEnum == ERarity.Rare) * 3;
            NbRares = currentCards.Where(c => c.RarityEnum == ERarity.Rare).Select(c => c.QuantityOwned)
                .Sum();
            RaisePropertyChanged(nameof(TextNbRares));
            RaisePropertyChanged(nameof(ProgressRares));
            RaisePropertyChanged(nameof(TextShardsRares));

            _maxCommonsPerRegion =
                currentCards.Count(c => c.RarityEnum == ERarity.Common) * 3;
            NbCommons = currentCards.Where(c => c.RarityEnum == ERarity.Common).Select(c => c.QuantityOwned)
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