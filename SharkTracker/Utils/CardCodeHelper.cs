using System.Collections.Generic;
using System.Linq;
using SharkTracker.LoRDeckCodes;
using SharkTracker.Managers;
using SharkTracker.Models;

namespace SharkTracker.Utils
{
    public class CardCodeHelper
    {

        // ATTRIBUTES

        // CONSTRUCTORS

        // METHODS

        public static List<Card> CardsFromCodes(IEnumerable<CardCodeAndCount> codes)
        {
            List<Card> cards = CardsManager.Instance.GetAllCards();

            return codes.Select(code => cards.First(c => c.Code == code.CardCode)).ToList();
        }

    }
}