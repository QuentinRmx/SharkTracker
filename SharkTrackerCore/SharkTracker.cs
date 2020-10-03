using System;
using System.Threading.Tasks;

namespace SharkTrackerCore
{
    public class SharkTracker
    {

        // ATTRIBUTES

        private readonly DownloadManager _downloadManager;

        // CONSTRUCTORS

        private SharkTracker()
        {
            _downloadManager = new DownloadManager();
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
                _downloadManager.OnProgress += handler;
                await _downloadManager.DownloadArtworks(new[] {"01DE002", "01DE003", "01DE004", "01DE006"});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }
}