using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MF2024_API.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MF2024_API.Method
{

    public class Nfcallotments
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Nfcallotments(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件でNFC割当情報を検索します。
        /// </summary>
        public async Task<List<Nfcallotment>> GetNfcallotmentProcess(GetNfcallotment getNfcallotment)
        {
            try
            {
                // NFC割当情報のクエリを初期化
                var Query = _context.Nfcallotments.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getNfcallotment.NfcallotmentID != null)
                    Query = Query.Where(x => x.NfcallotmentId == getNfcallotment.NfcallotmentID);
                if (getNfcallotment.NfcState != null)
                    Query = Query.Where(x => x.State == getNfcallotment.NfcState);
                if (getNfcallotment.NfcUid != null)
                    Query = Query.Where(x => x.Nfc != null && x.Nfc.NfcUid == getNfcallotment.NfcUid);
                if (getNfcallotment.NfcId != null)
                    Query = Query.Where(x => x.NfcId == getNfcallotment.NfcId);
                if (getNfcallotment.ReservationId != null)
                    Query = Query.Where(x => x.ReservationId == getNfcallotment.ReservationId);
                if (getNfcallotment.NoReservationId != null)
                    Query = Query.Where(x => x.NoReservationId == getNfcallotment.NoReservationId);
                if (getNfcallotment.UserId != null)
                    Query = Query.Where(x => x.UserId == getNfcallotment.UserId);
                if (getNfcallotment.NfcallotmentAddUserName != null)
                    Query = Query.Where(x => x.AddUser.UserName == getNfcallotment.NfcallotmentAddUserName);
                if (getNfcallotment.NfcallotmentUpdateUserName != null)
                    Query = Query.Where(x => x.UpdateUser.UserName == getNfcallotment.NfcallotmentUpdateUserName);
                if (getNfcallotment.AllotmentTimeStart != null)
                    Query = Query.Where(x => x.AllotmentTime >= getNfcallotment.AllotmentTimeStart);
                if (getNfcallotment.AllotmentTimeEnd != null)
                    Query = Query.Where(x => x.AllotmentTime <= getNfcallotment.AllotmentTimeEnd);

                // クエリを実行し、結果をリストで取得
                var reselt = await Query.ToListAsync();
                return reselt;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 指定された内容でNFC割当情報を更新します。
        /// </summary>
        public async Task<Nfcallotment> PutNfcallotmentProcess(PutNfcallotment putNfcallotment)
        {
            // 指定IDのNFC割当情報を取得
            var nfcallotment = await _context.Nfcallotments.FindAsync(putNfcallotment.NfcallotmentID);
            if (nfcallotment == null)
                throw new Exception("Nfcallotmentが見つかりません");

            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // 割当情報を更新
            nfcallotment.State = putNfcallotment.NfcState;
            nfcallotment.UpdateTime = DateTime.Now;
            nfcallotment.UpdateUserId = UserID;

            // エンティティの状態を変更
            _context.Entry(nfcallotment).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return nfcallotment;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!NfcallotmentExists(nfcallotment.NfcallotmentId))
                    throw new Exception("Nfcallotmentが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しいNFC割当情報を登録します。
        /// </summary>
        public async Task<Nfcallotment> PostNfcallotmentProcess(PostNfcallotment postNfcallotment)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // NFC UIDからNFCエンティティを取得
            var NfcID = _context.Nfcs.Where(x => x.NfcUid == postNfcallotment.NfcUid).FirstOrDefault();
            if (NfcID == null)
                throw new Exception("NFCが見つかりません");

            // 新しいNFC割当エンティティを作成
            Nfcallotment nfcallotment = new Nfcallotment
            {
                State = postNfcallotment.NfcState,
                NfcId = NfcID.NfcId,
                ReservationId = postNfcallotment.ReservationId,
                NoReservationId = postNfcallotment.NoReservationId,
                UserId = postNfcallotment.UserId,
                AddTime = DateTime.Now,
                AddUserId = UserID,
                UpdateTime = DateTime.Now,
                UpdateUserId = UserID,
                AllotmentTime = DateTime.Now
            };

            // DBに追加
            _context.Nfcallotments.Add(nfcallotment);

            await _context.SaveChangesAsync();
            return nfcallotment;
        }

        /// <summary>
        /// 指定されたIDのNFC割当情報を削除します。
        /// </summary>
        public async Task<bool> DeleteNfcallotmentProcess(int id)
        {
            // 指定IDのNFC割当情報を取得
            var nfcallotment = await _context.Nfcallotments.FindAsync(id);
            if (nfcallotment == null)
                throw new Exception("Nfcallotmentが見つかりません");

            // NFC割当情報を削除
            _context.Nfcallotments.Remove(nfcallotment);
            await _context.SaveChangesAsync();
            return true;
        }

        // 指定IDのNFC割当が存在するか確認
        private bool NfcallotmentExists(int id)
        {
            return _context.Nfcallotments.Any(e => e.NfcallotmentId == id);
        }
    }
    /// <summary>
    /// NFC割当情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostNfcallotment
    {
        /// <summary>
        /// NFC状態
        /// </summary>
        public required int NfcState { get; set; }
        /// <summary>
        /// NFC UID
        /// </summary>
        public required string NfcUid { get; set; }
        /// <summary>
        /// 予約ID
        /// </summary>
        public int? ReservationId { get; set; }
        /// <summary>
        /// 非予約ID
        /// </summary>
        public int? NoReservationId { get; set; }
        /// <summary>
        /// ユーザーID
        /// </summary>
        public string? UserId { get; set; }
    }
    /// <summary>
    /// NFC割当情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetNfcallotment
    {
        /// <summary>
        /// NFC割当ID
        /// </summary>
        public int? NfcallotmentID { get; set; }
        /// <summary>
        /// 割当日時（開始）
        /// </summary>
        public DateTime? AllotmentTimeStart { get; set; }
        /// <summary>
        /// 割当日時（終了）
        /// </summary>
        public DateTime? AllotmentTimeEnd { get; set; }
        /// <summary>
        /// NFC状態
        /// </summary>
        public int? NfcState { get; set; }
        /// <summary>
        /// NFC UID
        /// </summary>
        public string? NfcUid { get; set; }
        /// <summary>
        /// NFC ID
        /// </summary>
        public int? NfcId { get; set; }
        /// <summary>
        /// 予約ID
        /// </summary>
        public int? ReservationId { get; set; }
        /// <summary>
        /// 非予約ID
        /// </summary>
        public int? NoReservationId { get; set; }
        /// <summary>
        /// ユーザーID
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        public string? NfcallotmentAddUserName { get; set; }
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        public string? NfcallotmentUpdateUserName { get; set; }
    }
    /// <summary>
    /// NFC割当情報の更新時に使用するクラスです。
    /// </summary>
    public class PutNfcallotment
    {
        /// <summary>
        /// NFC割当ID
        /// </summary>
        public required int NfcallotmentID { get; set; }
        /// <summary>
        /// NFC状態
        /// </summary>
        public required int NfcState { get; set; }
    }
}
