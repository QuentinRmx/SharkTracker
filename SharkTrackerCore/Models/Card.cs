using Newtonsoft.Json;


namespace SharkTrackerCore.Models
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

        // CONSTRUCTORS

        // METHODS
    }
}