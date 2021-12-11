using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ducker
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
        public string SpotifyId { get; private set;  }
        
        [JsonProperty("spotifySecret")]
        public string SpotifySecret { get; private set;  }
        
        [JsonProperty("musicChannelId")]
        public ulong MusicChannelId { get; private set; }
        
        [JsonProperty("serverLogsChannelId")]
        public ulong ServerLogsChannelId { get; private set; }
        
        [JsonProperty("cmdChannelId")]
        public ulong CmdChannelId { get; private set; }

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