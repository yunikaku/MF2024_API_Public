using MF2024_API.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Security.Claims;

namespace MF2024_API.Interfaces
{
    public interface ITokenService
    {
        //Login用
        string CreateToken(User user,IList<string> roles);
        //Web用
        string CreateTokenReservation(string code);

        //リフレッシュトークン発行用
        string CreateRefreshToken();
        //リフレッシュトークン保存用（DB）
        void SaveRefreshToken(string token, string userId);
        //リフレッシュトークン検証用
        bool VerifyRefreshToken(string token,User user);
        //アクセストークン検証用
        ClaimsPrincipal VerifyToken(string token);

    }
}