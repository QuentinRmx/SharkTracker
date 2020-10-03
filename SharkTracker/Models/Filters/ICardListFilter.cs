using System.Collections.Generic;

namespace SharkTracker.Models.Filters
{
    public interface ICardListFilter
    {
        
        List<Card> Apply(List<Card> original);
    }
}