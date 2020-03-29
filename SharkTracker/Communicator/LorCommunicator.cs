using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LoRDeckCodes;
using Newtonsoft.Json;
using SharkTracker.LoRDeckCodes;

namespace SharkTracker.Communicator
{
    public class LorCommunicator : AbstractLorCommunicator
    {

        // ATTRIBUTES

        // CONSTRUCTORS
        
        /// <inheritdoc />
        public LorCommunicator(HttpClient client) : base(client)
        {
        }

        // METHODS

        
        public override List<CardCodeAndCount> GetActiveDeck()
        {
            if (_client == null)
                _client = new HttpClient();
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            Task<HttpResponseMessage> resp = _client.GetAsync("http://127.0.0.1:21337/static-decklist");
            if (resp.Result.IsSuccessStatusCode)
            {
                string respBody = resp.Result.Content.ReadAsStringAsync().Result;
                DeckResponse deckResponse = JsonConvert.DeserializeObject<DeckResponse>(respBody);
                if (deckResponse != null)
                {
                    deck = LoRDeckEncoder.GetDeckFromCode(deckResponse.DeckCode);
                }
            }
            return deck;
        }

        
    }
}