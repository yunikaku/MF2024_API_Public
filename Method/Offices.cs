using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MF2024_API.Models;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Authorization;

namespace MF2024_API.Method
{

    public class Offices
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Offices(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件でオフィス情報を検索します。
        /// </summary>
        public async Task<List<Office>> GetOfficeProcess(GetOffice getOffice)
        {
            try
            {
                // オフィス情報のクエリを初期化
                var query = _context.Offices.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getOffice.OfficeId != null)
                    query = query.Where(x => x.OfficeId == getOffice.OfficeId);
                if (getOffice.OfficeName != null)
                    query = query.Where(x => x.OfficeName == getOffice.OfficeName);
                if (getOffice.OfficeNameKana != null)
                    query = query.Where(x => x.OfficeNameKana == getOffice.OfficeNameKana);
                if (getOffice.OfficeLocation != null)
                    query = query.Where(x => x.OfficeLocation == getOffice.OfficeLocation);
                if (getOffice.OfficeAddUserID != null)
                    query = query.Where(x => x.OfficeAddUserID == getOffice.OfficeAddUserID);
                if (getOffice.OfficeAddStratTime != null)
                    query = query.Where(x => x.OfficeAddTime == getOffice.OfficeAddStratTime);
                if (getOffice.OfficeAddEndTime != null)
                    query = query.Where(x => x.OfficeAddTime == getOffice.OfficeAddEndTime);
                if (getOffice.OfficeUpDateUserID != null)
                    query = query.Where(x => x.OfficeUpDateUserID == getOffice.OfficeUpDateUserID);
                if (getOffice.OfficeUpDateTime != null)
                    query = query.Where(x => x.OfficeUpDateTime == getOffice.OfficeUpDateTime);
                if (getOffice.OfficeFlag != null)
                    query = query.Where(x => x.OfficeFlag == getOffice.OfficeFlag);

                // クエリを実行し、結果をリストで取得
                var reselt = await query.ToListAsync();
                return reselt;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 指定された内容でオフィス情報を更新します。
        /// </summary>
        public async Task<Office> PutOfficeProcess(PutOffice putOffice)
        {
            // 指定IDのオフィス情報を取得
            var office = await _context.Offices.FindAsync(putOffice.OfficeId);
            if (office == null)
                throw new Exception("Officeが見つかりません");

            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // オフィス情報を更新
            office.OfficeName = putOffice.OfficeName;
            office.OfficeNameKana = putOffice.OfficeNameKana;
            office.OfficeLocation = putOffice.OfficeLocation;
            office.OfficeUpDateUserID = UserID;
            office.OfficeUpDateTime = DateTime.Now;
            if (putOffice.OfficeFlag != null)
                office.OfficeFlag = (int)putOffice.OfficeFlag;

            // エンティティの状態を変更
            _context.Entry(office).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return office;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!OfficeExists(putOffice.OfficeId))
                    throw new Exception("Officeが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しいオフィス情報を登録します。
        /// </summary>
        public async Task<Office> PostOfficeProcess(PostOffice postOffice)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // 新しいオフィスエンティティを作成
            var office = new Office
            {
                OfficeName = postOffice.OfficeName,
                OfficeNameKana = postOffice.OfficeNameKana,
                OfficeLocation = postOffice.OfficeLocation,
                OfficeAddUserID = UserID,
                OfficeAddTime = DateTime.Now,
                OfficeUpDateUserID = UserID,
                OfficeUpDateTime = DateTime.Now,
                OfficeFlag = 0
            };

            // DBに追加
            _context.Offices.Add(office);

            try
            {
                await _context.SaveChangesAsync();
                return office;
            }
            catch (DbUpdateException)
            {
                // 既に存在する場合の例外処理
                if (OfficeExists(office.OfficeId))
                    throw new Exception("Officeが見つかりません");
                else
                    throw;
            }
        }

        /// <summary>
        /// 指定されたIDのオフィス情報を削除します。
        /// </summary>
        public async Task<bool> DeleteOfficeProcess(int id)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");
            // 指定IDのオフィス情報を取得
            var office = await _context.Offices.FindAsync(id);
            if (office == null)
                throw new Exception("Officeが見つかりません");
            office.OfficeFlag = 1; // フラグを立てて削除済みとする
            office.OfficeUpDateUserID = UserID;
            office.OfficeUpDateTime = DateTime.Now;
            _context.Entry(office).State = EntityState.Modified; // エンティティの状態を変更
            await _context.SaveChangesAsync();
            return true;
        }

        // 指定IDのオフィスが存在するか確認
        private bool OfficeExists(int id)
        {
            return _context.Offices.Any(e => e.OfficeId == id);
        }
    }
    /// <summary>
    /// オフィス情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetOffice
    {
        /// <summary>
        /// オフィスID
        /// </summary>
        public int? OfficeId { get; set; }
        /// <summary>
        /// オフィス名
        /// </summary>
        public string? OfficeName { get; set; }
        /// <summary>
        /// オフィス名カナ
        /// </summary>
        public string? OfficeNameKana { get; set; }
        /// <summary>
        /// オフィス所在地
        /// </summary>
        public string? OfficeLocation { get; set; }
        /// <summary>
        /// 登録ユーザーID
        /// </summary>
        public string? OfficeAddUserID { get; set; }
        /// <summary>
        /// 登録日時（開始）
        /// </summary>
        public DateTime? OfficeAddStratTime { get; set; }
        /// <summary>
        /// 登録日時（終了）
        /// </summary>
        public DateTime? OfficeAddEndTime { get; set; }
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        public string? OfficeUpDateUserID { get; set; }
        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime? OfficeUpDateTime { get; set; }
        /// <summary>
        /// オフィスフラグ
        /// </summary>
        public int? OfficeFlag { get; set; }
    }
    /// <summary>
    /// オフィス情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostOffice
    {
        /// <summary>
        /// オフィス名
        /// </summary>
        public required string OfficeName { get; set; }
        /// <summary>
        /// オフィス名カナ
        /// </summary>
        public required string OfficeNameKana { get; set; }
        /// <summary>
        /// オフィス所在地
        /// </summary>
        public required string OfficeLocation { get; set; }
    }
    /// <summary>
    /// オフィス情報の更新時に使用するクラスです。
    /// </summary>
    public class PutOffice
    {
        /// <summary>
        /// オフィスID
        /// </summary>
        public required int OfficeId { get; set; }
        /// <summary>
        /// オフィス名
        /// </summary>
        public required string OfficeName { get; set; }
        /// <summary>
        /// オフィス名カナ
        /// </summary>
        public required string OfficeNameKana { get; set; }
        /// <summary>
        /// オフィス所在地
        /// </summary>
        public required string OfficeLocation { get; set; }
        /// <summary>
        /// オフィスフラグ
        /// </summary>
        public int? OfficeFlag { get; set; }
    }
}
