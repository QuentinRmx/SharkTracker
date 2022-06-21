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


        public static async Task<List<Card>> GetAllCardsData()
        {
            var result = new List<Card>();

            try
            {
                int index = 1;
                bool success = true;
                // This way we don't have to know what's the latest set, once the Http call fails we know there is no more set to download.
                do
                {
                    success = await DownloadSet(index, false, false);
                    result.AddRange(await LoadCardsFromJson(index));
                    index++;
                } while (success);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return result;
            }

            return result;
        }

        private static async Task<bool> DownloadSet(int index, bool fullSet, bool overwrite)
        {
            try
            {
                string zipPath = FileAccessHelper.GetLocalFilePath(_nameZipFile + index + _suffixSet);
                string jsonPath = FileAccessHelper.GetLocalFilePath("set" + index + ".json");

                if (overwrite || !File.Exists(jsonPath))
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

                if (!File.Exists(jsonPath))
                {
                    using ZipArchive zipFile = ZipFile.OpenRead(zipPath);
                    foreach (ZipArchiveEntry entry in zipFile.Entries)
                    {
                        if (entry.FullName.Contains("set" + index + "-en_us.json"))
                        {
                            entry.ExtractToFile(jsonPath, overwrite);
                            return true;
                        }
                    }
                }

            }
            catch (Exception)
            {

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
