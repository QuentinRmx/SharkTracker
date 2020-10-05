using System.ComponentModel;
using Newtonsoft.Json;
using SharkTrackerCore.Observation;

namespace SharkTrackerCore.Models
{
    public class Card : AbstractObservable
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        [JsonIgnore] private int _quantityOwned;

        [JsonProperty("quantityOwned")]
        public int QuantityOwned
        {
            get => _quantityOwned;
            set
            {
                _quantityOwned = value;
                PropertyChanged?.Invoke(this, _propertyChangedEventArgsQuantityOwned);
            }
        }

        [JsonIgnore] private static readonly PropertyChangedEventArgs _propertyChangedEventArgsQuantityOwned =
            new PropertyChangedEventArgs(nameof(QuantityOwned));

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
       
        
        // [JsonIgnore]
        // public Brush RarityColor
        // {
        //     get
        //     {
        //         return RarityEnum switch
        //         {
        //             ERarity.Champion => new SolidColorBrush(Colors.Gold),
        //             ERarity.Epic => new SolidColorBrush(Colors.Purple),
        //             ERarity.Rare => new SolidColorBrush(Colors.RoyalBlue),
        //             ERarity.Common => new SolidColorBrush(Colors.ForestGreen),
        //             _ => new SolidColorBrush(Colors.Red)
        //         };
        //     }
        // }
    }
}