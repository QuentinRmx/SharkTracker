using System;
using System.Collections.Generic;
using System.Net.Http;
using SharkTrackerCore.LoRDeckCodes;

namespace SharkTracker.Communicator
{
    public abstract class AbstractLorCommunicator
    {

        protected HttpClient _client;

        protected bool _isActive = true;
        public bool IsActive => _isActive;

        public event EventHandler OnStarting;

        public event EventHandler OnStopping;

        protected AbstractLorCommunicator(HttpClient client)
        {
            _client = client;
        }
        
        public abstract List<CardCodeAndCount> GetActiveDeck();

        public void Start()
        {
            _isActive = true;
            OnStarting?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            _isActive = false;
            OnStopping?.Invoke(this, EventArgs.Empty);
        }
    }
}