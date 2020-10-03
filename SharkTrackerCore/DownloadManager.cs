using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharkTrackerCore
{
    public class DownloadManager
    {
        // EVENTS

        public delegate void ProgressEventHandler(object sender, EventArgs args);

        public event EventHandler OnProgress;

        // ATTRIBUTES

        /// <summary>
        /// Gives the progress (%) for the current task.
        /// </summary>
        public double ProgressPercent => (Progress * 100) / _totalProgress;

        private double _progress;

        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnProgress?.Invoke(this, EventArgs.Empty);
            }
        }

        private double _totalProgress;

        public double TotalProgress => _totalProgress;

        // CONSTRUCTORS

        // METHODS


        private async Task DownloadArtworkForCard(string code)
        {
            try
            {
                HttpClient client = new HttpClient();
                string path = Constants.URL_DL_CARD_IMG_START + code[1] + Constants.URL_DL_CARD_IMG_END + code +
                              Constants.ARTWORK_SUFFIX;
                HttpResponseMessage resp = await client.GetAsync(path);
                if (resp.IsSuccessStatusCode)
                {
                    byte[] respContent = await resp.Content.ReadAsByteArrayAsync();
                    Progress++;
                    // Console.WriteLine($"{code} => downloaded {respContent.Length} bytes.");

                    // TODO: what to do with the bytes now??
                    // BitmapArtwork = ImageHelper.LoadImageFromBytes(respContent);
                    // await File.WriteAllBytesAsync(artworkPath, respContent);
                }
                else
                {
                    throw new Exception(
                        $"Artwork for code [{code}] cannot be downloaded. Riot's servers are down or the " +
                        "given code doesn't correspond to an existing card.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task DownloadArtworks(IEnumerable<string> cardCodes)
        {
            IEnumerable<string> enumerable = cardCodes as string[] ?? cardCodes.ToArray();
            _totalProgress = enumerable.Count();
            Progress = 0;
            await Task.WhenAll(enumerable.Select(DownloadArtworkForCard));
            if ((int) Progress != (int) TotalProgress)
            {
                throw new Exception(
                    $"{TotalProgress - Progress} Error(s) happened while downloading all card artworks.");
            }
        }
    }
}