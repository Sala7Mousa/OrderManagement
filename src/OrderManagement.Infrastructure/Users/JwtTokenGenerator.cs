using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application;
using OrderManagement.Domain;

namespace OrderManagement.Infrastructure;

public sealed class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    public string Generate(User user)
    {
        var section = configuration.GetSection("Jwt");
        var key = section["Key"] ?? throw new InvalidOperationException("Jwt:Key is required.");
        if (!int.TryParse(section["ExpirationMinutes"], out var expirationMinutes) || expirationMinutes <= 0)
        {
            throw new InvalidOperationException("Jwt:ExpirationMinutes must be a positive integer.");
        }

        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(section["Issuer"], section["Audience"],
            [new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new Claim(JwtRegisteredClaimNames.Email, user.Email), new Claim(ClaimTypes.Role, user.Role)],
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
