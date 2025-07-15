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

    public class Nfcs
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Nfcs(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件でNFC情報を検索します。
        /// </summary>
        public async Task<List<Nfc>> GetNfcProcess(GetNfc getNfc)
        {
            try
            {
                // NFC情報のクエリを初期化
                var Query = _context.Nfcs.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getNfc.NfcID != null)
                    Query = Query.Where(x => x.NfcId == getNfc.NfcID);
                if (getNfc.NfcState != null)
                    Query = Query.Where(x => x.NfcState == getNfc.NfcState);
                if (getNfc.NfcUid != null)
                    Query = Query.Where(x => x.NfcUid == getNfc.NfcUid);
                if (getNfc.NfcAddUserName != null)
                    Query = Query.Where(x => x.AddUser.UserName == getNfc.NfcAddUserName);
                if (getNfc.NfcUpdateUserName != null)
                    Query = Query.Where(x => x.UpdateUser.UserName == getNfc.NfcUpdateUserName);

                // クエリを実行し、結果をリストで取得
                return await Query.ToListAsync();
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 指定された内容でNFC情報を更新します。
        /// </summary>
        public async Task<Nfc> PutNfcProcess(PutNfc putNfc)
        {
            // 指定IDのNFC情報を取得
            var nfc = await _context.Nfcs.FindAsync(putNfc.NfcID);
            if (nfc == null)
                throw new Exception("Nfcが見つかりません");

            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // NFC情報を更新
            nfc.NfcState = putNfc.NfcState;
            nfc.NfcUid = putNfc.NfcUid;
            nfc.NfcUpdateTime = DateTime.Now;
            nfc.NfcUpdateUserID = UserID;

            // エンティティの状態を変更
            _context.Entry(nfc).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return nfc;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!NfcExists(putNfc.NfcID))
                    throw new Exception("Nfcが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しいNFC情報を登録します。
        /// </summary>
        public async Task<Nfc> PostNfcProcess(PostNfc postNfc)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // 新しいNFCエンティティを作成
            var nfc = new Nfc
            {
                NfcState = postNfc.NfcState,
                NfcUid = postNfc.NfcUid,
                NfcAddTime = DateTime.Now,
                NfcAddUserID = UserID,
                NfcUpdateTime = DateTime.Now,
                NfcUpdateUserID = UserID
            };

            // DBに追加
            _context.Nfcs.Add(nfc);

            try
            {
                await _context.SaveChangesAsync();
                return nfc;
            }
            catch (DbUpdateException e)
            {
                // 既に存在する場合の例外処理
                if (NfcExists(nfc.NfcId))
                    throw new Exception("Nfcが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 指定されたIDのNFC情報を削除します。
        /// </summary>
        public async Task<bool> DeleteNfcProcess(int id)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");
            // 指定IDのNFC情報を取得
            var nfc = await _context.Nfcs.FindAsync(id);
            if (nfc == null)
                throw new Exception("Nfcが見つかりません");
            nfc.NfcState = 1; // 削除状態に変更
            nfc.NfcUpdateTime = DateTime.Now;
            nfc.NfcUpdateUserID = UserID;
            // エンティティの状態を変更
            _context.Entry(nfc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        // 指定IDのNFCが存在するか確認
        private bool NfcExists(int id)
        {
            return _context.Nfcs.Any(e => e.NfcId == id);
        }
    }
    /// <summary>
    /// NFC情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetNfc
    {
        /// <summary>
        /// NFC ID
        /// </summary>
        public int? NfcID { get; set; }
        /// <summary>
        /// NFCの状態
        /// </summary>
        public int? NfcState { get; set; }
        /// <summary>
        /// NFCのUID
        /// </summary>
        public string? NfcUid { get; set; }
        /// <summary>
        /// 登録ユーザー名
        /// </summary>
        public string? NfcAddUserName { get; set; }
        /// <summary>
        /// 更新ユーザー名
        /// </summary>
        public string? NfcUpdateUserName { get; set; }
    }
    /// <summary>
    /// NFC情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostNfc
    {
        /// <summary>
        /// NFCのUID
        /// </summary>
        public required string NfcUid { get; set; }
        /// <summary>
        /// NFCの状態
        /// </summary>
        public required int NfcState { get; set; }
    }
    /// <summary>
    /// NFC情報の更新時に使用するクラスです。
    /// </summary>
    public class PutNfc
    {
        /// <summary>
        /// NFC ID
        /// </summary>
        public required int NfcID { get; set; }
        /// <summary>
        /// NFCの状態
        /// </summary>
        public required int NfcState { get; set; }
        /// <summary>
        /// NFCのUID
        /// </summary>
        public required string NfcUid { get; set; }
    }
}
