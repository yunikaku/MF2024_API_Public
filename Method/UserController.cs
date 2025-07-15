


using MF2024_API.Interfaces;
using MF2024_API.Models;
using MF2024API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MF2024_API.Method
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserService _userService;
        private DateTime DateTime;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signinManager, RoleManager<IdentityRole> roleManager, UserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _signinManager = signinManager;
            _roleManager = roleManager;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        List<string> DeviceRoole = new List<string>() { "RoomDevice", "ConferencRoomDevice", "Web", "Reseption", "PubulicSpace" };

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsers getUsers)
        {
            //roleが一致するユーザーを取得
            var users = await _userManager.GetUsersInRoleAsync(getUsers.Role);
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser([FromQuery] GetUser getUser)
        {
            try
            {
                var query = _userManager.Users.AsQueryable();
                if (getUser.Id != null)
                {
                    query = query.Where(x => x.Id == getUser.Id);
                }
                if (getUser.UserName != null)
                {
                    query = query.Where(x => x.UserName == getUser.UserName);
                }
                if (getUser.UserGender != null)
                {
                    query = query.Where(x => x.UserGender == getUser.UserGender);
                }
                if (getUser.UserAddress != null)
                {
                    query = query.Where(x => x.UserAddress == getUser.UserAddress);
                }
                if (getUser.PhoneNumber != null)
                {
                    query = query.Where(x => x.PhoneNumber == getUser.PhoneNumber);
                }
                if (getUser.Email != null)
                {
                    query = query.Where(x => x.Email == getUser.Email);
                }
                if (getUser.Role != null)
                {
                    var role = await _roleManager.FindByNameAsync(getUser.Role);
                    if (role == null)
                    {
                        return BadRequest("Role not found");
                    }
                    var users = await _userManager.GetUsersInRoleAsync(role.Name);
                    query = query.Where(x => users.Contains(x));
                }
                var result = await query.ToListAsync();
                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetUserRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }



        [HttpPost]
        [Route("Register")]
        [Authorize(Roles = "Admin")]
        //[Authorize]
        public async Task<IActionResult> Register([FromBody] postUser model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserDateOfBirth = model.UserDateOfBirth,
                    UserGender = model.UserGender,
                    UserAddress = model.UserAddress,
                    UserDateOfEntry = model.UserDateOfEntry,
                    UserPasswoedUpdataTime = DateTime.Now,
                    UserDateOfRetirement = DateTime.Now,
                    UserUpdataDate = DateTime.Now,
                    UserUpdataUser = model.UserName

                };
                var createUser = await _userManager.CreateAsync(user, model.Password);
                if (createUser.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(user, model.Role);
                    if (role.Succeeded)
                    {
                        List<string> roles = new List<string>() { model.Role };
                        var accsesstoken = _tokenService.CreateToken(user, roles);
                        return Ok
                            (_tokenService.CreateToken(user, roles));
                    }
                    else
                    {
                        var delete = await _userManager.DeleteAsync(user);
                        return StatusCode(500, role.Errors);
                    }
                }
                else
                {
                    var delete = await _userManager.DeleteAsync(user);
                    return StatusCode(500, createUser.Errors);
                }
            }
            catch (Exception e)
            {
                var delete = await _userManager.FindByNameAsync(model.UserName);
                if (delete != null)
                {
                    var deleteuser = await _userManager.DeleteAsync(delete);
                }
                return StatusCode(500, e);
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpDate([FromBody] UpDateUser upDate)
        {
            using (var context = new Mf2024apiDbContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                    try
                    {
                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }
                        var user = await _userManager.FindByIdAsync(upDate.ID);
                        if (user == null)
                        {
                            return Unauthorized("Invalid username!");
                        }
                        //UserデータのパスワードとUpdataのパスワードが一致しているか確認
                        var result = await _signinManager.CheckPasswordSignInAsync(user, upDate.Password, false);
                        if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");
                        //UpDateのNull以外のデータをUserデータに上書き
                        if (upDate.UserName != null) user.UserName = upDate.UserName;
                        if (upDate.UpdataPassword != null) user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, upDate.UpdataPassword);
                        if (upDate.Email != null) user.Email = upDate.Email;
                        if (upDate.PhoneNumber != null) user.PhoneNumber = upDate.PhoneNumber;
                        if (upDate.UserGender != null) user.UserGender = upDate.UserGender;
                        if (upDate.UserAddress != null) user.UserAddress = upDate.UserAddress;
                        if (upDate.UserDateOfBirth != null) user.UserDateOfBirth = (DateTime)upDate.UserDateOfBirth;
                        if (upDate.UserDateOfRetirement != null) user.UserDateOfRetirement = upDate.UserDateOfRetirement;

                        user.UserUpdataDate = DateTime.Now;
                        user.UserUpdataUser = user.UserName;
                        //Userデータを更新
                        var update = await _userManager.UpdateAsync(user);
                        if (update.Succeeded)
                        {
                            if (upDate.Role != null)
                            {
                                //Userデータのロールを取得
                                var role = await _userManager.GetRolesAsync(user);
                                //Userデータのロールを削除
                                var remove = await _userManager.RemoveFromRolesAsync(user, role);
                                if (remove.Succeeded)
                                {
                                    //UpDateのロールをUserデータに追加
                                    var add = await _userManager.AddToRoleAsync(user, upDate.Role);
                                    if (add.Succeeded)
                                    {
                                        //Userデータのロールを取得
                                        var roles = await _userManager.GetRolesAsync(user);
                                        //トークンを生成
                                        return Ok(_tokenService.CreateToken(user, roles));
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return StatusCode(500, add.Errors);
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return StatusCode(500, remove.Errors);
                                }
                            }
                            else
                            {
                                //Userデータのロールを取得
                                var roles = await _userManager.GetRolesAsync(user);
                                //トークンを生成
                                return Ok(_tokenService.CreateToken(user, roles));
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return StatusCode(500, update.Errors);
                        }


                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return StatusCode(500, e);
                    }
            }
        }




        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName.ToLower());


                if (user == null) return Unauthorized("Invalid username!");

                var userid = await _userManager.GetUserIdAsync(user);
                if (userid == null) return Unauthorized("Invalid userid!");

                var role = await _userService.GetUserRolesAsync(userid);


                var result = await _signinManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

                if (CheckList((List<string>)role, DeviceRoole))
                {
                    var refreshToken = _tokenService.CreateRefreshToken();
                    _tokenService.SaveRefreshToken(refreshToken, user.Id);
                    var accsesstoken = _tokenService.CreateToken(user, role);
                    return Ok(new { accsesstoken, refreshToken });
                }

                var accessToken = _tokenService.CreateToken(user, role);
                return Ok(new { accessToken });


            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        //list同士で重複しているか確認
        private bool CheckList(List<string> list1, List<string> list2)
        {
            foreach (var item in list1)
            {
                if (list2.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _signinManager.SignOutAsync();
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
                return Ok("Logged out");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);

            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            var principal = _tokenService.VerifyToken(model.AccessToken);
            if (principal == null)
            {
                return Unauthorized("Invalid access token");
            }
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null)
            {
                return Unauthorized("Invalidusername!");
            }
            var isValid = _tokenService.VerifyRefreshToken(model.RefreshToken, user);
            if (!isValid)
            {
                return Unauthorized("Invalidrefresh token");
            }
            var accessToken = _tokenService.CreateToken(user, principal.Claims.Select(c => c.Value).ToList());
            var refreshToken = _tokenService.CreateRefreshToken();
            _tokenService.SaveRefreshToken(refreshToken, user.Id);
            return Ok(new { accessToken, refreshToken });

        }

    }
    public class GetUsers
    {
        public required string Role { get; set; }
    }

    public class GetUser
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? UserGender { get; set; }
        public string? UserAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }

    public class postUser
    {
        public required string UserName { get; set; }
        //ユーザー名
        public required string Email { get; set; }
        //メールアドレス
        public required string Password { get; set; }
        //パスワード
        public required string Role { get; set; }
        //ロール
        public required string PhoneNumber { get; set; }
        //電話番号
        public required DateTime UserDateOfBirth { get; set; }
        //生年月日
        public required string UserGender { get; set; }
        //性別
        public required string UserAddress { get; set; }
        //住所
        public required DateTime UserDateOfEntry { get; set; }
        //入社日
    }

    public class UpDateUser
    {
        public required string ID { get; set; }
        //ユーザーID
        public string? UserName { get; set; }
        //ユーザー名
        public string? Email { get; set; }
        //メールアドレス
        public required string Password { get; set; }
        //パスワード
        public string? UpdataPassword { get; set; }
        //更新パスワード
        public string? Role { get; set; }
        //ロール
        public string? PhoneNumber { get; set; }
        //電話番号
        public DateTime? UserDateOfBirth { get; set; }
        //生年月日
        public string? UserGender { get; set; }
        //性別
        public string? UserAddress { get; set; }
        //住所
        public DateTime? UserDateOfRetirement { get; set; }
        //退職日


    }


    public class returnUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserNameKana { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public IList<string> Role { get; set; }
    }

    public class LoginUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RefreshTokenRequest
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
