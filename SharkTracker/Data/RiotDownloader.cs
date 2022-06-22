using Newtonsoft.Json;
using SharkTracker.Data.Models;
using System.IO.Compression;

namespace SharkTracker.Data
{
    public class RiotDownloader
    {

        private static string _pathSetZip = "https://dd.b.pvp.net/latest/set";

        private static string _suffixSet = "-en_us.zip";

        private static string _nameZipFile = "set";

        private static string _artworkSuffix = @".png";

        private static int _lastSet = 6;


        public static async Task<List<Card>> GetAllCardsData()
        {
            var result = new List<Card>();

            try
            {
                List<Task> actionList = new();

                for (int index = 1; index <= _lastSet; index++)
                {

                    actionList.Add(DownloadSet(index, false, false));
                }

                result = await RunTasks(actionList.ToArray());

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return result;
            }

            return result;
        }

        public async static Task<List<Card>> RunTasks(Task[] tasks)
        {
            var result = new List<Card>();
            await Task.WhenAll(tasks);

            for (int index = 1; index <= _lastSet; index++)
            {
                result.AddRange(await LoadCardsFromJson(index));
            }

            return result;
        }

        private static async Task<bool> DownloadSet(int index, bool fullSet, bool overwrite)
        {
            try
            {
                string zipPath = FileAccessHelper.GetLocalFilePath(_nameZipFile + index + _suffixSet);
                string jsonPath = FileAccessHelper.GetLocalFilePath("set" + index + ".json");
                string artworkDirectory = FileAccessHelper.GetLocalFilePath("artwork");

                if (!Directory.Exists(artworkDirectory))
                {
                    Directory.CreateDirectory(artworkDirectory);
                }

                if (overwrite || (!File.Exists(jsonPath) && !File.Exists(zipPath)))
                {
                    using HttpClient client = new HttpClient();

                    HttpResponseMessage resp = await client.GetAsync(_pathSetZip + index + _suffixSet);
                    if (!resp.IsSuccessStatusCode) throw new Exception("No response from server, impossible to download");

                    // caching zip file locally
                    HttpContent content = resp.Content;
                    byte[] contentStream = await content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(zipPath, contentStream);
                    content.Dispose();
                    contentStream = null;
                }

                using ZipArchive zipFile = ZipFile.OpenRead(zipPath);
                foreach (ZipArchiveEntry entry in zipFile.Entries)
                {
                    if (entry.FullName.Contains("set" + index + "-en_us.json") && !File.Exists(jsonPath))
                    {
                        entry.ExtractToFile(jsonPath, overwrite);

                        break;     
                    }
                    //else if (entry.FullName.Contains(_artworkSuffix) && !File.Exists(FileAccessHelper.GetLocalFilePath("artwork/" + entry.Name)))
                    //{
                    //    string artworkPath = FileAccessHelper.GetLocalFilePath("artwork/" + entry.Name);
                    //    entry.ExtractToFile(artworkPath);
                    //}

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        private static async Task<List<Card>> LoadCardsFromJson(int index)
        {
            List<Card> result = new List<Card>();
            string jsonPath = FileAccessHelper.GetLocalFilePath("set" + index + ".json");
            string json = await File.ReadAllTextAsync(jsonPath);
            result = JsonConvert.DeserializeObject<List<Card>>(json);

            return result;

        }
    }


}
