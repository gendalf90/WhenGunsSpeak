using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Connection.Rooms
{
    internal class JwtToken : IToken
    {
        private readonly byte[] key;
        private readonly Guid userId;

        public JwtToken(byte[] key, Guid userId)
        {
            this.key = key;
            this.userId = userId;
        }

        public string Get()
        {
            var securityKey = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { new Claim(ClaimTypes.Name, userId.ToString()) };
            var jwtToken = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            return jwtTokenHandler.WriteToken(jwtToken);
        }
    }
}
