using System.Collections.Generic;
using System.Net.Http;
using SharkTracker.LoRDeckCodes;

namespace SharkTracker.Communicator
{
    public abstract class AbstractLorCommunicator
    {

        protected HttpClient _client;

        protected bool _isActive = true;

        protected AbstractLorCommunicator(HttpClient client)
        {
            _client = client;
        }
        
        public abstract List<CardCodeAndCount> GetActiveDeck();

        public void Start()
        {
            _isActive = true;
        }

        public void Stop()
        {
            _isActive = false;
        }
    }
}