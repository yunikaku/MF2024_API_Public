using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MF2024_API.Models;

using MF2024_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MF2024_API.Method;

namespace MF2024_API.Controllers.todo
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Reseption")]
    public class ReceptionDeviceController : ControllerBase
    {
        private readonly Mf2024apiDbContext _context;
        private readonly Reservations _reseervationController;

        private readonly Nfcallotments _nfcallotmentsController;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DiscordInterfase _discordInterfase;
        private readonly Departments _departmentsController;
        private readonly Sections _sectionsController;
        private readonly Offices _officesController;
        public ReceptionDeviceController(ITokenService tokenService, AESInterfaces aESInterfaces, IHttpContextAccessor httpContextAccessor, DiscordInterfase discordInterfase)
        {
            _context = new Mf2024apiDbContext();
            _httpContextAccessor = httpContextAccessor;
            _reseervationController = new Reservations(_context, tokenService, aESInterfaces, httpContextAccessor);
            _nfcallotmentsController = new Nfcallotments(_context, httpContextAccessor);
            _discordInterfase = discordInterfase;
            _departmentsController = new Departments(_context, httpContextAccessor);
            _sectionsController = new Sections(_context, httpContextAccessor);
            _officesController = new Offices(_context, httpContextAccessor);

        }
        //受付システム用コントローラー

        //受付用予約データ取得メソッド(GET)
        [HttpGet("GetReservation")]
        public async Task<IActionResult> GetReservation([FromQuery] GetReservation getReservation)
        {
            try
            {
                var result = await _reseervationController.GetReservationProcess(getReservation);
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
        //受付用予約データ変更メソッド(PUT)
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
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //受付用予約データ作成ソッド(POST)
        [HttpPost("PostReservation")]
        public async Task<IActionResult> PostReservation(PostReservation postReservation)
        {
            try
            {
                var reselt = await _reseervationController.PostReservationProses(postReservation);
                return Ok(reselt);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //受付用NFC割り当てデータ作成メソッド（POST）
        [HttpPost("PostNfcallotment")]
        public async Task<IActionResult> PostNfcallotment(PostNfcallotment postNfcallotment)
        {
            try
            {
                var result = await _nfcallotmentsController.PostNfcallotmentProcess(postNfcallotment);
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
        //受付用NFC割り当てデータ変更メソッド（PUT）
        [HttpPut("PutNfcallotment")]
        public async Task<IActionResult> PutNfcallotment(PutNfcallotment putNfcallotment)
        {
            try
            {
                var result = await _nfcallotmentsController.PutNfcallotmentProcess(putNfcallotment);
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
        //受付用NFC割り当てデータ取得メソッド（GET）
        [HttpGet("GetNfcallotment")]
        public async Task<IActionResult> GetNfcallotment([FromQuery] string UID)
        {
            try
            {
                var nfcchek = _context.Nfcs.Any(x => x.NfcUid == UID);
                if (!nfcchek)
                {
                    return BadRequest("NFCが登録されていません");
                }
                var result = await _nfcallotmentsController.GetNfcallotmentProcess(new GetNfcallotment { NfcUid = UID, NfcState = 0 });
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("DiscordSend")]
        public async Task<IActionResult> DiscordSend(int sectionID, string message)
        {
            try
            {
                var result = await _discordInterfase.Discordsend(sectionID, message);
                if (!result)
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

        [HttpPost("DiscordSendReservation")]
        public async Task<IActionResult> DiscordSendReservation(int sectionID, int ReservationID)
        {
            try
            {
                var result = await _discordInterfase.DiscordReservationSend(sectionID, ReservationID);
                if (!result)
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
