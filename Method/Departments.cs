
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

    public class Departments
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Departments(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件で部署情報を検索します。
        /// </summary>
        public async Task<List<Department>> GetDepartmentProcess(GetDepartment getDepartment)
        {
            try
            {
                // 部署情報のクエリを初期化
                var Query = _context.Departments.AsQueryable();

                // 検索条件が指定されていればクエリに追加
                if (getDepartment.DepartmentId != null)
                    Query = Query.Where(x => x.DepartmentId == getDepartment.DepartmentId);

                if (getDepartment.DepartmentName != null)
                    Query = Query.Where(x => x.DepartmentName == getDepartment.DepartmentName);

                if (getDepartment.DepartmentNameKana != null)
                    Query = Query.Where(x => x.DepartmentNameKana == getDepartment.DepartmentNameKana);

                if (getDepartment.OfficeId != null)
                    Query = Query.Where(x => x.OfficeId == getDepartment.OfficeId);

                if (getDepartment.DepartmentFlag != null)
                    Query = Query.Where(x => x.DepartmentFlag == getDepartment.DepartmentFlag);

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
        /// 指定された内容で部署情報を更新します。
        /// </summary>
        public async Task<Department> PutDepartmentProcess(PutDepartment Putdepartment)
        {
            // 指定IDの部署情報を取得
            var Department = await _context.Departments.FindAsync(Putdepartment.DepartmentId);

            if (Department == null)
                throw new Exception("Departmentが見つかりません");

            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // 部署情報を更新
            Department.DepartmentName = Putdepartment.DepartmentName;
            Department.DepartmentNameKana = Putdepartment.DepartmentNameKana;
            Department.OfficeId = Putdepartment.OfficeId;
            Department.DepartmentUpDateUserID = UserID;
            Department.DepartmentUpDateTime = DateTime.Now;

            // DiscordURLが指定されていれば更新
            if (Putdepartment.DiscordURL != null)
                Department.DiscordURL = Putdepartment.DiscordURL;

            // 部署フラグが指定されていれば更新
            if (Putdepartment.DepartmentFlag != null)
                Department.DepartmentFlag = (int)Putdepartment.DepartmentFlag;

            // エンティティの状態を変更
            _context.Entry(Department).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return Department;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!DepartmentExists(Putdepartment.DepartmentId))
                    throw new Exception("Departmentが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しい部署情報を登録します。
        /// </summary>
        public async Task<Department> PostDepartmentProcess(PostDepartment Postdepartment)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // 新しい部署エンティティを作成
            var department = new Department
            {
                DepartmentName = Postdepartment.DepartmentName,
                DepartmentNameKana = Postdepartment.DepartmentNameKana,
                OfficeId = Postdepartment.OfficeId,
                DepartmentAddUserID = UserID,
                DepartmentAddTime = DateTime.Now,
                DepartmentUpDateUserID = UserID,
                DepartmentUpDateTime = DateTime.Now,
                DepartmentFlag = 0
            };

            // DBに追加
            _context.Departments.Add(department);

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return department;
            }
            catch (DbUpdateException e)
            {
                // 既に存在する場合の例外処理
                if (DepartmentExists(department.DepartmentId))
                    throw new Exception("Departmentが重複しています");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 指定されたIDの部署情報を削除します。
        /// </summary>
        public async Task<bool> DeleteDepartmentProcess(int id)
        {
            // 指定IDの部署情報を取得
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                throw new Exception("Departmentが見つかりません");

            // 部署情報を削除
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return true;
        }

        // 指定IDの部署が存在するか確認
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
    /// <summary>
    /// 部署情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetDepartment
    {
        /// <summary>
        /// 部署ID
        /// </summary>
        public int? DepartmentId { get; set; }
        /// <summary>
        /// 部署名
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 部署名カナ
        /// </summary>
        public string? DepartmentNameKana { get; set; }
        /// <summary>
        /// 所属オフィスID
        /// </summary>
        public int? OfficeId { get; set; }
        /// <summary>
        /// 部署フラグ
        /// </summary>
        public int DepartmentFlag { get; set; }
    }
    /// <summary>
    /// 部署情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostDepartment
    {
        /// <summary>
        /// 部署名
        /// </summary>
        public required string DepartmentName { get; set; }
        /// <summary>
        /// 部署名カナ
        /// </summary>
        public required string DepartmentNameKana { get; set; }
        /// <summary>
        /// DiscordのURL
        /// </summary>
        public string? DiscordURL { get; set; }
        /// <summary>
        /// 所属オフィスID
        /// </summary>
        public required int OfficeId { get; set; }
    }
    /// <summary>
    /// 部署情報の更新時に使用するクラスです。
    /// </summary>
    public class PutDepartment
    {
        /// <summary>
        /// 部署ID
        /// </summary>
        public required int DepartmentId { get; set; }
        /// <summary>
        /// 部署名
        /// </summary>
        public required string DepartmentName { get; set; }
        /// <summary>
        /// 部署名カナ
        /// </summary>
        public required string DepartmentNameKana { get; set; }
        /// <summary>
        /// DiscordのURL
        /// </summary>
        public string? DiscordURL { get; set; }
        /// <summary>
        /// 所属オフィスID
        /// </summary>
        public required int OfficeId { get; set; }
        /// <summary>
        /// 部署フラグ
        /// </summary>
        public int? DepartmentFlag { get; set; }
    }
}
