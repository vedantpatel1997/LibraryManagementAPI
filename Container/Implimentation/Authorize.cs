//using LibraryManagement.API.Container.Service;
//using LibraryManagement.API.Helper;
//using LibraryManagement.API.Modal;
//using LibraryManagement.API.Repos.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//namespace LibraryManagement.API.Container.Implimentation
//{
//    public class Authorize : IAuthorize
//    {
//        private readonly LibraryProjectContext _dbContext;
//        private readonly JWTSettings jwtSettings;

//        public Authorize(LibraryProjectContext dbContext, IOptions<JWTSettings> options)
//        {
//            this._dbContext = dbContext;
//            this.jwtSettings = options.Value;
//        }



//        public async Task<APITokenResponse> GenerateToken(UserCredentails usercred)
//        {
//            APITokenResponse response = new();
//            var user = await this._dbContext.Users.FirstOrDefaultAsync(x => x.Email == usercred.UserEmail && x.Password == usercred.Password);
//            if (user != null)
//            {
//                // Generate Token
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var tokenKey = Encoding.UTF8.GetBytes(this.jwtSettings.securityKey);
//                var tokenDesc = new SecurityTokenDescriptor
//                {
//                    Subject = new ClaimsIdentity(new Claim[]
//                    {
//                        new Claim(ClaimTypes.Email,user.Email),
//                        new Claim(ClaimTypes.Role, user.Role)
//                    }),
//                    Expires = DateTime.UtcNow.AddSeconds(30),
//                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
//                };
//                var token = tokenHandler.CreateToken(tokenDesc);
//                var finalToken = tokenHandler.WriteToken(token);
//                response.Token = finalToken;
//                response.IsSuccess = true;
//                response.ResponseCode = 200;
//                response.RefreshToken = await GenerateToken(user.Email);
//            }
//            else
//            {
//                response.ResponseCode = 401;
//                response.ErrorMessage = "UnAuthorized";
//            }
//            return response;
//        }

//        public async Task<string> GenerateToken(string userEmail)
//        {
//            var randomNumber = new byte[32];
//            using (var randomNumberGenerator = RandomNumberGenerator.Create())
//            {
//                randomNumberGenerator.GetBytes(randomNumber);
//                string refreshToken = Convert.ToBase64String(randomNumber);
//                var ExistingToken = await _dbContext.AuthenticationRefreshTokens.FirstOrDefaultAsync(x => x.UserId == userEmail);
//                if (ExistingToken != null)
//                {
//                    ExistingToken.RefreshToken = refreshToken;
//                }
//                else
//                {
//                    await this._dbContext.AuthenticationRefreshTokens.AddAsync(new AuthenticationRefreshToken
//                    {
//                        //UserId = userEmail,
//                        TokenId = new Random().Next().ToString(),
//                        RefreshToken1 = refreshToken
//                    });
//                }
//                await this._dbContext.SaveChangesAsync();
//                return refreshToken;
//            }

//        }

//        public async Task<APITokenResponse> GenerateRefreshToken(TokenResponse tokenResponse)
//        {
//            APITokenResponse response = new();
//            var _refreshToken = await this._dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken1 == tokenResponse.RefreshToken);
//            if (_refreshToken != null)
//            {
//                // Generate Token
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var tokenKey = Encoding.UTF8.GetBytes(this.jwtSettings.securityKey);
//                SecurityToken securityToken;
//                var principal = tokenHandler.ValidateToken(tokenResponse.Token, new TokenValidationParameters()
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                }, out securityToken);

//                var _token = securityToken as JwtSecurityToken;
//                if (_token != null && _token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
//                {
//                    string username = principal.Identity?.Name;
//                    var existData = await this._dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserEmailId == username && x.RefreshToken1 == tokenResponse.RefreshToken);
//                    if (existData != null)
//                    {
//                        var _newToken = new JwtSecurityToken(claims: principal.Claims.ToArray(),
//                            expires: DateTime.Now.AddSeconds(30),
//                            signingCredentials: new SigningCredentials(
//                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtSettings.securityKey)),
//                                SecurityAlgorithms.HmacSha256)
//                            );
//                        var _finalToken = tokenHandler.WriteToken(_newToken);
//                        response.Token = _finalToken;
//                        response.IsSuccess = true;
//                        response.ResponseCode = 200;
//                        response.RefreshToken = await GenerateToken(username);
//                    }
//                }
//                else
//                {

//                    response.ResponseCode = 401;
//                    response.ErrorMessage = "UnAuthorized";
//                }

//            }
//            else
//            {
//                response.ResponseCode = 401;
//                response.ErrorMessage = "UnAuthorized";
//            }
//            return response;
//        }
//    }
//}
