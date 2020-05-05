using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
            List<CardCodeAndCount> deck = new List<CardCodeAndCount>();
            try
            {
                _client ??= new HttpClient();

                Task<HttpResponseMessage> resp = _client.GetAsync("http://127.0.0.1:21337/static-decklist");
                if (resp.Result.IsSuccessStatusCode)
                {
                    string respBody = resp.Result.Content.ReadAsStringAsync().Result;
                    DeckResponse deckResponse = JsonConvert.DeserializeObject<DeckResponse>(respBody);
                    if (deckResponse != null && deckResponse.DeckCode != string.Empty)
                    {
                        deck = LoRDeckEncoder.GetDeckFromCode(deckResponse.DeckCode);
                    }
                }

            }
            catch (AggregateException ex)
            {
                //TODO: do something about that lol.
                Console.WriteLine(ex);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error here: {ex}");
            }
            return deck;
        }

        
    }
}