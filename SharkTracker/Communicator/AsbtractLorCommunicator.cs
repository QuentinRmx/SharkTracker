using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SharkTracker.LoRDeckCodes;

namespace SharkTracker.Communicator
{
    public abstract class AbstractLorCommunicator
    {

        protected HttpClient _client;

        protected AbstractLorCommunicator(HttpClient client)
        {
            _client = client;
        }
        
        public abstract List<CardCodeAndCount> GetActiveDeck();
    }
}