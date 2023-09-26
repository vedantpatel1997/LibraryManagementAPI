//using LibraryManagement.API.Container.Service;
//using LibraryManagement.API.Repos.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Cryptography;

//public class RefreshHandler : IRefreshHandler
//{
//    private readonly LibraryProjectContext _dbContext;

//    public RefreshHandler(LibraryProjectContext dbContext)
//    {
//        this._dbContext = dbContext;
//    }
//    public async Task<string> GenerateToken(string username)
//    {
//        var randomNumber = new byte[32];
//        using (var randomNumberGenerator = RandomNumberGenerator.Create())
//        {
//            randomNumberGenerator.GetBytes(randomNumber);
//            string refreshToken = Convert.ToBase64String(randomNumber);
//            var ExistingToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserEmailId == username);
//            if (ExistingToken != null)
//            {
//                ExistingToken.RefreshToken1 = refreshToken;
//            }
//            else
//            {
//                await this._dbContext.RefreshTokens.AddAsync(new RefreshToken
//                {
//                    //UserId = username,
//                    TokenId = new Random().Next().ToString(),
//                    RefreshToken1 = refreshToken
//                });
//            }
//            await this._dbContext.SaveChangesAsync();
//            return refreshToken;
//        }

//    }
//}

