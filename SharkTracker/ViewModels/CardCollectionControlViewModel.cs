using System.Collections.Generic;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using SharkTrackerCore.Models;
using SharkTrackerCore.Observation;

namespace SharkTracker.ViewModels
{
    public class CardCollectionControlViewModel : ViewModelBase, IObservable
    {

        // ATTRIBUTES
        
        protected readonly List<Observer> _observers = new List<Observer>();
        
        private Card _card;

        public Card Card
        {
            get => _card;
            set
            {
                _card = value;
                RaisePropertyChanged(nameof(Card));
                NotifyAll();
            }
        }
        

        // CONSTRUCTORS

        // METHODS
        
        /// <inheritdoc />
        public void Register(Observer o)
        {
            if (!_observers.Contains(o))
            {
                _observers.Add(o);
                _card.PropertyChanged += NotifyAll;
            }
        }

        private void NotifyAll(object sender, PropertyChangedEventArgs e)
        {
            NotifyAll();
        }

        /// <inheritdoc />
        public void Unregister(Observer o)
        {
            if (_observers.Contains(o))
            {
                _observers.Remove(o);
            }
        }

        public void NotifyAll()
        {
            _observers.ForEach(o => o.Notify());
        }

    }
}