using MF2024_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MF2024_API.Method
{

    public class Entrants
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Entrants(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件で入室情報を検索します。
        /// </summary>
        public async Task<List<Models.Entrants>> GetEntrantsProcess(GetEntrants getEntrants)
        {
            try
            {
                // 入室情報のクエリを初期化
                var Query = _context.Entrants.AsQueryable();

                // 入室情報IDで絞り込み
                if (getEntrants.EntrantsID != null)
                    Query = Query.Where(x => x.EntrantsID == getEntrants.EntrantsID);

                // デバイスIDで絞り込み
                if (getEntrants.DeviceID != null)
                    Query = Query.Where(x => x.DeviceID == getEntrants.DeviceID);

                // 関連エンティティ（Device, Nfcallotments, User, Reservation）も含めて取得
                var result = await Query
                    .Include(x => x.Device)
                    .Include(x => x.Nfcallotments).ThenInclude(x => x.User)
                    .Include(x => x.Nfcallotments).ThenInclude(x => x.Reservation)
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 新しい入室情報を登録します。
        /// </summary>
        public async Task<Models.Entrants> PostEntrantsProcess(PostEntrants postEntrants)
        {
            try
            {
                // 別コンテキストで同じDeviceIDの入室情報が既に存在するか確認
                using (var context1 = new Mf2024apiDbContext())
                {
                    var entrant = await context1.Entrants.FirstOrDefaultAsync(x => x.DeviceID == postEntrants.DeviceID);
                    if (entrant != null)
                        // 既に存在する場合は例外をスロー
                        throw new Exception("入室情報がすでに存在します");

                    // 新しい入室情報エンティティを作成
                    var entrants = new Models.Entrants
                    {
                        DeviceID = postEntrants.DeviceID
                    };

                    // DBに追加
                    context1.Entrants.Add(entrants);
                    await context1.SaveChangesAsync();
                    return entrants;
                }
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 入室処理を行います。
        /// </summary>
        public async Task<Models.Entrants> EnterEntrantsProcess(EnterEntrants enterEntrants)
        {
            try
            {
                // 指定DeviceIDの入室情報を取得し、NFC割当情報も含めて取得
                var entrants = await _context.Entrants.Include(x => x.Nfcallotments)
                    .FirstOrDefaultAsync(x => x.DeviceID == enterEntrants.DeviceID);
                // 指定NFC割当IDのNfcallotmentを取得
                var nfcallotment = await _context.Nfcallotments
                    .FirstOrDefaultAsync(x => x.NfcallotmentId == enterEntrants.NfcallotmentID);

                // どちらかが見つからなければ例外
                if (entrants == null || nfcallotment == null)
                    throw new Exception("入室情報が見つかりません");

                // 入室情報にNFC割当情報を追加（入室処理）
                entrants.Nfcallotments.Add(nfcallotment);

                // DBに保存
                await _context.SaveChangesAsync();
                return entrants;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 退室処理を行います。
        /// </summary>
        public async Task<Models.Entrants> ExitEntrantsProcess(ExitEntrants exitEntrants)
        {
            try
            {
                // 指定DeviceIDの入室情報を取得し、NFC割当情報も含めて取得
                var entrants = await _context.Entrants.Include(x => x.Nfcallotments)
                    .FirstOrDefaultAsync(x => x.DeviceID == exitEntrants.DeviceID);
                // 指定NFC割当IDのNfcallotmentを取得
                var nfcallotment = await _context.Nfcallotments
                    .FirstOrDefaultAsync(x => x.NfcallotmentId == exitEntrants.NfcallotmentID);

                // どちらかが見つからなければ例外
                if (entrants == null || nfcallotment == null)
                    throw new Exception("入室情報が見つかりません");

                // 入室情報からNFC割当情報を削除（退室処理）
                entrants.Nfcallotments.Remove(nfcallotment);

                // DBに保存
                await _context.SaveChangesAsync();
                return entrants;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }
    }
    /// <summary>
    /// 入室情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetEntrants
    {
        /// <summary>
        /// 入室情報ID
        /// </summary>
        public int? EntrantsID { get; set; }
        /// <summary>
        /// デバイスID
        /// </summary>
        public int? DeviceID { get; set; }
    }
    /// <summary>
    /// 入室情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostEntrants
    {
        /// <summary>
        /// デバイスID
        /// </summary>
        public int DeviceID { get; set; }
    }
    /// <summary>
    /// 入室処理時に使用するクラスです。
    /// </summary>
    public class EnterEntrants
    {
        /// <summary>
        /// デバイスID
        /// </summary>
        public int DeviceID { get; set; }
        /// <summary>
        /// NFC割当ID
        /// </summary>
        public int NfcallotmentID { get; set; }
    }
    /// <summary>
    /// 退室処理時に使用するクラスです。
    /// </summary>
    public class ExitEntrants
    {
        /// <summary>
        /// デバイスID
        /// </summary>
        public int DeviceID { get; set; }
        /// <summary>
        /// NFC割当ID
        /// </summary>
        public int NfcallotmentID { get; set; }
    }



}
