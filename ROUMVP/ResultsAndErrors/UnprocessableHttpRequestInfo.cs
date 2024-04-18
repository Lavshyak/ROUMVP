using System.Text.Json.Serialization;

namespace ROUMVP.ResultsAndErrors;

public class UnprocessableHttpRequestInfo
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }
    [JsonPropertyName("description")]
    public required string Description { get; set; }
}