using System.Collections.Generic;
using Newtonsoft.Json;

namespace SharkTracker.Communicator
{
    public class DeckResponse
    {

        // ATTRIBUTES

        [JsonProperty("DeckCode")]
        public string DeckCode { get; set; }
        
        [JsonProperty("CardsInDeck")]
        public Dictionary<string, int> CardsInDeck { get; set; }

        // CONSTRUCTORS

        // METHODS


    }
}