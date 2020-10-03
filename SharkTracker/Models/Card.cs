using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using SharkTracker.Utils;

namespace SharkTracker.Models
{
    public class Card : ObservableObject
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

        [JsonProperty("quantityOwned")] public int QuantityOwned { get; set; }

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

        [JsonIgnore]
        public ERegion RegionEnum
        {
            get
            {
                ERegion regionEnum = Region.ToLower() switch
                {
                    "bilgewater" => ERegion.Bilgewater,
                    "demacia" => ERegion.Demacia,
                    "freljord" => ERegion.Freljord,
                    "ionia" => ERegion.Ionia,
                    "noxus" => ERegion.Noxus,
                    "piltover & zaun" => ERegion.PnZ,
                    "shadow isles" => ERegion.Si,
                    "targon" => ERegion.Targon,
                    _ => ERegion.ALL
                };

                return regionEnum;
            }
        }

        public bool IsSelectedRegion(ERegion region)
        {
            return (region == ERegion.ALL || region == RegionEnum);
        }

        [JsonIgnore] public BitmapSource BitmapArtwork { get; private set; }

        [JsonIgnore]
        public Brush RarityColor
        {
            get
            {
                return RarityEnum switch
                {
                    ERarity.Champion => new SolidColorBrush(Colors.Gold),
                    ERarity.Epic => new SolidColorBrush(Colors.Purple),
                    ERarity.Rare => new SolidColorBrush(Colors.RoyalBlue),
                    ERarity.Common => new SolidColorBrush(Colors.ForestGreen),
                    _ => new SolidColorBrush(Colors.Red)
                };
            }
        }

        // CONSTRUCTORS

        // METHODS

        public async Task LoadArtwork()
        {
            if (BitmapArtwork != null)
                return;
            BitmapArtwork = new BitmapImage();
            if (string.IsNullOrEmpty(Code))
            {
                return;
            }

            string artworkPath = Constants.PATH_CACHE_ARTWORK + Code + Constants.ARTWORK_SUFFIX;
            if (File.Exists(artworkPath))
            {
                // byte[] img = await File.ReadAllBytesAsync(artworkPath);
                // BitmapArtwork = ImageHelper.LoadImageFromBytes(img);
                BitmapArtwork = new BitmapImage(new Uri(artworkPath, UriKind.Relative));
                return;
            }

            HttpClient client = new HttpClient();
            string path = Constants.URL_DL_CARD_IMG_START + Code[1] + Constants.URL_DL_CARD_IMG_END + Code +
                          Constants.ARTWORK_SUFFIX;
            HttpResponseMessage resp = await client.GetAsync(path);
            if (resp.IsSuccessStatusCode)
            {
                byte[] respContent = await resp.Content.ReadAsByteArrayAsync();
                BitmapArtwork = ImageHelper.LoadImageFromBytes(respContent);
                await File.WriteAllBytesAsync(artworkPath, respContent);
            }

            RaisePropertyChanged(nameof(BitmapArtwork));
        }

        /// <summary>
        /// Empties the BitmapArtwork property to reduce memory usage.
        /// </summary>
        public void ClearArtwork()
        {
            BitmapArtwork = null;
        }
    }
}