using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharkTracker.Models;
using SharkTracker.Observation;

namespace SharkTracker.Utils
{
    public class CardsManager : AbstractObservable
    {
        // ATTRIBUTES

        private List<Card> _cards;

        private Dictionary<string, int> _userCollection;

        private static CardsManager _instance;


        public static CardsManager Instance
        {
            get
            {
                _instance ??= new CardsManager();
                return _instance;
            }
        }

        // CONSTRUCTORS

        private CardsManager()
        {
            _cards = new List<Card>();
            _userCollection = new Dictionary<string, int>();
            LoadUserCollection();
        }

        // METHODS

        public async Task<bool> LoadAllCards()
        {
            _cards = new List<Card>();

            // SET 1
            string json = await File.ReadAllTextAsync(Constants.PATH_CARD_SET_1);
            _cards.AddRange(JsonConvert.DeserializeObject<List<Card>>(json));
            
            _observers.ForEach(o => o.Notify());
            foreach (Card c in _cards)
            {
                try
                {
                    _userCollection.TryGetValue(c.Code, out int value);
                    c.QuantityOwned = value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            // SET 2
            json = await File.ReadAllTextAsync(Constants.PATH_CARD_SET_2);
            _cards.AddRange(JsonConvert.DeserializeObject<List<Card>>(json));
            
            _observers.ForEach(o => o.Notify());
            foreach (Card c in _cards)
            {
                _userCollection.TryGetValue(c.Code, out int value);
                c.QuantityOwned = value;
            }
            return true;
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
                _userCollection = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
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

            _cards.First(ca => ca.Code == c.Code).QuantityOwned = c.QuantityOwned;
        }

        
    }
}