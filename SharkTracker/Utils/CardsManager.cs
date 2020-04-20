using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SharkTracker.Models;

namespace SharkTracker.Utils
{
    public static class CardsManager
    {
        // ATTRIBUTES

        // CONSTRUCTORS

        // METHODS

        private static List<Card> _cards;


        public static List<Card> getAllCards()
        {
            if (_cards == null)
            {
                _cards = new List<Card>();
                if (!File.Exists(Constants.PATH_COLLECTION))
                {
                    using FileStream tmp = File.Create(Constants.PATH_COLLECTION);
                    tmp.Close();
                    File.WriteAllText(Constants.PATH_COLLECTION, File.ReadAllText(Constants.PATH_CARD_SET_1));
                }

                string json = File.ReadAllText(Constants.PATH_COLLECTION);
                _cards.AddRange(JsonConvert.DeserializeObject<List<Card>>(json));
            }

            return _cards;
        }

        public static void saveAllCards(List<Card> cards)
        {
            string json = JsonConvert.SerializeObject(cards);
            using StreamWriter writer = new StreamWriter(Constants.PATH_COLLECTION);
            writer.Write(json);
            writer.Close();
        }
    }
}