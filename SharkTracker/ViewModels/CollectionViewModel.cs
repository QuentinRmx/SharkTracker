using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharkTracker.Models;
using SharkTracker.Utils;

namespace SharkTracker.ViewModels
{
    public class CollectionViewModel : ViewModelBase
    {
        // ATTRIBUTES

        private List<Card> _playerCollection;

        public List<Card> PlayerCollection
        {
            get => _playerCollection;
            set
            {
                _playerCollection = value;
                RaisePropertyChanged(nameof(PlayerCollection));
            }
        }

        public IEnumerable<Card> DemaciaCards =>
            _playerCollection.FindAll(c => c.Collectible && c.Region == "Demacia").OrderBy(c => c.RarityEnum)
                .ThenBy(c => c.Cost).ThenBy(c => c.Name).ToList();

        public IEnumerable<Card> IoniaCards =>
            _playerCollection.FindAll(c => c.Collectible && c.Region == "Ionia").OrderBy(c => c.RarityEnum)
                .ThenBy(c => c.Cost).ThenBy(c => c.Name).ToList();

        public IEnumerable<Card> FreljordCards =>
            _playerCollection.FindAll(c => c.Collectible && c.Region == "Freljord").OrderBy(c => c.RarityEnum)
                .ThenBy(c => c.Cost).ThenBy(c => c.Name).ToList().ToList();

        public IEnumerable<Card> NoxusCards =>
            _playerCollection.FindAll(c => c.Collectible && c.Region == "Noxus").OrderBy(c => c.RarityEnum)
                .ThenBy(c => c.Cost).ThenBy(c => c.Name).ToList();

        public IEnumerable<Card> PnzCards =>
            _playerCollection.FindAll(c => c.Collectible && c.Region == "Piltover & Zaun").OrderBy(c => c.RarityEnum)
                .ThenBy(c => c.Cost).ThenBy(c => c.Name).ToList();

        public IEnumerable<Card> SiCards =>
            _playerCollection.FindAll(c => c.Collectible && c.Region == "Shadow Isles").OrderBy(c => c.RarityEnum)
                .ThenBy(c => c.Cost).ThenBy(c => c.Name).ToList();

        public ICommand CommandSave => new RelayCommand(SaveCollection);


        // CONSTRUCTORS
        public CollectionViewModel()
        {
            PlayerCollection = new List<Card>(CardsManager.getAllCards());
        }


        // METHODS

        private void SaveCollection()
        {
            CardsManager.saveAllCards(PlayerCollection);
        }
    }
}