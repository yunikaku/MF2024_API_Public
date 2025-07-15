using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MF2024_API.Models;
using System.Security.Claims;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using Microsoft.AspNetCore.Authorization;



namespace MF2024_API.Method
{

    public class Rooms
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Entrants _entrantsController;
        public Rooms(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _entrantsController = new Entrants(context, httpContextAccessor);
        }

        /// <summary>
        /// 指定された条件で部屋情報を検索します。
        /// </summary>
        /// <param name="getRoom">検索条件を格納した <see cref="GetRoom"/> オブジェクト</param>
        /// <returns>条件に一致する部屋情報のリスト <see cref="List{Room}"/></returns>
        /// <exception cref="Exception">検索処理中にエラーが発生した場合</exception>
        /// <see cref="Room"/>
        public async Task<List<Room>> GetRoomProcess(GetRoom getRoom)
        {
            try
            {
                // 部屋情報のクエリを初期化
                var Query = _context.Rooms.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getRoom.RoomId != null)
                    Query = Query.Where(x => x.RoomId == getRoom.RoomId);

                if (getRoom.RoomName != null)
                    Query = Query.Where(x => x.RoomName == getRoom.RoomName);
                else if (getRoom.RoomCapacityStart != null)
                    Query = Query.Where(x => x.RoomCapacity >= getRoom.RoomCapacityStart);
                else if (getRoom.RoomCapacityEnd != null)
                    Query = Query.Where(x => x.RoomCapacity <= getRoom.RoomCapacityEnd);

                if (getRoom.OfficeId != null)
                    Query = Query.Where(x => x.OfficeId == getRoom.OfficeId);

                if (getRoom.RoomState != null)
                    Query = Query.Where(x => x.RoomState == getRoom.RoomState);

                if (getRoom.SectionId != null)
                    Query = Query.Where(x => x.SectionId == getRoom.SectionId);

                if (getRoom.EquipmentsID != null)
                    Query = Query.Where(x => x.Equipment.Any(y => getRoom.EquipmentsID.Contains(y.EquipmentID)));

                // 設備・画像も含めて取得
                return await Query.Include(x => x.Equipment).Include(x => x.Images).ToListAsync();
            }
            catch (Exception ex)
            {
                // 例外発生時はそのまま再スロー
                throw ex;
            }
        }

        /// <summary>
        /// 指定された内容で部屋情報を更新します。
        /// </summary>
        /// <param name="putRoom">更新内容を格納した <see cref="PutRoom"/> オブジェクト</param>
        /// <returns>更新後の部屋情報 <see cref="Room"/></returns>
        /// <exception cref="Exception">対象が存在しない場合や更新時にエラーが発生した場合</exception>
        /// <see cref="Room"/>
        public async Task<Room> PutRoomProcess(PutRoom putRoom)
        {
            try
            {
                // 指定IDの部屋情報を取得
                var room = await _context.Rooms.FindAsync(putRoom.RoomId);
                if (room == null)
                    throw new Exception("Roomが見つかりません");

                // 各プロパティが指定されていれば更新
                if (putRoom.RoomName != null)
                    room.RoomName = putRoom.RoomName;
                if (putRoom.RoomCapacity != null)
                    room.RoomCapacity = (int)putRoom.RoomCapacity;
                if (putRoom.OfficeId != null)
                    room.OfficeId = (int)putRoom.OfficeId;
                if (putRoom.RoomState != null)
                    room.RoomState = (int)putRoom.RoomState;
                if (putRoom.SectionId != null)
                    room.SectionId = putRoom.SectionId;

                // 画像が指定されていれば既存画像を削除し、新しい画像を保存
                if (putRoom.images != null)
                {
                    // 既存画像を削除
                    var roomimage = _context.Images.Where(x => x.roomID == putRoom.RoomId).ToList();
                    _context.Images.RemoveRange(roomimage);
                    _context.SaveChanges();

                    using (var memoryStream = new MemoryStream())
                    {
                        // 新しい画像をリサイズ・保存
                        foreach (var image in putRoom.images)
                        {
                            var imagestream = image.OpenReadStream();
                            var imageresize = await Image.LoadAsync(imagestream);
                            imageresize.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Max,
                                Size = new Size(800, 600)
                            }));
                            await imageresize.SaveAsJpegAsync(memoryStream, new JpegEncoder
                            {
                                Quality = 75
                            });

                            var imageFile = memoryStream.ToArray();
                            var imageModel = new RoomImage()
                            {
                                RoomImageData = imageFile,
                                RoomImageName = image.FileName,
                                roomID = putRoom.RoomId
                            };
                            _context.Images.Add(imageModel);
                            room.Images.Add(imageModel);
                            _context.SaveChanges();
                        }
                    }
                }

                // 設備情報の更新
                if (putRoom.Equipments != null)
                {
                    var Equipments = _context.Equipments.Where(x => putRoom.Equipments.Contains(x.EquipmentID)).ToList();
                    room.Equipment = Equipments;
                }

                // ユーザー情報を取得し、更新者情報をセット
                var user = _httpContextAccessor.HttpContext?.User;
                var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (UserID == null)
                    throw new Exception("ユーザーが見つかりません");
                room.RoompDateUserID = UserID;
                room.RoomUpDateTime = DateTime.Now;

                // エンティティの状態を変更
                _context.Entry(room).State = EntityState.Modified;

                // DBに保存
                await _context.SaveChangesAsync();
                return room;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!RoomExists(putRoom.RoomId))
                    throw new Exception("Roomが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しい部屋情報を登録します。
        /// </summary>
        /// <param name="postRoom">登録内容を格納した <see cref="PostRoom"/> オブジェクト</param>
        /// <returns>登録された部屋情報 <see cref="Room"/></returns>
        /// <exception cref="Exception">登録時にエラーが発生した場合</exception>
        /// <see cref="Room"/>
        public async Task<Room> PostRoomProcess(PostRoom postRoom)
        {
            // ユーザー情報を取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // 新しい部屋エンティティを作成
            var room = new Room
            {
                RoomName = postRoom.RoomName,
                RoomCapacity = postRoom.RoomCapacity,
                OfficeId = postRoom.OfficeId,
                RoomState = postRoom.RoomState,
                SectionId = postRoom.SectionId,
                RoomAddUserID = UserID,
                RommAddTime = DateTime.Now,
                RoompDateUserID = UserID,
                RoomUpDateTime = DateTime.Now
            };

            // 画像が指定されていれば保存
            if (postRoom.images != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    // 画像をリサイズ・保存
                    foreach (var image in postRoom.images)
                    {
                        var imagestream = image.OpenReadStream();
                        var imageresize = await Image.LoadAsync(imagestream);
                        imageresize.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(800, 600)
                        }));
                        await imageresize.SaveAsJpegAsync(memoryStream, new JpegEncoder
                        {
                            Quality = 75
                        });

                        var imageFile = memoryStream.ToArray();
                        var imageModel = new RoomImage()
                        {
                            RoomImageData = imageFile,
                            RoomImageName = image.FileName,
                            roomID = room.RoomId
                        };
                        _context.Images.Add(imageModel);
                        room.Images.Add(imageModel);
                    }
                }
            }

            // 設備情報の設定
            if (postRoom.Equipments != null)
            {
                var Equipments = _context.Equipments.Where(x => postRoom.Equipments.Contains(x.EquipmentID)).ToList();
                room.Equipment = Equipments;
            }

            // DBに追加
            _context.Rooms.Add(room);
            try
            {
                await _context.SaveChangesAsync();
                return room;
            }
            catch (DbUpdateException e)
            {
                // 既に存在する場合の例外処理
                if (RoomExists(room.RoomId))
                    throw new Exception("Roomが重複しています");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 指定されたIDの部屋情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の部屋ID</param>
        /// <returns>削除に成功した場合は <see cref="bool"/>（true）</returns>
        /// <exception cref="Exception">対象が存在しない場合や削除時にエラーが発生した場合</exception>
        /// <see cref="Room"/>
        public async Task<bool> DeleteRoomProcess(int id)
        {
            try
            {
                // ユーザー情報を取得
                var user = _httpContextAccessor.HttpContext?.User;
                var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (UserID == null)
                    throw new Exception("ユーザーが見つかりません");

                // 指定IDの部屋情報を取得
                var room = await _context.Rooms.FindAsync(id);
                if (room == null)
                    throw new Exception("Roomが見つかりません");
                // 部屋情報を削除
                room.RoomState = 1; // 論理削除フラグを立てる
                room.RoompDateUserID = UserID;
                room.RoomUpDateTime = DateTime.Now;
                _context.Entry(room).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // その他の例外はそのまま再スロー
                throw ex;
            }
        }

        // 指定IDの部屋が存在するか確認
        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomId == id);
        }
    }
    /// <summary>
    /// 部屋情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetRoom
    {
        /// <summary>
        /// 部屋ID
        /// </summary>
        public int? RoomId { get; set; }
        /// <summary>
        /// 部屋名
        /// </summary>
        public string? RoomName { get; set; }
        /// <summary>
        /// 収容人数（下限）
        /// </summary>
        public int? RoomCapacityStart { get; set; }
        /// <summary>
        /// 収容人数（上限）
        /// </summary>
        public int? RoomCapacityEnd { get; set; }
        /// <summary>
        /// オフィスID
        /// </summary>
        public int? OfficeId { get; set; }
        /// <summary>
        /// 部屋状態
        /// </summary>
        public int? RoomState { get; set; }
        /// <summary>
        /// 課ID
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// 設備IDリスト
        /// </summary>
        public List<int>? EquipmentsID { get; set; }
    }
    /// <summary>
    /// 部屋情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostRoom
    {
        /// <summary>
        /// 部屋名
        /// </summary>
        public required string RoomName { get; set; }
        /// <summary>
        /// 収容人数
        /// </summary>
        public required int RoomCapacity { get; set; }
        /// <summary>
        /// オフィスID
        /// </summary>
        public required int OfficeId { get; set; }
        /// <summary>
        /// 部屋状態
        /// </summary>
        public required int RoomState { get; set; }
        /// <summary>
        /// 課ID
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// 部屋画像ファイルリスト
        /// </summary>
        public List<IFormFile>? images { get; set; }
        /// <summary>
        /// 設備IDリスト
        /// </summary>
        public List<int>? Equipments { get; set; }
    }
    /// <summary>
    /// 部屋情報の更新時に使用するクラスです。
    /// </summary>
    public class PutRoom
    {
        /// <summary>
        /// 部屋ID
        /// </summary>
        public required int RoomId { get; set; }
        /// <summary>
        /// 部屋名
        /// </summary>
        public string? RoomName { get; set; }
        /// <summary>
        /// 収容人数
        /// </summary>
        public int? RoomCapacity { get; set; }
        /// <summary>
        /// オフィスID
        /// </summary>
        public int? OfficeId { get; set; }
        /// <summary>
        /// 部屋状態
        /// </summary>
        public int? RoomState { get; set; }
        /// <summary>
        /// 課ID
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// 部屋画像ファイルリスト
        /// </summary>
        public List<IFormFile>? images { get; set; }
        /// <summary>
        /// 設備IDリスト
        /// </summary>
        public List<int>? Equipments { get; set; }
    }
}
