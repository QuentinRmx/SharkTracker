using Newtonsoft.Json;

namespace SharkTracker.Data.Models
{
    public class Card
    {

        public int Id { get; set; }

        [JsonProperty("cardCode")] public string Code { get; set; }

        [JsonProperty("cost")] public int Cost { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("region")] public string Region { get; set; }

        [JsonProperty("attack")] public string Attack { get; set; }

        [JsonProperty("Health")] public string Health { get; set; }

        [JsonProperty("descriptionRaw")] public string Description { get; set; }

        [JsonProperty("levelupDescriptionRaw")]
        public string LevelUpDescription { get; set; }

        [JsonProperty("rarity")] public string Rarity { get; set; }

        [JsonProperty("collectible")] public bool Collectible { get; set; }

        [JsonIgnore] private Image _artwork;

        [JsonIgnore]
        public Image Artwork
        {
            get
            {
                if (_artwork is null)
                {
                    _artwork = new Image
                    {
                        Source = ImageSource.FromFile(FileAccessHelper.GetLocalFilePath("artwork\\" + Code + ".png"))
                    };
                }

                return _artwork;
            }
        }

        [JsonIgnore]
        public string ArtworkPath => FileAccessHelper.GetLocalFilePath("artwork\\" + Code + ".png");
    }
}
