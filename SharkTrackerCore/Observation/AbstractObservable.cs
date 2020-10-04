using System.Collections.Generic;

namespace SharkTrackerCore.Observation
{
    public abstract class AbstractObservable : IObservable
    {

        // ATTRIBUTES

        protected readonly List<Observer> _observers = new List<Observer>();

        // CONSTRUCTORS

        // METHODS

        /// <inheritdoc />
        public void Register(Observer o)
        {
            if (!_observers.Contains(o))
            {
                _observers.Add(o);
            }
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