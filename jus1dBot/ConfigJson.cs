using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;

namespace jus1dBot
{
    public class ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
    }
}