using AlphaShopArticoliAPI.Models;
using AlphaShopArticoliAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AlphaShopArticoliAPI.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService userService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService)
            : base(options, logger, encoder, clock)
        {
            this.userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization Header Mancante");

            Utenti utente = null;

            bool IsOk = false;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                var username = credentials[0];
                var password = credentials[1];

                IsOk = await userService.Authenticate(username, password);

                if (IsOk)
                {
                    utente = await userService.GetUser(username);
                }
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail("Authorization Header Non Valido!" + ex.Message.ToString());
            }

            if (!IsOk)
            {
                return AuthenticateResult.Fail("Nome utente o password errati!");
            }

            ICollection<Profili> userProfiles = utente.Profili;

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, utente.UserId));

            foreach (var Profile in userProfiles)
            {
                claims.Add(new Claim(ClaimTypes.Role, Profile.Tipo));
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);


        }
    }
}
