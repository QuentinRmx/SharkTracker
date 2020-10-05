using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SharkTrackerCore.Models;
using SharkTrackerCore.Observation;

namespace SharkTrackerCore.Managers
{
    internal class CardsManager : AbstractObservable
    {
        // ATTRIBUTES

        private List<Card> _cards;

        public int CardCount => _cards?.Count ?? 0;

        private Dictionary<string, int> _userCollection;

        private UserResources _userResources;


        // CONSTRUCTORS

        internal CardsManager()
        {
            // CheckLocalFiles();
            _cards = new List<Card>();
            _userCollection = new Dictionary<string, int>();
            LoadUserCollection();
            LoadUserResources();
        }

        // private static void CheckLocalFiles()
        // {
        //     if (!File.Exists(Constants.PATH_COLLECTION))
        //     {
        //         File.Create(Constants.PATH_COLLECTION);
        //     }
        //     
        //     if (!File.Exists(Constants.PATH_USER_RESOURCES))
        //     {
        //         File.Create(Constants.PATH_USER_RESOURCES);
        //     }
        // }

        // METHODS

        public void SetAllCards(List<Card> cards)
        {
            _cards = cards;
            foreach (Card ca in _cards)
            {
                _userCollection.TryGetValue(ca.Code, out int qty);
                ca.QuantityOwned = qty;
            }
        }

        private void LoadUserCollection()
        {
            if (!File.Exists(Constants.PATH_COLLECTION))
            {
                _userCollection = new Dictionary<string, int>();
                using FileStream tmp = File.Create(Constants.PATH_COLLECTION);
                tmp.Close();
                return;
            }

            try
            {
                string json = File.ReadAllText(Constants.PATH_COLLECTION);
                _userCollection = JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ??
                                  new Dictionary<string, int>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<Card> GetAllCards()
        {
            return _cards;
        }

        public void SaveUserCollection(List<Card> cards)
        {
            foreach (Card c in cards)
            {
                UpdateUserCollection(c);
            }

            string json = JsonConvert.SerializeObject(_userCollection);
            using StreamWriter writer = new StreamWriter(Constants.PATH_COLLECTION);
            writer.Write(json);
            writer.Close();
        }

        private void UpdateUserCollection(Card c)
        {
            if (_userCollection.ContainsKey(c.Code))
            {
                _userCollection[c.Code] = c.QuantityOwned;
            }
            else
            {
                _userCollection.Add(c.Code, c.QuantityOwned);
            }

            foreach (Card ca in _cards)
            {
                if (ca.Code == c.Code)
                    ca.QuantityOwned = c.QuantityOwned;
                break;
            }
        }

        private void LoadUserResources()
        {
            if (!File.Exists(Constants.PATH_USER_RESOURCES))
            {
                using FileStream tmp = File.Create(Constants.PATH_USER_RESOURCES);
                tmp.Close();
                SaveUserResources(new UserResources());
                return;
            }

            string json = File.ReadAllText(Constants.PATH_USER_RESOURCES);
            _userResources = JsonConvert.DeserializeObject<UserResources>(json);
            if (_userResources == null)
            {
                _userResources = new UserResources();
                SaveUserResources(_userResources);
            }
        }

        public void SaveUserResources(UserResources userResources)
        {
            _userResources = userResources;
            string json = JsonConvert.SerializeObject(userResources);
            using StreamWriter writer = new StreamWriter(Constants.PATH_USER_RESOURCES);
            writer.Write(json);
            writer.Close();
        }

        public UserResources GetUserResources()
        {
            if (_userResources == null)
            {
                LoadUserResources();
            }

            return _userResources;
        }
    }
}