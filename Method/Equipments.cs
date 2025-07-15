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

    public class Equipments
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Equipments(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件で設備情報を検索します。
        /// </summary>
        public async Task<List<Equipment>> GetEquipmentProcess(GetEquipment getEquipment)
        {
            try
            {
                // 設備情報のクエリを初期化
                var Query = _context.Equipments.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getEquipment.EquipmentID != null)
                    Query = Query.Where(x => x.EquipmentID == getEquipment.EquipmentID);

                if (getEquipment.EquipmentName != null)
                    Query = Query.Where(x => x.EquipmentName.Contains(getEquipment.EquipmentName));

                if (getEquipment.EquipmentFlag != null)
                    Query = Query.Where(x => x.EquipmentFlag == getEquipment.EquipmentFlag);

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
        /// 指定された内容で設備情報を更新します。
        /// </summary>
        public async Task<Equipment> PutEquipmentProcess(PutEquipment putEquipment)
        {
            // 指定IDの設備情報を取得
            var equipment = await _context.Equipments.FindAsync(putEquipment.EquipmentID);
            if (equipment == null)
                throw new Exception("Equipmentが見つかりません");

            // 設備名を更新
            equipment.EquipmentName = putEquipment.EquipmentName;

            // ファイルデータが指定されていればバイト配列として保存
            if (putEquipment.EquipmentData != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await putEquipment.EquipmentData.CopyToAsync(memoryStream);
                    equipment.EquipmentData = memoryStream.ToArray();
                }
            }

            // フラグが指定されていれば更新
            if (putEquipment.EquipmentFlag != null)
                equipment.EquipmentFlag = (int)putEquipment.EquipmentFlag;

            // エンティティの状態を変更
            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return equipment;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!EquipmentExists(equipment.EquipmentID))
                    throw new Exception("Equipmentが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しい設備情報を登録します。
        /// </summary>
        public async Task<Equipment> PostEquipmentProcess(PostEquipment postEquipment)
        {
            // 新しい設備エンティティを作成
            var equipment = new Equipment
            {
                EquipmentName = postEquipment.EquipmentName,
                EquipmentFlag = 0
            };

            // ファイルデータが指定されていればバイト配列として保存
            if (postEquipment.EquipmentData != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await postEquipment.EquipmentData.CopyToAsync(memoryStream);
                    equipment.EquipmentData = memoryStream.ToArray();
                }
            }

            // DBに追加
            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
            return equipment;
        }

        /// <summary>
        /// 指定されたIDの設備情報を削除します。
        /// </summary>
        public async Task<bool> DeleteEquipmentProcess(int id)
        {
            // 指定IDの設備情報を取得
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
                throw new Exception("Equipmentが見つかりません");

            // 設備情報を削除
            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
            return true;
        }

        // 指定IDの設備が存在するか確認
        private bool EquipmentExists(int id)
        {
            return _context.Equipments.Any(e => e.EquipmentID == id);
        }
    }
    /// <summary>
    /// 設備情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostEquipment
    {
        /// <summary>
        /// 設備名
        /// </summary>
        public required string EquipmentName { get; set; }
        /// <summary>
        /// 設備データ（ファイル）
        /// </summary>
        public IFormFile? EquipmentData { get; set; }
    }
    /// <summary>
    /// 設備情報の更新時に使用するクラスです。
    /// </summary>
    public class PutEquipment
    {
        /// <summary>
        /// 設備ID
        /// </summary>
        public required int EquipmentID { get; set; }
        /// <summary>
        /// 設備名
        /// </summary>
        public required string EquipmentName { get; set; }
        /// <summary>
        /// 設備データ（ファイル）
        /// </summary>
        public IFormFile? EquipmentData { get; set; }
        /// <summary>
        /// 設備フラグ
        /// </summary>
        public int? EquipmentFlag { get; set; }
    }
    /// <summary>
    /// 設備情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetEquipment
    {
        /// <summary>
        /// 設備ID
        /// </summary>
        public int? EquipmentID { get; set; }
        /// <summary>
        /// 設備名
        /// </summary>
        public string? EquipmentName { get; set; }
        /// <summary>
        /// 設備フラグ
        /// </summary>
        public int? EquipmentFlag { get; set; }
    }
}
