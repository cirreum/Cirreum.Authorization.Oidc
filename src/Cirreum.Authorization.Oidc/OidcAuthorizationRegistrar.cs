namespace Cirreum.Authorization;

using System.Security.Claims;
using Cirreum.Authorization.Configuration;
using Cirreum.AuthorizationProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Registrar for generic OIDC authorization provider instances.
/// Validates JWTs from any OIDC-compliant issuer using standard <c>AddJwtBearer()</c>.
/// No vendor-specific SDK dependency.
/// </summary>
public sealed class OidcAuthorizationRegistrar
	: AudienceAuthorizationProviderRegistrar<
		OidcAuthorizationSettings,
		OidcAuthorizationInstanceSettings> {

	/// <inheritdoc/>
	public override string ProviderName => "Oidc";

	/// <inheritdoc/>
	public override void ValidateSettings(OidcAuthorizationInstanceSettings settings) {

		if (string.IsNullOrWhiteSpace(settings.Authority)) {
			throw new InvalidOperationException(
				$"OIDC provider instance '{settings.Scheme}' requires an Authority.");
		}

	}

	/// <inheritdoc/>
	public override void AddAuthorizationForWebApi(IConfigurationSection instanceSection,
		OidcAuthorizationInstanceSettings providerSettings,
		AuthenticationBuilder authBuilder) {
		authBuilder.AddJwtBearer(providerSettings.Scheme, options => {
			options.Authority = providerSettings.Authority;
			options.Audience = providerSettings.Audience;
			options.RequireHttpsMetadata = true;

			if (providerSettings.RequiredScopes is { Count: > 0 }) {
				var requiredScopes = providerSettings.RequiredScopes;
				options.Events = new JwtBearerEvents {
					OnTokenValidated = context => {
						ValidateRequiredScopes(context.Principal, requiredScopes, context);
						return Task.CompletedTask;
					}
				};
			}
		});
	}

	/// <inheritdoc/>
	public override void AddAuthorizationForWebApp(IConfigurationSection instanceSection,
		OidcAuthorizationInstanceSettings providerSettings,
		AuthenticationBuilder authBuilder) {
		authBuilder.AddOpenIdConnect(providerSettings.Scheme, options => {
			options.Authority = providerSettings.Authority;
			options.ClientId = providerSettings.Audience;
			options.RequireHttpsMetadata = true;

			if (providerSettings.RequiredScopes is { Count: > 0 }) {
				foreach (var scope in providerSettings.RequiredScopes) {
					options.Scope.Add(scope);
				}
			}
		});
	}

	private static void ValidateRequiredScopes(
		ClaimsPrincipal? principal,
		List<string> requiredScopes,
		Microsoft.AspNetCore.Authentication.JwtBearer.TokenValidatedContext context) {

		if (principal is null) {
			context.Fail("No principal available for scope validation.");
			return;
		}

		// OIDC issuers use either "scp" (space-delimited) or "scope" claim
		var scopeClaim = principal.FindFirst("scp") ?? principal.FindFirst("scope");
		if (scopeClaim is null) {
			context.Fail("Token does not contain a scope claim.");
			return;
		}

		var tokenScopes = scopeClaim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		foreach (var required in requiredScopes) {
			if (!tokenScopes.Contains(required, StringComparer.OrdinalIgnoreCase)) {
				context.Fail($"Token is missing required scope '{required}'.");
				return;
			}
		}

	}

}
