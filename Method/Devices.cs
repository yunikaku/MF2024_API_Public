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

    public class Devices
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Entrants _entrantsController;

        public Devices(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _entrantsController = new Entrants(context, httpContextAccessor);
        }

        /// <summary>
        /// 指定された条件でデバイス情報を検索します。
        /// </summary>
        public async Task<List<Device>> GetDeviceProcess(GetDevice getDevice)
        {
            try
            {
                // デバイス情報のクエリを初期化
                var Query = _context.Devices.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getDevice.DeviceId != null)
                    Query = Query.Where(x => x.DeviceId == getDevice.DeviceId);

                if (getDevice.DeviceName != null)
                    Query = Query.Where(x => x.DeviceName == getDevice.DeviceName);

                if (getDevice.DeviceLocation != null)
                    Query = Query.Where(x => x.DeviceLocation == getDevice.DeviceLocation);

                if (getDevice.DeviceCategory != null)
                    Query = Query.Where(x => x.DeviceCategory == getDevice.DeviceCategory);

                if (getDevice.DeviceAddUserName != null)
                    Query = Query.Where(x => x.AddUser.UserName == getDevice.DeviceAddUserName);

                if (getDevice.DeviceUpdateUserName != null)
                    Query = Query.Where(x => x.UpdateUser.UserName == getDevice.DeviceUpdateUserName);

                if (getDevice.RoomId != null)
                    Query = Query.Where(x => x.RoomId == getDevice.RoomId);

                if (getDevice.OfficeId != null)
                    Query = Query.Where(x => x.Room.OfficeId == getDevice.OfficeId);

                if (getDevice.DeviceFlag != null)
                    Query = Query.Where(x => x.DeviceFlag == getDevice.DeviceFlag);

                // 部屋情報も含めて取得
                var result = await Query.Include(x => x.Room).ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 指定された内容でデバイス情報を更新します。
        /// </summary>
        public async Task<Device> PutDeviceProcess(PutDevice putDevice)
        {
            // DeviceIdがnullの場合は例外
            if (putDevice.DeviceId == null)
                throw new Exception("Deviceが見つかりません");

            // 指定IDのデバイス情報を取得
            var device = await _context.Devices.FindAsync(putDevice.DeviceId);
            if (device == null)
                throw new Exception("Deviceが見つかりません");

            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // デバイス情報を更新
            device.DeviceName = putDevice.DeviceName;
            device.DeviceLocation = putDevice.DeviceLocation;
            device.DeviceCategory = putDevice.DeviceCategory;
            device.RoomId = putDevice.RoomId;
            device.DeviceUpdateUserID = UserID;
            device.DeviceUpDateTime = DateTime.Now;
            device.DeviceFlag = putDevice.DeviceFlag;

            // エンティティの状態を変更
            _context.Entry(device).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return device;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!DeviceExists(putDevice.DeviceId))
                    throw new Exception("Deviceが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しいデバイス情報を登録します。
        /// </summary>
        public async Task<Device> PostDeviceProcess(PostDevice postDevice)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // 新しいデバイスエンティティを作成
            Device device = new Device
            {
                DeviceName = postDevice.DeviceName,
                DeviceLocation = postDevice.DeviceLocation,
                DeviceCategory = postDevice.DeviceCategory,
                DeviceUserID = postDevice.DeviceUserID,
                RoomId = postDevice.RoomId,
                DeviceAddUserID = UserID,
                DeviceAddTime = DateTime.Now,
                DeviceUpdateUserID = UserID,
                DeviceUpDateTime = DateTime.Now,
                DeviceFlag = 0
            };

            // DBに追加
            _context.Devices.Add(device);

            try
            {
                // デバイス情報を保存
                await _context.SaveChangesAsync();

                // 関連する入室情報も同時に登録
                _entrantsController.PostEntrantsProcess(new PostEntrants { DeviceID = device.DeviceId });

                return device;
            }
            catch (DbUpdateException e)
            {
                // 既に存在する場合の例外処理
                if (DeviceExists(device.DeviceId))
                    throw new Exception("Deviceが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 指定されたIDのデバイス情報を削除します。
        /// </summary>
        public async Task<bool> DeleteDeviceProcess(int id)
        {
            try
            {
                // 指定IDのデバイス情報を取得
                var device = await _context.Devices.FindAsync(id);
                if (device == null)
                    throw new Exception("Deviceが見つかりません");

                // デバイス情報を削除
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        // 指定IDのデバイスが存在するか確認
        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.DeviceId == id);
        }
    }
    /// <summary>
    /// デバイス情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetDevice
    {
        /// <summary>
        /// デバイスID
        /// </summary>
        public int? DeviceId { get; set; }
        /// <summary>
        /// デバイス名
        /// </summary>
        public string? DeviceName { get; set; }
        /// <summary>
        /// デバイスの設置場所
        /// </summary>
        public string? DeviceLocation { get; set; }
        /// <summary>
        /// デバイスカテゴリ（会議室=1、オフィス=2、パブリックスペース=3）
        /// </summary>
        public int? DeviceCategory { get; set; }
        /// <summary>
        /// デバイス登録ユーザー名
        /// </summary>
        public string? DeviceAddUserName { get; set; }
        /// <summary>
        /// デバイス更新ユーザー名
        /// </summary>
        public string? DeviceUpdateUserName { get; set; }
        /// <summary>
        /// 部屋ID
        /// </summary>
        public int? RoomId { get; set; }
        /// <summary>
        /// オフィスID
        /// </summary>
        public int? OfficeId { get; set; }
        /// <summary>
        /// デバイスフラグ
        /// </summary>
        public int? DeviceFlag { get; set; }
    }
    /// <summary>
    /// デバイス情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostDevice
    {
        /// <summary>
        /// デバイス名
        /// </summary>
        public required string DeviceName { get; set; }
        /// <summary>
        /// デバイスの設置場所
        /// </summary>
        public required string DeviceLocation { get; set; }
        /// <summary>
        /// デバイスカテゴリ（会議室=1、オフィス=2、パブリックスペース=3）
        /// </summary>
        public required int DeviceCategory { get; set; }
        /// <summary>
        /// 部屋ID
        /// </summary>
        public required int RoomId { get; set; }
        /// <summary>
        /// デバイス利用ユーザーID
        /// </summary>
        public required string DeviceUserID { get; set; }
    }
    /// <summary>
    /// デバイス情報の更新時に使用するクラスです。
    /// </summary>
    public class PutDevice
    {
        /// <summary>
        /// デバイスID
        /// </summary>
        public required int DeviceId { get; set; }
        /// <summary>
        /// デバイス名
        /// </summary>
        public required string DeviceName { get; set; }
        /// <summary>
        /// デバイスの設置場所
        /// </summary>
        public required string DeviceLocation { get; set; }
        /// <summary>
        /// デバイスカテゴリ（会議室=1、オフィス=2、パブリックスペース=3）
        /// </summary>
        public required int DeviceCategory { get; set; }
        /// <summary>
        /// 部屋ID
        /// </summary>
        public required int RoomId { get; set; }
        /// <summary>
        /// デバイス利用ユーザーID
        /// </summary>
        public required string DeviceUserID { get; set; }
        /// <summary>
        /// デバイスフラグ
        /// </summary>
        public required int DeviceFlag { get; set; }
    }
}
