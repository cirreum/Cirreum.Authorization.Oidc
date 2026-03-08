namespace Cirreum.Authorization.Configuration;

using Cirreum.AuthorizationProvider.Configuration;

/// <summary>
/// Root settings for the generic OIDC authorization provider.
/// </summary>
public class OidcAuthorizationSettings
	: AuthorizationProviderSettings<OidcAuthorizationInstanceSettings>;
