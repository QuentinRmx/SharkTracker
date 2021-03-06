namespace SharkTrackerCore.Observation
{
    public interface IObservable
    {
        void Register(Observer o);

        void Unregister(Observer o);

        void NotifyAll();
    }
}