using System.Text.Json.Serialization;

namespace Application.Dtos;

public record MoviesActorsFormDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("character")]
    public string Character { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("picture")]
    public string Picture { get; set; }
}
