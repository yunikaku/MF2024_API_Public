using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MF2024_API.Models;
using Microsoft.AspNetCore.Authorization;
using MF2024_API.Method;
using Entrants = MF2024_API.Method.Entrants;

namespace MF2024_API.Controllers.todo
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "RoomDevice")]
    public class RoomDeviceController : ControllerBase
    {
        //部屋入室システム用コントローラー
        private readonly OptOuts _optOutsController;
        private readonly Entrants _entrantsController;
        private readonly Mf2024apiDbContext _context;
        private readonly Nfcallotments _nfcallotmentsController;
        private readonly Devices _devicesController;
        public RoomDeviceController(IHttpContextAccessor httpContextAccessor)
        {
            _context = new Mf2024apiDbContext();
            _entrantsController = new Entrants(_context, httpContextAccessor);
            _optOutsController = new OptOuts(_context, httpContextAccessor);
            _nfcallotmentsController = new Nfcallotments(_context, httpContextAccessor);
            _devicesController = new Devices(_context, httpContextAccessor);
        }

        //入室用メソッド
        //引数　デバイスID、NFCUID
        //戻り値 入室しているユーザーのリスト

        //コード
        //デバイスIDから入室者テーブルのレコードを取得
        //NFCallotmentテーブルからNFCUIDでNFC割り当てIDを取得
        //Outputテーブルに入退出情報を登録
        //入室者レコードのListにNFC割り当てIDを追加
        //入室者テーブルを更新
        //入室者テーブルのレコードを取得して返却

        [HttpPost("EnterRoom")]
        public async Task<IActionResult> EnterRoom(EnterEntrants enterEntrants)
        {
            var Entrant = await _entrantsController.EnterEntrantsProcess(enterEntrants);
            if (Entrant == null)
            {
                return BadRequest();
            }
            var OptOut = await _optOutsController.PostOptOutProcess(new PostOptOut { DeviceId = enterEntrants.DeviceID, NfcallotmentId = enterEntrants.NfcallotmentID, OptOutState = 0 });
            if (OptOut == null)
            {
                return BadRequest();
            }
            return Ok(Entrant);
        }

        //退出用メソッド
        //引数　デバイスID、NFCUID
        //戻り値 入室しているユーザーのリスト

        //コード
        //デバイスIDから入室者テーブルのレコードを取得
        //NFCallotmentテーブルからNFCUIDでNFC割り当てIDを取得
        //Outputテーブルに入退出情報を登録
        //入室者レコードのListからNFC割り当てIDを削除
        //入室者テーブルを更新
        //入室者テーブルのレコードを取得して返却

        [HttpPost("ExitRoom")]
        public async Task<IActionResult> ExitRoom(ExitEntrants exitEntrants)
        {
            var Entrant = await _entrantsController.ExitEntrantsProcess(exitEntrants);
            if (Entrant == null)
            {
                return BadRequest();
            }
            var OptOut = await _optOutsController.PostOptOutProcess(new PostOptOut { DeviceId = exitEntrants.DeviceID, NfcallotmentId = exitEntrants.NfcallotmentID, OptOutState = 1 });
            if (OptOut == null)
            {
                return BadRequest();
            }
            return Ok(Entrant);
        }

        //入室者情報取得メソッド
        //引数　デバイスID
        //戻り値 入室しているユーザーのリスト

        //コード
        //デバイスIDから入室者テーブルのレコードを取得
        //入室者テーブルのレコードを取得して返却
        [HttpGet("GetRoomUsers")]
        public async Task<IActionResult> GetRoomUsers([FromQuery] GetEntrants getEntrants)
        {
            var Entrant = await _entrantsController.GetEntrantsProcess(getEntrants);
            if (Entrant == null)
            {
                return BadRequest();
            }
            return Ok(Entrant);
        }

        //NFC情報取得メソッド
        [HttpGet("GetNfcallotment")]
        public async Task<IActionResult> GetNfcallotment([FromQuery] GetNfcallotment getNfcallotment)
        {
            var result = await _nfcallotmentsController.GetNfcallotmentProcess(getNfcallotment);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        //会議室情報取得メソッド
        [HttpGet("GetConferenceRoomData")]
        public async Task<IActionResult> GetConferenceRoomData([FromQuery] int DeviceID)
        {
            var result = await _devicesController.GetDeviceProcess(new GetDevice { DeviceId = DeviceID });
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }



    }
}
