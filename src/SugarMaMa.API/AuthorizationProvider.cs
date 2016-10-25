using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using SugarMaMa.API.DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using SugarMaMa.API.Services;

namespace SugarMaMa.API
{
    internal class AuthorizationProvider : OpenIdConnectServerProvider
    {
        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            // Reject the token request that don't use grant_type=password or grant_type=refresh_token.
            if (!context.Request.IsPasswordGrantType() && !context.Request.IsRefreshTokenGrantType())
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    description: "Only resource owner password credentials and refresh token " +
                                 "are accepted by this authorization server");

                return Task.FromResult(0);
            }

            // Since there's only one application and since it's a public client
            // (i.e a client that cannot keep its credentials private), call Skip()
            // to inform the server the request should be accepted without 
            // enforcing client authentication.
            context.Skip();
            
            return Task.FromResult(0);
        }

        public override async Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            // Resolve ASP.NET Core Identity's user manager from the DI container.
            var manager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

            // Only handle grant_type=password requests and let ASOS
            // process grant_type=refresh_token requests automatically.
            if (context.Request.IsPasswordGrantType())
            {
                var user = await manager.FindByNameAsync(context.Request.Username);
                if (user == null)
                {
                    context.Reject(
                        error: OpenIdConnectConstants.Errors.InvalidGrant,
                        description: "Invalid credentials.");

                    return;
                }

                // Ensure the user is not already locked out.
                if (manager.SupportsUserLockout && await manager.IsLockedOutAsync(user))
                {
                    context.Reject(
                        error: OpenIdConnectConstants.Errors.InvalidGrant,
                        description: "Invalid credentials.");

                    return;
                }

                // Ensure the password is valid.
                if (!await manager.CheckPasswordAsync(user, context.Request.Password))
                {
                    if (manager.SupportsUserLockout)
                    {
                        await manager.AccessFailedAsync(user);
                    }

                    context.Reject(
                        error: OpenIdConnectConstants.Errors.InvalidGrant,
                        description: "Invalid credentials.");

                    return;
                }

                if (manager.SupportsUserLockout)
                {
                    await manager.ResetAccessFailedCountAsync(user);
                }

                // Reject the token request if two-factor authentication has been enabled by the user.
                if (manager.SupportsUserTwoFactor && await manager.GetTwoFactorEnabledAsync(user))
                {
                    context.Reject(
                        error: OpenIdConnectConstants.Errors.InvalidGrant,
                        description: "Two-factor authentication is required for this account.");

                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationScheme);

                // Note: the name identifier is always included in both identity and
                // access tokens, even if an explicit destination is not specified.
                identity.AddClaim(ClaimTypes.NameIdentifier, await manager.GetUserIdAsync(user));

                // When adding custom claims, you MUST specify one or more destinations.
                // Read "part 7" for more information about custom claims and scopes.
                identity.AddClaim("username", await manager.GetUserNameAsync(user),
                    OpenIdConnectConstants.Destinations.AccessToken,
                    OpenIdConnectConstants.Destinations.IdentityToken);

                identity.AddClaim("firstName", user.FirstName, OpenIdConnectConstants.Destinations.IdentityToken);

                //Add roles
                var roles = await manager.GetRolesAsync(user);
                var roleClaims = new List<Claim>();
                foreach(var role in roles)
                {
                    identity.AddClaim(ClaimTypes.Role, role, OpenIdConnectConstants.Destinations.AccessToken,
                    OpenIdConnectConstants.Destinations.IdentityToken);
                }

                var estheticians = (IEstheticianService)context.HttpContext.RequestServices.GetService(typeof(IEstheticianService));

                var esthetician = await estheticians.GetByEmailAsync(user.Email);

                if (esthetician != null)
                {
                   identity.AddClaim("estheticianId", esthetician.Id.ToString(), OpenIdConnectConstants.Destinations.AccessToken,
                   OpenIdConnectConstants.Destinations.IdentityToken);
                }

                // Create a new authentication ticket holding the user identity.
                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties(),
                    context.Options.AuthenticationScheme);

                // Set the list of scopes granted to the client application.
                ticket.SetScopes(
                    /* openid: */ OpenIdConnectConstants.Scopes.OpenId,
                    /* email: */ OpenIdConnectConstants.Scopes.Email,
                    
                    /* profile: */ OpenIdConnectConstants.Scopes.Profile,
                    /* offline_access */ OpenIdConnectConstants.Scopes.OfflineAccess);
                // Set the resource servers the access token should be issued for.
                ticket.SetResources("resource_server");
                
                context.Validate(ticket);
            }
        }

        //public override Task HandleUserinfoRequest(HandleUserinfoRequestContext context)
        //{
        //    // You can retrieve the claims stored in the access token extracted
        //    // from the userinfo request by accessing the authentication ticket.
        //    var principal = context.Ticket.Principal;

        //    // Set family_name, given_name, birth_date using the corresponding ClaimTypes names:
        //    context.FamilyName = principal.FindClaim(ClaimTypes.Surname)?.Value;
        //    context.GivenName = principal.FindClaim(ClaimTypes.GivenName)?.Value;
        //    context.BirthDate = principal.FindClaim(ClaimTypes.DateOfBirth)?.Value;

        //    // Only expose "custom-claim" if "custom-scope" was granted by the resource owner.
        //    if (context.Ticket.HasScope("custom-scope"))
        //    {
        //        context.Claims["custom-claim"] = "claim-value";
        //    }

        //    return Task.FromResult(0);
        //}

    }
}