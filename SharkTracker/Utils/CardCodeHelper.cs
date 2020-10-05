using System.Collections.Generic;
using System.Linq;
using SharkTrackerCore.LoRDeckCodes;
using SharkTrackerCore.Models;

namespace SharkTracker.Utils
{
    public static class CardCodeHelper
    {

        // ATTRIBUTES

        // CONSTRUCTORS

        // METHODS

        public static List<Card> CardsFromCodes(IEnumerable<CardCodeAndCount> codes)
        {
            List<Card> cards = App.SharkTracker.GetAllCards();

            return codes.Select(code => cards.First(c => c.Code == code.CardCode)).ToList();
        }

    }
}