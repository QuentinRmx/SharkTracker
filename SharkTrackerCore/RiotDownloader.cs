using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharkTrackerCore.Models;

namespace SharkTrackerCore
{
    public class RiotDownloader
    {
        // EVENTS

        public event EventHandler OnProgress;

        // ATTRIBUTES

        /// <summary>
        /// Gives the progress (%) for the current task.
        /// </summary>
        public double ProgressPercent => (Progress * 100) / TotalProgress;


        private readonly object _lockProgress = new object();

        private double _progress;

        public double Progress
        {
            get => _progress;
            set
            {
                lock (_lockProgress)
                {
                    _progress = value;
                    OnProgress?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private readonly object _lockTotalProgress = new object();

        private double _totalProgress;

        public double TotalProgress
        {
            get => _totalProgress;
            set
            {
                lock (_lockTotalProgress)
                {
                    _totalProgress = value;
                }
            }
        }

        // CONSTRUCTORS

        internal RiotDownloader()
        {
            CheckFolderStructure();
        }

        private static void CheckFolderStructure()
        {
            string[] directories = {"Data", "en_us", "img", "cards", "alt"};
            string path = Path.GetFullPath("./");
            foreach (string directory in directories)
            {
                path = Path.Combine(path, directory);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }


        // METHODS


        // private async Task DownloadArtworkForCard(HttpClient client, string code)
        // {
        //     try
        //     {
        //         string path = Constants.URL_LATEST_SET_BASE + code[1] + Constants.URL_DL_CARD_IMG_END + code +
        //                       Constants.ARTWORK_SUFFIX;
        //         HttpResponseMessage resp = await client.GetAsync(path);
        //         if (resp.IsSuccessStatusCode)
        //         {
        //             // byte[] respContent = await resp.Content.ReadAsByteArrayAsync();
        //             Progress++;
        //             // Console.WriteLine($"{code} => downloaded {respContent.Length} bytes.");
        //
        //             // TODO: what to do with the bytes now??
        //             // BitmapArtwork = ImageHelper.LoadImageFromBytes(respContent);
        //             // await File.WriteAllBytesAsync(artworkPath, respContent);
        //         }
        //         else
        //         {
        //             throw new Exception(
        //                 $"Artwork for code [{code}] cannot be downloaded. Riot's servers are down or the " +
        //                 "given code doesn't correspond to an existing card.");
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //     }
        // }

        // public async Task DownloadArtworks(HttpClient client, IEnumerable<string> cardCodes)
        // {
        //     IEnumerable<string> codes = cardCodes as string[] ?? cardCodes.ToArray();
        //     TotalProgress = codes.Count();
        //     Progress = 0;
        //     await Task.WhenAll(codes.Select(code => DownloadArtworkForCard(client, code)));
        //     if ((int) Progress != (int) TotalProgress)
        //     {
        //         throw new Exception(
        //             $"{TotalProgress - Progress} Error(s) happened while downloading all card artworks.");
        //     }
        // }

        /// <summary>
        /// Order the RiotDownloader to start downloading the corresponding zip file from Riot's servers 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Card>> DownloadAllSets(HttpClient client)
        {
            // TODO: Delete zip files.
            int[] sets = new int[Constants.SET_AMOUNT];
            // temporary amount
            TotalProgress = 0;
            Progress = 0;
            for (int setIndex = 0; setIndex < sets.Length; setIndex++)
            {
                sets[setIndex] = setIndex + 1;
            }

            List<Card>[] setsCards = await Task.WhenAll(sets.Select(s => DownloadSet(client, s)));
            List<Card> cards = setsCards.SelectMany(c => c).ToList();

            // TODO: fix that threading issue instead of patching it...
            // if (TotalProgress - Progress < 1 && TotalProgress - Progress != 0)
            // {
            //     Progress = TotalProgress;
            // }

            return cards;
        }

        public async Task<List<Card>> DownloadSet(HttpClient client, int setIndex, bool overwrite = false)
        {
            try
            {
                string path = Constants.URL_LATEST_SET_BASE + setIndex + "-" + Constants.LOCAL_ID +
                              Constants.SET_SUFFIX;

                string zipPath = Path.GetFullPath("./set" + setIndex + ".zip");
                if (overwrite || !File.Exists(zipPath))
                {
                    HttpResponseMessage resp = await client.GetAsync(path);
                    if (resp.IsSuccessStatusCode)
                    {
                        // Parse and save zip file.
                        HttpContent content = resp.Content;
                        byte[] contentStream = await content.ReadAsByteArrayAsync();
                        await File.WriteAllBytesAsync(zipPath, contentStream);
                        content.Dispose();
                    }
                }

                using ZipArchive zipFile = ZipFile.OpenRead(zipPath);
                // We update the total progress to reflect the real amount of tasks (nb of pictures + 1 json file).
                int tasks = zipFile.Entries.Count(e => e.FullName.Contains(Constants.ARTWORK_SUFFIX)) + 1;
                TotalProgress += tasks;
                List<Card> cards = new List<Card>();
                foreach (ZipArchiveEntry entry in zipFile.Entries)
                {
                    if (entry.FullName.Contains("set" + setIndex + "-" + Constants.LOCAL_ID + ".json"))
                    {
                        string jsonPath = Path.GetFullPath("./" + entry.Name);
                        // Console.WriteLine(jsonPath);
                        // if (!File.Exists(jsonPath))
                        // {
                        //     File.Create(jsonPath);
                        // }
                        entry.ExtractToFile(jsonPath, true);
                        // Console.WriteLine($"{Progress}/{TotalProgress} - {entry.FullName} ({path})");
                        cards = JsonConvert.DeserializeObject<List<Card>>(await File.ReadAllTextAsync(jsonPath));
                        Progress++;
                    }
                    // TODO: we shouldn't check like that if the file already exist because it won't overwrite it if it's changed.
                    // In the future, change the condition so things that get changed can be overwritten properly.
                    // As for now, it should be fine but it MUST be changed before release.
                    else if (entry.FullName.Contains(Constants.ARTWORK_SUFFIX))
                    {
                        if (entry.FullName.Contains("alt") &&
                            !File.Exists(Path.GetFullPath(Constants.PATH_CACHE_ARTWORK + "alt/" + entry.Name)))
                        {
                            string artworkPath = Path.GetFullPath(Constants.PATH_CACHE_ARTWORK + "alt/" + entry.Name);
                            if (!File.Exists(artworkPath))
                                entry.ExtractToFile(artworkPath, true);
                            // Console.WriteLine($"{Progress}/{TotalProgress} - {entry.FullName} ({path})");
                        }
                        else
                        {
                            string artworkPath = Path.GetFullPath(Constants.PATH_CACHE_ARTWORK + entry.Name);
                            if (!File.Exists(artworkPath))
                                entry.ExtractToFile(artworkPath, true);
                            // Console.WriteLine($"{Progress}/{TotalProgress} - {entry.FullName} ({path})");
                        }

                        Progress++;
                    }
                }

                return cards;
            }
            catch (Exception e)
            {
                throw new Exception(
                    "An error happened while downloading the latest sets json. See inner exception for details.", e);
            }
        }
    }
}