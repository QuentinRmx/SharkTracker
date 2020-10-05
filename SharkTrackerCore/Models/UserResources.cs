using Newtonsoft.Json;

namespace SharkTrackerCore.Models
{
    public class UserResources
    {

        // ATTRIBUTES
        
        [JsonProperty("champion wcs")]
        public int ChampionWcs { get; set; }
        
        [JsonProperty("epic wcs")]
        public int EpicWcs { get; set; }
        
        [JsonProperty("rare wcs")]
        public int RareWcs { get; set; }
        
        [JsonProperty("common wcs")]
        public int CommonWcs { get; set; }
        
        [JsonProperty("shards")]
        public int Shards { get; set; }

        // CONSTRUCTORS

        // METHODS


    }
}