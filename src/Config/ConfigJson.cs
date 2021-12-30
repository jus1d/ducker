using System.Text;
using Newtonsoft.Json;

namespace ducker.Config;

public class ConfigJson
{
    /// <summary>
    ///     Get bot token from config file
    /// </summary>
    [JsonProperty("token")]
    public string? Token { get; private set; }

    /// <summary>
    ///     Get bot commands prefix from config file
    /// </summary>
    [JsonProperty("prefix")]
    public string? Prefix { get; private set; }

    /// <summary>
    ///     Get bot ID from config file
    /// </summary>
    [JsonProperty("id")]
    public ulong Id { get; private set; }

    /// <summary>
    ///     Get Spotify Client ID from config file
    /// </summary>
    [JsonProperty("spotifyId")]
    public string? SpotifyId { get; private set; }

    /// <summary>
    ///     Get Spotify Client Secret from config file
    /// </summary>
    [JsonProperty("spotifySecret")]
    public string? SpotifySecret { get; private set; }

    [JsonProperty("mySqlConnectionString")]
    public string? MySqlConnectionString { get; private set; }

    /// <summary>
    ///     Method to get config field
    /// </summary>
    /// <returns>Return config field</returns>
    public static ConfigJson GetConfigField()
    {
        var json = string.Empty;

        using (var fs = File.OpenRead("config.json"))
        using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
        {
            json = sr?.ReadToEndAsync().Result;
        }

        return JsonConvert.DeserializeObject<ConfigJson>(json);
    }
}