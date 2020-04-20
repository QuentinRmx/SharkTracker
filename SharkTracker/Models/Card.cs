using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using SharkTracker.Utils;

namespace SharkTracker.Models
{
    public class Card
    {
        // ATTRIBUTES

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("cardCode")] public string Code { get; set; }

        [JsonProperty("cost")] public int Cost { get; set; }

        [JsonProperty("region")] public string Region { get; set; }

        [JsonProperty("attack")] public string Attack { get; set; }

        [JsonProperty("Health")] public string Health { get; set; }

        [JsonProperty("descriptionRaw")] public string Description { get; set; }

        [JsonProperty("levelupDescriptionRaw")]
        public string LevelUpDescription { get; set; }

        [JsonProperty("rarity")] public string Rarity { get; set; }

        [JsonProperty("collectible")] public bool Collectible { get; set; }

        [JsonProperty("quantityOwned")] public int QuantityOwned { get; set; } = 0;

        [JsonIgnore]
        public ERarity RarityEnum
        {
            get
            {
                ERarity rarityEnum = Rarity switch
                {
                    "Champion" => ERarity.Champion,
                    "Epic" => ERarity.Epic,
                    "Rare" => ERarity.Rare,
                    "Common" => ERarity.Common,
                    _ => ERarity.Common
                };

                return rarityEnum;
            }
        }

        [JsonIgnore] private BitmapSource _bitmapArtwork = null;

        [JsonIgnore]
        public BitmapSource BitmapArtwork
        {
            get => _bitmapArtwork;
            private set => _bitmapArtwork = value;
        }

        // CONSTRUCTORS

        public Card()
        {
        }

        // METHODS

        public async Task<BitmapSource> GetArtworkFromInternet()
        {
            BitmapArtwork = new BitmapImage();
            if (string.IsNullOrEmpty(Code))
            {
                return BitmapArtwork;
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage resp = await client.GetAsync(Constants.URL_DL_CARD_IMG + Code + ".png");
            if (resp.IsSuccessStatusCode)
            {
                byte[] respContent = await resp.Content.ReadAsByteArrayAsync();
                BitmapArtwork = ImageHelper.LoadImage(respContent);
            }

            return BitmapArtwork;
        }
    }
}