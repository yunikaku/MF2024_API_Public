using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MF2024_API.Models;
using MF2024_API.Interfaces;
using MF2024_API.Service;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using MF2024_API.Method;

namespace MF2024_API.Controllers.todo
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Web")]
    public class ReseervationWebController : ControllerBase
    {

        private readonly Mf2024apiDbContext _context;
        private readonly Reservations _reseervationController;
        private readonly Sections _sectionsController;
        private readonly Departments _departmentsController;
        private readonly Offices _officesController;
        private readonly MailService _mailInterfase;
        public ReseervationWebController(Mf2024apiDbContext context, ITokenService tokenService, AESInterfaces aESInterfaces, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _reseervationController = new Reservations(_context, tokenService, aESInterfaces, httpContextAccessor);
            _sectionsController = new Sections(_context, httpContextAccessor);
            _departmentsController = new Departments(_context, httpContextAccessor);
            _officesController = new Offices(_context, httpContextAccessor);
            _mailInterfase = new MailService();

        }
        //予約システム用コントローラー
        //web予約用


        //予約追加用メソッド
        //引数　予約情報
        //戻り値　予約情報
        //コード
        //予約テーブルに予約情報を登録
        //予約情報のメールアドレスに予約情報を通知
        //予約情報を取得して返却

        [HttpPost("PostReservation")]
        public async Task<IActionResult> PostReservation(PostReservation postReservation)
        {
            try
            {
                var result = await _reseervationController.PostReservationProses(postReservation);
                if (result == null)
                {
                    return BadRequest();
                }
                await _mailInterfase.SendMailReservation(new MailModelTemplateReservstion { ReservationID = result.ReservationId, Subject = "予約完了" });
                //メール送信

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //予約変更用メソッド
        //引数　予約ID,予約情報,予約変更ステータス
        //戻り値　予約情報
        //コード
        //予約テーブルから予約IDで予約情報を取得
        //if 予約変更ステータスが変更なら
        //予約テーブルの予約情報を変更
        //メールを送信
        //予約情報を取得して返却
        //else 予約変更ステータスが削除なら
        //予約テーブルの予約情報を削除
        //メールを送信
        //予約情報を取得して返却

        [HttpPut("PutReservation")]
        public async Task<IActionResult> PutReservation(PutReservation putReservation)
        {
            try
            {
                var result = await _reseervationController.PutReservationProcess(putReservation);
                if (result == null)
                {
                    return BadRequest();
                }
                _mailInterfase.SendMailReservation(new MailModelTemplateReservstion { ReservationID = result.ReservationId, Subject = "予約変更" });
                //メール送信
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        //予約者情報取得メソッド
        //予約テーブルから予約者情報を取得するメソッド
        //引数　予約コード
        //戻り値　予約者のリスト
        //コード
        //予約コードから予約テーブルのレコードを取得
        //予約テーブルのレコードを取得して返却

        [HttpGet("GetReservation")]
        public async Task<IActionResult> GetReservation([FromQuery] int ID)
        {
            try
            {
                var result = await _reseervationController.GetReservationProcess(new GetReservation { ReservationId = ID });
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //部署情報取得メソッド
        [HttpGet("GetDepartment")]
        public async Task<IActionResult> GetDepartment([FromQuery] GetDepartment getDepartment)
        {
            try
            {
                var result = await _departmentsController.GetDepartmentProcess(getDepartment);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //課情報取得メソッド
        [HttpGet("GetSection")]
        public async Task<IActionResult> GetSection([FromQuery] GetSection getSection)
        {
            try
            {
                var result = await _sectionsController.GetSectionProcess(getSection);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //事業所情報取得メソッド
        [HttpGet("GetOffice")]
        public async Task<IActionResult> GetOffice([FromQuery] GetOffice getOffice)
        {
            try
            {
                var result = await _officesController.GetOfficeProcess(getOffice);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
