# Cirreum.Authorization.Oidc

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.Authorization.Oidc.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Authorization.Oidc/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Cirreum.Authorization.Oidc.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Authorization.Oidc/)
[![GitHub Release](https://img.shields.io/github/v/release/cirreum/Cirreum.Authorization.Oidc?style=flat-square&labelColor=1F1F1F&color=FF3B2E)](https://github.com/cirreum/Cirreum.Authorization.Oidc/releases)
[![License](https://img.shields.io/github/license/cirreum/Cirreum.Authorization.Oidc?style=flat-square&labelColor=1F1F1F&color=F2F2F2)](https://github.com/cirreum/Cirreum.Authorization.Oidc/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-003D8F?style=flat-square&labelColor=1F1F1F)](https://dotnet.microsoft.com/)

**Generic OIDC JWT validation for any OpenID Connect-compliant issuer.**

## Overview

**Cirreum.Authorization.Oidc** is an authorization provider for the Cirreum framework that validates JWTs from any OIDC issuer using standard `AddJwtBearer()` — no vendor-specific SDK dependency.

### When to use

Use this provider when your API needs to validate tokens from a standard OIDC issuer:

| Token source | Use this? | Why |
|---|---|---|
| **Entra External ID** (CIAM) | Yes | Standard OIDC issuer, no Microsoft-specific features needed |
| **Auth0** | Yes | Standard OIDC |
| **Okta** | Yes | Standard OIDC |
| **Keycloak** | Yes | Standard OIDC |
| Any `.well-known/openid-configuration` issuer | Yes | That's all this provider needs |
| **Entra Workforce** (employees) | Use `.Entra` | Needs Microsoft.Identity.Web for Graph, OBO, sovereign clouds |
| **Customer-owned IdPs** (B2B SaaS) | Use `.External` | Needs dynamic tenant resolution at runtime |

### Installation

```
dotnet add package Cirreum.Authorization.Oidc
```

### Configuration

```json
{
  "Cirreum": {
    "Authorization": {
      "Providers": {
        "Oidc": {
          "Instances": {
            "entraExternal": {
              "Enabled": true,
              "Authority": "https://myapp.ciamlogin.com/tenant-id/v2.0",
              "Audience": "50eca7c5-..."
            },
            "auth0": {
              "Enabled": true,
              "Authority": "https://myapp.auth0.com",
              "Audience": "https://my-api",
              "RequiredScopes": ["access_as_user"]
            }
          }
        }
      }
    }
  }
}
```

### Instance settings

| Property | Required | Description |
|---|---|---|
| `Enabled` | Yes | Enable/disable this instance |
| `Authority` | Yes | OIDC issuer URL (must publish `.well-known/openid-configuration`) |
| `Audience` | Yes | Expected JWT `aud` claim — used for audience-based scheme routing |
| `RequiredScopes` | No | Scopes that must be present in the `scp`/`scope` claim |

### Registration

The provider is registered automatically by the Cirreum authorization pipeline via configuration. No manual registration code is needed beyond standard `builder.AddAuthorization()`.

```csharp
// In HostingExtensions (framework-level)
builder.RegisterAuthorizationProvider<
    OidcAuthorizationRegistrar,
    OidcAuthorizationSettings,
    OidcAuthorizationInstanceSettings>(authenticationBuilder);
```

## Contribution Guidelines

1. **Be conservative with new abstractions**
   The API surface must remain stable and meaningful.

2. **Limit dependency expansion**
   Only add foundational, version-stable dependencies.

3. **Favor additive, non-breaking changes**
   Breaking changes ripple through the entire ecosystem.

4. **Include thorough unit tests**
   All primitives and patterns should be independently testable.

5. **Document architectural decisions**
   Context and reasoning should be clear for future maintainers.

6. **Follow .NET conventions**
   Use established patterns from Microsoft.Extensions.* libraries.

## Versioning

Cirreum.Authorization.Oidc follows [Semantic Versioning](https://semver.org/):

- **Major** - Breaking API changes
- **Minor** - New features, backward compatible
- **Patch** - Bug fixes, backward compatible

Given its foundational role, major version bumps are rare and carefully considered.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Cirreum Foundation Framework**  
*Layered simplicity for modern .NET*
