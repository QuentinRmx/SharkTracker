using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharkTracker.Models
{
    public class Card
    {

        // ATTRIBUTES
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("cardCode")]
        public string Code { get; set; }
        
        [JsonProperty("cost")]
        public string Cost { get; set; }
        
        [JsonProperty("region")]
        public string Region { get; set; }
        
        [JsonProperty("attack")]
        public string Attack { get; set; }

        [JsonProperty("Health")]
        public string Health { get; set; }
        
        [JsonProperty("descriptionRaw")]
        public string Description { get; set; }
        
        [JsonProperty("levelupDescriptionRaw")]
        public string LevelUpDescription { get; set; }
        
        // CONSTRUCTORS

        public Card()
        {
            
        }

        // METHODS


    }
}