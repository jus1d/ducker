﻿using System;
using System.IO;
using System.Text;
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

        public static ConfigJson? GetConfigField()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json =  sr.ReadToEndAsync().ConfigureAwait(false).ToString();
            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            return configJson;
        }
    }
}