﻿using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagement.API.Container.Implimentation
{
    public class Authorize : IAuthorize
    {
        private readonly LibraryProjectContext _dbContext;
        private readonly JWTSettings jwtSettings;
        private readonly int tokenTime = 20;
        private readonly int refreshTokenTime = 2000;

        public Authorize(LibraryProjectContext dbContext, IOptions<JWTSettings> options)
        {
            this._dbContext = dbContext;
            this.jwtSettings = options.Value;
        }

        public async Task<APIResponse<TokenResponse>> GenerateToken(UserCredentails usercred)
        {
            APIResponse<TokenResponse> response = new();
            var user = await this._dbContext.Users.FirstOrDefaultAsync(x => (x.Username == usercred.Username || x.Email == usercred.Username) && x.Password == usercred.Password);
            if (user != null)
            {
                // Generate Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(this.jwtSettings.securityKey);
                var tokenDesc = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddSeconds(tokenTime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDesc);
                var finalToken = tokenHandler.WriteToken(token);

                response.Data = new TokenResponse() { Token = finalToken, RefreshToken = await GenerateRefreshToken(user.Username) };
                response.IsSuccess = true;
                response.ResponseCode = 200;
            }
            else
            {
                response.ResponseCode = 401;
                response.ErrorMessage = "UnAuthorized";
            }
            return response;
        }

        public async Task<APIResponse<TokenResponse>> GenerateRefreshToken(TokenResponse tokenResponse)
        {
            APIResponse<TokenResponse> response = new();
            var _refreshToken = await this._dbContext.AuthenticationRefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == tokenResponse.RefreshToken);
            if (_refreshToken != null)
            {
                // Generate Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(this.jwtSettings.securityKey);
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(tokenResponse.Token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out securityToken);

                var token = securityToken as JwtSecurityToken;
                if (token != null && token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    string username = principal.Identity.Name;
                    var existData = await this._dbContext.AuthenticationRefreshTokens.FirstOrDefaultAsync(x => x.UserId == username && x.RefreshToken == tokenResponse.RefreshToken);
                    if (existData != null)
                    {
                        var newToken = new JwtSecurityToken(
                            claims: principal.Claims.ToArray(),
                            expires: DateTime.Now.AddSeconds(refreshTokenTime),
                            signingCredentials: new SigningCredentials(
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtSettings.securityKey)), SecurityAlgorithms.HmacSha256));
                        var finalToken = tokenHandler.WriteToken(newToken);

                        response.Data = new TokenResponse() { Token = finalToken, RefreshToken = await GenerateRefreshToken(username) };
                        response.IsSuccess = true;
                        response.ResponseCode = 200;
                    }
                }
                else
                {
                    response.ResponseCode = 401;
                    response.ErrorMessage = "UnAuthorized";
                }
            }
            else
            {
                response.ResponseCode = 401;
                response.ErrorMessage = "UnAuthorized";
            }
            return response;
        }

        public async Task<string> GenerateRefreshToken(string username)
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                string refreshToken = Convert.ToBase64String(randomNumber);
                var ExistingToken = await _dbContext.AuthenticationRefreshTokens.FirstOrDefaultAsync(x => x.UserId == username);
                if (ExistingToken != null)
                {
                    ExistingToken.RefreshToken = refreshToken;
                }
                else
                {
                    await this._dbContext.AuthenticationRefreshTokens.AddAsync(new AuthenticationRefreshToken
                    {
                        UserId = username,
                        TokenId = new Random().Next().ToString(),
                        RefreshToken = refreshToken,

                    });
                }
                await this._dbContext.SaveChangesAsync();
                return refreshToken;
            }
        }

    }
}
