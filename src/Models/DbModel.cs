using Newtonsoft.Json;

namespace BlinkBro.Models;

public class DbModel
{
    [JsonProperty("BlinkCount")] public int BlinkCount { get; set; }
}