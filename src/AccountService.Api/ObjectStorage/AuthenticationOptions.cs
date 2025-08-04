namespace AccountService.Api.ObjectStorage;

// TODO: отрефачить
public class AuthenticationOptions
{
    /// <summary>
    /// {0} - baseUrl or KeycloakDockerUrl
    /// {1} - realmName
    /// </summary>
    private const string PatternRealmSegment = "{0}/realms/{1}";
    public required string Realm { get; set; }
    public required string BaseUrl { get; set; }
    public string? KeycloakDockerUrl { get; set; }
    public required bool RequireHttpsMetadata { get; set; }
    public required string Audience { get; set; }
    public string Authority => string.Format(PatternRealmSegment, KeycloakDockerUrl ?? BaseUrl, Realm);
    public string AuthorizationUrl => string.Format(PatternRealmSegment, BaseUrl, Realm) + "/protocol/openid-connect/auth";
    public string TokenUrl => $"{string.Format(PatternRealmSegment, BaseUrl, Realm)}/protocol/openid-connect/token";
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}