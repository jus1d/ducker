using System.Text;
using Newtonsoft.Json;

namespace ducker.Config
{
    public class ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        
        [JsonProperty("id")]
        public ulong Id { get; private set; }
        
        [JsonProperty("spotifyId")]
        public string SpotifyId { get; private set; }
        
        [JsonProperty("spotifySecret")]
        public string SpotifySecret { get; private set; }

        public static ConfigJson GetConfigField()
        {
            var json = string.Empty;

            using ( var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr?.ReadToEndAsync().Result;
            return JsonConvert.DeserializeObject<ConfigJson>(json);
        }
    }
}