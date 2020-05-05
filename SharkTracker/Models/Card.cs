using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;
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
                ERarity rarityEnum = Rarity.ToLower() switch
                {
                    "champion" => ERarity.Champion,
                    "epic" => ERarity.Epic,
                    "rare" => ERarity.Rare,
                    "common" => ERarity.Common,
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

        [JsonIgnore]
        public Brush RarityColor
        {
            get
            {
                switch (RarityEnum)
                {
                    case ERarity.Champion:
                        return new SolidColorBrush(Colors.Gold); 
                        break;
                    case ERarity.Epic:
                        return new SolidColorBrush(Colors.Purple); 
                        break;
                    case ERarity.Rare:
                        return new SolidColorBrush(Colors.RoyalBlue); 
                        break;
                    case ERarity.Common:
                    default:
                        return new SolidColorBrush(Colors.ForestGreen); 
                        break;
                }
            }
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
            string path = Constants.URL_DL_CARD_IMG_START + Code[1] + Constants.URL_DL_CARD_IMG_END;
            HttpResponseMessage resp = await client.GetAsync(path + Code + ".png");
            if (resp.IsSuccessStatusCode)
            {
                byte[] respContent = await resp.Content.ReadAsByteArrayAsync();
                BitmapArtwork = ImageHelper.LoadImage(respContent);
            }

            return BitmapArtwork;
        }
    }
}