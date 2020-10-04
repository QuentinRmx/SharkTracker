using System.Collections.Generic;
using SharkTrackerCore.Models;

namespace SharkTracker.Models.Filters
{
    public interface ICardListFilter
    {
        
        List<Card> Apply(List<Card> original);
    }
}