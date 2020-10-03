using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SharkTracker.LoRDeckCodes;
using SharkTracker.Utils;

namespace SharkTracker.Models
{
    public class Deck
    {
        // ATTRIBUTES
        [JsonProperty("decklist")] public List<CardCodeAndCount> Decklist { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("regions")] public List<ERegion> Regions { get; set; }

        [Newtonsoft.Json.JsonIgnore] public List<Card> Cards { get; set; }

        // CONSTRUCTORS

        private Deck()
        {
        }

        // METHODS


        public static Deck NewFromDeckList(List<CardCodeAndCount> decklist, string name = "")
        {
            List<Card> cards = CardCodeHelper.CardsFromCodes(decklist);
            Deck deck = new Deck
            {
                Decklist = decklist,
                Name = name,
                Cards = cards,
                Regions = cards.Select(c => c.RegionEnum).Distinct().ToList()
            };
            
            
            return deck;
        }
    }
}