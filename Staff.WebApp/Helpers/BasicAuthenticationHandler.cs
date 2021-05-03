using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


using Staff.Data;
using Staff.Data.Models;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace Staff.WebApp.Helpers
{
    public class BasicAuthenticationEventsHandler : BasicAuthenticationEvents
    {
        private readonly IUserService _userService;

        public BasicAuthenticationEventsHandler( IUserService userService)
        {
            _userService = userService;
        }
        public override Task ValidatePrincipalAsync(ValidatePrincipalContext context)
        {
            var user = _userService.Authenticate(context.UserName, context.Password);
            if( user != null )
            {
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                context.Principal = principal;

            }
            return Task.CompletedTask;
        }
    }
}
