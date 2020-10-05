#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SharkTrackerCore.Managers;
using SharkTrackerCore.Models;
using SharkTrackerCore.Observation;

namespace SharkTrackerCore
{
    /// <summary>
    /// Main class and entry point of the API.
    /// This class is basically the main way from outside to perform actions such as tracking the active deck, updating
    /// the data from Riot's servers or managing the player's collection.
    /// </summary>
    public class SharkTracker
    {

        internal static readonly HttpClient HttpClient;

        static SharkTracker()
        {
            HttpClient = new HttpClient();
        }

        
        // ATTRIBUTES

        /// <summary>
        /// Private instance of RiotDownloader. Responsible of downloading json files and artworks.
        /// Also keep track of the current progress of the download.
        /// </summary>
        private readonly RiotDownloader _riotDownloader;

        private readonly CardsManager _cardsManager;

        // CONSTRUCTORS

        /// <summary>
        /// Private constructor.
        /// </summary>
        private SharkTracker()
        {
            // TODO: Dependency injections.
            _riotDownloader = new RiotDownloader();
            _cardsManager = new CardsManager();
        }

        // METHODS

        /// <summary>
        /// API's entry point. Calling this method will return a new instance of SharkTracker ready to be used.
        /// The new instance is not initialized though, meaning that no data is preloaded to ensure that we have the
        /// fastest starting time possible.
        /// </summary>
        /// <returns>The ready-to-use SharkTracker instance.</returns>
        public static SharkTracker New()
        {
            SharkTracker tracker = new SharkTracker();
            return tracker;
        }

        /// <summary>
        /// This method will fetch all necessary data from the official Riot Games' servers.
        /// It:
        ///  - retrieves all json files containing the sets' infos
        ///  - for each card, if not already downloaded, download its artwork and save it locally.
        /// </summary>
        /// <param name="handler">EventHandler to monitor the progress of the task</param>
        /// <param name="overwrite">If true then data will be overwritten if already there.</param>
        public async Task UpdateFromRiot(EventHandler handler, bool overwrite = false)
        {
            try
            {
                _riotDownloader.OnProgress += handler;
                // await _riotDownloader.DownloadArtworks(new[] {"01DE002", "01DE003", "01DE004", "01DE006"});
                List<Card> allCards = await _riotDownloader.DownloadAllSets(HttpClient);
                _cardsManager.SetAllCards(allCards);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public List<Card> GetAllCards()
        {
            return _cardsManager.GetAllCards();
        }

        public void SaveUserCollection(List<Card> userCollection)
        {
            _cardsManager.SaveUserCollection(userCollection);
        }

        public void RegisterObserverToCardManager(Observer observer)
        {
            _cardsManager.Register(observer);
        }
    }
}