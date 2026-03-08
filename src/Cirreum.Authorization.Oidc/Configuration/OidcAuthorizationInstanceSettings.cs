namespace Cirreum.Authorization.Configuration;

using Cirreum.AuthorizationProvider.Configuration;

/// <summary>
/// Instance settings for a generic OIDC authorization provider.
/// Each instance represents one OIDC issuer (e.g., Entra External, Auth0, Okta).
/// </summary>
public class OidcAuthorizationInstanceSettings
	: AudienceAuthorizationProviderInstanceSettings {

	/// <summary>
	/// Gets or sets the OIDC authority URL (issuer).
	/// Must publish a <c>.well-known/openid-configuration</c> document.
	/// </summary>
	/// <example>https://myapp.ciamlogin.com/tenant-id/v2.0</example>
	/// <example>https://myapp.auth0.com</example>
	public string Authority { get; set; } = "";

	/// <summary>
	/// Gets or sets the required scopes that must be present in the token.
	/// When configured, tokens missing any of these scopes will be rejected.
	/// Scopes are checked against both the <c>scp</c> and <c>scope</c> claims.
	/// </summary>
	public List<string> RequiredScopes { get; set; } = [];

}
