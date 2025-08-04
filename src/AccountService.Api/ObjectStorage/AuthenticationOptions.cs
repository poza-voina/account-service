namespace AccountService.Api.ObjectStorage;

public class AuthenticationOptions
{
    public required string Realm { get; set; }
    public required string BaseUrl { get; set; }
    public required bool RequireHttpsMetadata { get; set; }
    public required string Audience { get; set; }
    public string Authority => $"{BaseUrl}/realms/{Realm}";
    public string AuthorizationUrl => $"{Authority}/protocol/openid-connect/auth";
    public string TokenUrl => $"{Authority}/protocol/openid-connect/token";
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}