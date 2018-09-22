using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RoomsService.Initialization;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RoomsService.Common.GetToken
{
    class GetTokenStrategy : IGetTokenStrategy
    {
        private readonly IOptions<SecurityOptions> options;

        public GetTokenStrategy(IOptions<SecurityOptions> options)
        {
            this.options = options;
        }

        public Task<string> GetAsync()
        {
            var userId = Guid.NewGuid();
            var securityKey = options.Value.SecurityKey;
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()), };
            var jwtToken = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            return Task.FromResult(jwtTokenHandler.WriteToken(jwtToken));
        }
    }
}
