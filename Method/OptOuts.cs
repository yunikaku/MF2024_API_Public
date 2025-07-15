using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MF2024_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace MF2024_API.Method
{

    public class OptOuts
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OptOuts(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件でオプトアウト情報を検索します。
        /// </summary>
        public async Task<ActionResult<List<OptOut>>> GetOptOutProcess(GetOptOut getOptOut)
        {
            try
            {
                // オプトアウト情報のクエリを初期化
                var query = _context.OptOuts.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getOptOut.OptOutId != null)
                    query = query.Where(x => x.OptOutId == getOptOut.OptOutId);
                if (getOptOut.DeviceId != null)
                    query = query.Where(x => x.DeviceId == getOptOut.DeviceId);
                if (getOptOut.NfcallotmentId != null)
                    query = query.Where(x => x.NfcallotmentId == getOptOut.NfcallotmentId);
                if (getOptOut.OptOutState != null)
                    query = query.Where(x => x.OptOutState == getOptOut.OptOutState);
                if (getOptOut.OptOutStartTime != null)
                    query = query.Where(x => x.OptOutTime >= getOptOut.OptOutStartTime);
                if (getOptOut.OptOutEndTime != null)
                    query = query.Where(x => x.OptOutTime <= getOptOut.OptOutEndTime);

                // クエリを実行し、結果をリストで取得
                return await query.ToListAsync();
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 指定された内容でオプトアウト情報を更新します。
        /// </summary>
        public async Task<OptOut> PutOptOutProcess(PutOptOut putOptOut)
        {
            // 指定IDのオプトアウト情報を取得
            var optOut = await _context.OptOuts.FindAsync(putOptOut.OptOutId);
            if (optOut == null)
                throw new Exception("OptOutが見つかりません");

            // プロパティを更新
            optOut.DeviceId = putOptOut.DeviceId;
            optOut.NfcallotmentId = putOptOut.NfcallotmentId;
            optOut.OptOutState = putOptOut.OptOutState;

            // エンティティの状態を変更
            _context.Entry(optOut).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return optOut;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!OptOutExists(optOut.OptOutId))
                    throw new Exception("OptOutが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しいオプトアウト情報を登録します。
        /// </summary>
        public async Task<OptOut> PostOptOutProcess(PostOptOut postOptOut)
        {
            // 新しいオプトアウトエンティティを作成
            var optOut = new OptOut
            {
                DeviceId = postOptOut.DeviceId,
                NfcallotmentId = postOptOut.NfcallotmentId,
                OptOutState = postOptOut.OptOutState,
                OptOutTime = DateTime.Now
            };

            // DBに追加
            _context.OptOuts.Add(optOut);

            try
            {
                await _context.SaveChangesAsync();
                return optOut;
            }
            catch (DbUpdateException e)
            {
                // 既に存在する場合の例外処理
                if (OptOutExists(optOut.OptOutId))
                    throw new Exception("OptOutが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 指定されたIDのオプトアウト情報を削除します。
        /// </summary>
        public async Task<bool> DeleteOptOutProcess(int id)
        {
            // 指定IDのオプトアウト情報を取得
            var optOut = await _context.OptOuts.FindAsync(id);
            if (optOut == null)
                throw new Exception("OptOutが見つかりません");

            // オプトアウト情報を削除
            _context.OptOuts.Remove(optOut);
            await _context.SaveChangesAsync();
            return true;
        }

        // 指定IDのオプトアウトが存在するか確認
        private bool OptOutExists(int id)
        {
            return _context.OptOuts.Any(e => e.OptOutId == id);
        }
    }
    /// <summary>
    /// オプトアウト情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetOptOut
    {
        /// <summary>
        /// オプトアウトID
        /// </summary>
        public int? OptOutId { get; set; }
        /// <summary>
        /// デバイスID
        /// </summary>
        public int? DeviceId { get; set; }
        /// <summary>
        /// NFC割当ID
        /// </summary>
        public int? NfcallotmentId { get; set; }
        /// <summary>
        /// オプトアウト状態
        /// </summary>
        public int? OptOutState { get; set; }
        /// <summary>
        /// オプトアウト日時（開始）
        /// </summary>
        public DateTime? OptOutStartTime { get; set; }
        /// <summary>
        /// オプトアウト日時（終了）
        /// </summary>
        public DateTime? OptOutEndTime { get; set; }
    }
    /// <summary>
    /// オプトアウト情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostOptOut
    {
        /// <summary>
        /// デバイスID
        /// </summary>
        public required int DeviceId { get; set; }
        /// <summary>
        /// NFC割当ID
        /// </summary>
        public required int NfcallotmentId { get; set; }
        /// <summary>
        /// オプトアウト状態
        /// </summary>
        public required int OptOutState { get; set; }
    }


    /// <summary>
    /// オプトアウト情報の更新時に使用するクラスです。
    /// </summary>
    public class PutOptOut
    {
        /// <summary>
        /// オプトアウトID
        /// </summary>
        public required int OptOutId { get; set; }
        /// <summary>
        /// デバイスID
        /// </summary>
        public required int DeviceId { get; set; }
        /// <summary>
        /// NFC割当ID
        /// </summary>
        public required int NfcallotmentId { get; set; }
        /// <summary>
        /// オプトアウト状態
        /// </summary>
        public required int OptOutState { get; set; }
    }
}
