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
    public class Sections
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Sections(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件で課（セクション）情報を検索します。
        /// </summary>
        /// <param name="getSection">検索条件を格納した <see cref="GetSection"/> オブジェクト</param>
        /// <returns>条件に一致する課情報のリスト <see cref="List{Section}"/></returns>
        /// <exception cref="Exception">検索処理中にエラーが発生した場合</exception>
        /// <see cref="Section"/>
        public async Task<List<Section>> GetSectionProcess(GetSection getSection)
        {
            try
            {
                // 検索クエリの初期化
                var query = _context.Sections.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getSection.SectionId != null)
                {
                    query = query.Where(x => x.SectionId == getSection.SectionId);
                }
                if (getSection.SectionName != null)
                {
                    query = query.Where(x => x.SectionName == getSection.SectionName);
                }
                if (getSection.SectionDepartmentID != null)
                {
                    query = query.Where(x => x.DepartmentId == getSection.SectionDepartmentID);
                }
                if (getSection.SectionFlag != null)
                {
                    query = query.Where(x => x.SectionFlag == getSection.SectionFlag);
                }

                // クエリを実行し、結果をリストで取得
                var section = await query.ToListAsync();
                return section;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 指定された内容で課（セクション）情報を更新します。
        /// </summary>
        /// <param name="putSection">更新内容を格納した <see cref="PutSection"/> オブジェクト</param>
        /// <returns>更新後の課情報 <see cref="Section"/></returns>
        /// <exception cref="Exception">対象が存在しない場合や更新時にエラーが発生した場合</exception>
        /// <see cref="Section"/>
        public async Task<Section> PutSectionProcess(PutSection putSection)
        {
            // 指定IDのセクションを取得
            var section = await _context.Sections.FindAsync(putSection.SectionId);
            if (section == null)
            {
                throw new Exception("Sectionが見つかりません");
            }

            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
            {
                throw new Exception("ユーザーが見つかりません");
            }

            // セクション情報を更新
            section.SectionName = putSection.SectionName;
            section.SectionNameKana = putSection.SectionNameKana;
            section.DepartmentId = putSection.DepartmentID;
            section.SectionUpDateUserID = UserID;
            section.SectionUpDateTime = DateTime.Now;

            // オプション項目の更新
            if (putSection.DiscordURL != null)
            {
                section.DiscordURL = putSection.DiscordURL;
            }

            // エンティティの状態を変更
            _context.Entry(section).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return section;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!SectionExists(section.SectionId))
                {
                    throw new Exception("Sectionが見つかりません");
                }
                else
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 新しい課（セクション）情報を登録します。
        /// </summary>
        /// <param name="postSection">登録内容を格納した <see cref="PostSection"/> オブジェクト</param>
        /// <returns>登録された課情報 <see cref="Section"/></returns>
        /// <exception cref="Exception">登録時にエラーが発生した場合</exception>
        /// <see cref="Section"/>
        public async Task<Section> PostSectionProcess(PostSection postSection)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
            {
                throw new Exception("ユーザーが見つかりません");
            }

            // 新しいセクションエンティティを作成
            var section = new Section
            {
                SectionName = postSection.SectionName,
                SectionNameKana = postSection.SectionNameKana,
                DepartmentId = postSection.DepartmentID,
                SectionAddUserID = UserID,
                SectionAddTime = DateTime.Now,
                SectionUpDateUserID = UserID,
                SectionUpDateTime = DateTime.Now,
                SectionFlag = 0
            };

            // オプション項目の設定
            if (postSection.DiscordURL != null)
            {
                section.DiscordURL = postSection.DiscordURL;
            }

            // DBに追加
            _context.Sections.Add(section);
            try
            {
                await _context.SaveChangesAsync();
                return section;
            }
            catch (DbUpdateException)
            {
                // 既に存在する場合の例外処理
                if (SectionExists(section.SectionId))
                {
                    throw new Exception("Sectionが見つかりません");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 指定されたIDの課（セクション）情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の課ID</param>
        /// <returns>削除に成功した場合は <see cref="bool"/>（true）</returns>
        /// <exception cref="Exception">対象が存在しない場合や削除時にエラーが発生した場合</exception>
        /// <see cref="Section"/>
        public async Task<bool> DeleteSectionProcess(int id)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (UserID == null)
                {
                    throw new Exception("ユーザーが見つかりません");
                }
                // 指定IDのセクションを取得
                var section = await _context.Sections.FindAsync(id);
                if (section == null)
                {
                    throw new Exception("Sectionが見つかりません");
                }

                // セクションを削除
                section.SectionFlag = 1; // 論理削除フラグを立てる
                section.SectionUpDateUserID = UserID;
                section.SectionUpDateTime = DateTime.Now;
                _context.Entry(section).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                // その他の例外はそのまま再スロー
                throw e;
            }
        }

        // 指定IDのセクションが存在するか確認
        private bool SectionExists(int id)
        {
            return _context.Sections.Any(e => e.SectionId == id);
        }
    }
    /// <summary>
    /// 課（セクション）情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetSection
    {
        /// <summary>
        /// 課ID
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// 課名
        /// </summary>
        public string? SectionName { get; set; }
        /// <summary>
        /// 所属部署ID
        /// </summary>
        public int? SectionDepartmentID { get; set; }
        /// <summary>
        /// 課フラグ
        /// </summary>
        public int? SectionFlag { get; set; }
    }
    /// <summary>
    /// 課（セクション）情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostSection
    {
        /// <summary>
        /// 課名
        /// </summary>
        public required string SectionName { get; set; }
        /// <summary>
        /// 課名カナ
        /// </summary>
        public required string SectionNameKana { get; set; }
        /// <summary>
        /// 所属部署ID
        /// </summary>
        public required int DepartmentID { get; set; }
        /// <summary>
        /// DiscordのURL
        /// </summary>
        public string? DiscordURL { get; set; }
    }

    /// <summary>
    /// 課（セクション）情報の更新時に使用するクラスです。
    /// </summary>
    public class PutSection
    {
        /// <summary>
        /// 課ID
        /// </summary>
        public required int SectionId { get; set; }
        /// <summary>
        /// 課名
        /// </summary>
        public required string SectionName { get; set; }
        /// <summary>
        /// 課名カナ
        /// </summary>
        public required string SectionNameKana { get; set; }
        /// <summary>
        /// 所属部署ID
        /// </summary>
        public required int DepartmentID { get; set; }
        /// <summary>
        /// DiscordのURL
        /// </summary>
        public string? DiscordURL { get; set; }

    }
}
