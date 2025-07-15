using MF2024_API.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Security.Claims;

namespace MF2024_API.Interfaces
{
    public interface ITokenService
    {
        //Login�p
        string CreateToken(User user,IList<string> roles);
        //Web�p
        string CreateTokenReservation(string code);

        //���t���b�V���g�[�N�����s�p
        string CreateRefreshToken();
        //���t���b�V���g�[�N���ۑ��p�iDB�j
        void SaveRefreshToken(string token, string userId);
        //���t���b�V���g�[�N�����ؗp
        bool VerifyRefreshToken(string token,User user);
        //�A�N�Z�X�g�[�N�����ؗp
        ClaimsPrincipal VerifyToken(string token);

    }
}