using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MF2024_API.Controllers.todo;
using MF2024_API.Models;
using Microsoft.Graph.IdentityGovernance.LifecycleWorkflows.DeletedItems.Workflows.Item.MicrosoftGraphIdentityGovernanceCreateNewVersion;
using MF2024_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MF2024_API.Method;
using Entrants = MF2024_API.Method.Entrants;

namespace MF2024_API.Controllers.todo
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ConferencRoomDevice")]
    public class ConferenceRoomDeviceController : ControllerBase
    {
        private readonly Mf2024apiDbContext _context;
        private readonly ConferenceRoomReservations _conferenceRoomReservationsController;
        private readonly OptOuts _optOutsController;
        private readonly Entrants _entrantsController;
        private readonly Nfcallotments _nfcallotmentsController;
        private readonly Devices _devicesController;
        private readonly MailInterfase mailInterfase;
        private readonly DiscordInterfase _discordInterfase;
        private readonly Nfcs _nfcsController;
        public ConferenceRoomDeviceController(IHttpContextAccessor httpContextAccessor, MailInterfase MailInterfase, DiscordInterfase discordInterfase)
        {
            _context = new Mf2024apiDbContext();
            _conferenceRoomReservationsController = new ConferenceRoomReservations(_context, httpContextAccessor);
            _optOutsController = new OptOuts(_context, httpContextAccessor);
            _entrantsController = new Entrants(_context, httpContextAccessor);
            _devicesController = new Devices(_context, httpContextAccessor);
            mailInterfase = MailInterfase;
            _discordInterfase = discordInterfase;
            _nfcallotmentsController = new Nfcallotments(_context, httpContextAccessor);
            _nfcsController = new Nfcs(_context, httpContextAccessor);

        }


        //会議室予約システム用コントローラー

        //会議室予約用取得メソッド
        /// <summary>
        /// 会議室予約情報を取得するメソッド
        /// </summary>
        /// <param name="getConferenceRoomReservation">GetConferenceRoomReservation</param>
        /// <returns> </returns>
        [HttpGet("GetConferenceRoomReservation")]
        public async Task<ActionResult<ConferenceRoomReservation>> GetConferenceRoomReservation([FromQuery] GetConferenceRoomReservation getConferenceRoomReservation)
        {
            try
            {
                var conferenceRoomReservation = await _conferenceRoomReservationsController.GetConferenceRoomReservationProcess(getConferenceRoomReservation);
                return Ok(conferenceRoomReservation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //会議室予約変更用メソッド
        [HttpPut("PutConferenceRoomReservation")]
        public async Task<ActionResult<ConferenceRoomReservation>> PutConferenceRoomReservation(PutConferenceRoomReservation putConferenceRoomReservation)
        {
            try
            {
                // 予約重複チェック(更新する予約を抜いて)
                var conferenceRoomReservationData = await _conferenceRoomReservationsController.GetConferenceRoomReservationProcess(new GetConferenceRoomReservation
                {
                    DeviceId = putConferenceRoomReservation.DeviceId,
                    StartTime = putConferenceRoomReservation.StartTime,
                    EndTime = putConferenceRoomReservation.EndTime
                });
                conferenceRoomReservationData.RemoveAll(x => x.ConferenceRoomReservationId == putConferenceRoomReservation.ConferenceRoomReservationId);
                if (conferenceRoomReservationData.Count > 0)
                {
                    return BadRequest("予約が重複しています");
                }
                var conferenceRoomReservation = await _conferenceRoomReservationsController.PutConferenceRoomReservationProcess(putConferenceRoomReservation);

                return Ok(conferenceRoomReservation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //会議室予約登録用メソッド
        [HttpPost("PostConferenceRoomReservation")]
        public async Task<ActionResult<ConferenceRoomReservation>> PostConferenceRoomReservation(PostConferenceRoomReservation postConferenceRoomReservation)
        {
            try
            {
                // 予約重複チェック
                var conferenceRoomReservationData = await _conferenceRoomReservationsController.GetConferenceRoomReservationProcess(new GetConferenceRoomReservation
                {
                    DeviceId = postConferenceRoomReservation.DeviceId,
                    StartTime = postConferenceRoomReservation.StartTime,
                    EndTime = postConferenceRoomReservation.EndTime
                });
                if (conferenceRoomReservationData.Count > 0)
                {
                    return BadRequest("予約が重複しています");
                }
                var conferenceRoomReservation = await _conferenceRoomReservationsController.PostConferenceRoomReservationProcess(postConferenceRoomReservation);

                return Ok(conferenceRoomReservation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //会議室予約削除用メソッド
        [HttpDelete("DeleteConferenceRoomReservation")]
        public async Task<IActionResult> DeleteConferenceRoomReservation(int id)
        {
            try
            {
                var conferenceRoomReservation = await _conferenceRoomReservationsController.DeleteConferenceRoomReservationProcess(id);
                return Ok(conferenceRoomReservation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //会議室入室者情報取得メソッド
        [HttpGet("GetRoomUsers")]
        public async Task<ActionResult<List<Models.Entrants>>> GetRoomUsers([FromQuery] GetEntrants getEntrants)
        {
            var Entrants = await _entrantsController.GetEntrantsProcess(getEntrants);
            if (Entrants == null)
            {
                return BadRequest();
            }
            var entrant = Entrants;
            return Ok(entrant);
        }

        //会議室入室用メソッド
        [HttpPost("EnterRoom")]
        public async Task<ActionResult<Models.Entrants>> EnterRoom(EnterEntrants enterEntrants)
        {
            var OptOut = await _optOutsController.PostOptOutProcess(new PostOptOut { DeviceId = enterEntrants.DeviceID, NfcallotmentId = enterEntrants.NfcallotmentID, OptOutState = 0 });
            if (OptOut == null)
            {
                return BadRequest();
            }
            var Entrant = await _entrantsController.EnterEntrantsProcess(enterEntrants);
            if (Entrant == null)
            {
                return BadRequest();
            }

            return Ok(Entrant);
        }

        //会議室退出用メソッド
        [HttpPost("ExitRoom")]
        public async Task<ActionResult<Models.Entrants>> ExitRoom(ExitEntrants exitEntrants)
        {
            var OptOut = await _optOutsController.PostOptOutProcess(new PostOptOut { DeviceId = exitEntrants.DeviceID, NfcallotmentId = exitEntrants.NfcallotmentID, OptOutState = 1 });
            if (OptOut == null)
            {
                return BadRequest();
            }
            var Entrant = await _entrantsController.ExitEntrantsProcess(exitEntrants);
            if (Entrant == null)
            {
                return BadRequest();
            }

            return Ok(Entrant);
        }

        //NFC情報取得メソッド
        [HttpGet("GetNfcallotment")]
        public async Task<ActionResult<List<Nfcallotment>>> GetNfcallotment([FromQuery] GetNfcallotment getNfcallotment)
        {
            try
            {
                var nfcs = await _nfcsController.GetNfcProcess(new GetNfc { NfcUid = getNfcallotment.NfcUid });
                if (nfcs == null)
                {
                    throw new Exception("NFCが見つかりません");
                }
                var nfc = nfcs.FirstOrDefault();

                var result = await _nfcallotmentsController.GetNfcallotmentProcess(new GetNfcallotment { NfcId = nfc.NfcId, NfcState = getNfcallotment.NfcState });
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

        //会議室情報取得メソッド
        [HttpGet("GetConferenceRoomData")]
        public async Task<ActionResult<List<Device>>> GetConferenceRoomData([FromQuery] int DeviceID)
        {
            var result = await _devicesController.GetDeviceProcess(new GetDevice { DeviceId = DeviceID });
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }


        [HttpPost("testmail")]
        public async Task<IActionResult> postmail(MailModelTemplateReservstion mailModelTemplateReservstion)
        {
            try
            {
                var result = await mailInterfase.SendMailReservation(mailModelTemplateReservstion);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("TestDiscord")]
        public async Task<IActionResult> TestDiscord(int sectionID, string message)
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
    }

}
