using System.Collections.Generic;

namespace SharkTracker.Observation
{
    public interface Observable
    {
        void Register(Observer o);

        void Unregister(Observer o);
    }
}