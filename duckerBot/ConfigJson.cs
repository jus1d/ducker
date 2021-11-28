using Newtonsoft.Json;

namespace duckerBot
{
    public class ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        
        [JsonProperty("spotifyId")]
        public string SpotifyId { get; private set;  }
        
        [JsonProperty("spotifySecret")]
        public string SpotifySecret { get; private set;  }
    }
}