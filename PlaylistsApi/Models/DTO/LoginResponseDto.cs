using System.Text.Json.Serialization;

namespace PlaylistsApi.Models.DTO;

public class LoginResponseDto
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("expiration")]
    public long Expiration { get; set; }

    [JsonPropertyName("criadorId")]
    public int CriadorId { get; set; }

    [JsonPropertyName("nomeCriador")]
    public required string Nome { get; set; }
}
