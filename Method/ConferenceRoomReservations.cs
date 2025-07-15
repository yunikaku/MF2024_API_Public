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

    public class ConferenceRoomReservations
    {
        private readonly Mf2024apiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConferenceRoomReservations(Mf2024apiDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件で会議室予約情報を検索します。
        /// </summary>
        /// <param name="getConferenceRoomReservation">検索条件を格納した GetConferenceRoomReservation オブジェクト</param>
        /// <returns>条件に一致する会議室予約情報のリスト</returns>
        /// <exception cref="Exception">検索処理中にエラーが発生した場合</exception>
        public async Task<List<ConferenceRoomReservation>> GetConferenceRoomReservationProcess(GetConferenceRoomReservation getConferenceRoomReservation)
        {
            // 会議室予約情報のクエリを初期化
            var query = _context.ConferenceRoomReservation.AsQueryable();

            // 各検索条件が指定されていればクエリに追加
            if (getConferenceRoomReservation.ConferenceRoomReservationId != null)
            {
                query = query.Where(x => x.ConferenceRoomReservationId == getConferenceRoomReservation.ConferenceRoomReservationId);
            }
            if (getConferenceRoomReservation.ConferenceRoomReservationRequirement != null)
            {
                query = query.Where(x => x.ConferenceRoomReservationRequirement == getConferenceRoomReservation.ConferenceRoomReservationRequirement);
            }
            if (getConferenceRoomReservation.DeviceId != null)
            {
                query = query.Where(x => x.DeviceId == getConferenceRoomReservation.DeviceId);
            }
            if (getConferenceRoomReservation.StartTime != null)
            {
                // 指定開始日時より前に開始する予約を取得
                query = query.Where(x => x.StartTime < getConferenceRoomReservation.StartTime);
            }
            if (getConferenceRoomReservation.EndTime != null)
            {
                // 指定終了日時より後に終了する予約を取得
                query = query.Where(x => x.EndTime > getConferenceRoomReservation.EndTime);
            }
            if (getConferenceRoomReservation.GetDay != null)
            {
                // 指定日の予約を取得
                var getDay = (DateTime)getConferenceRoomReservation.GetDay;
                var getDayEnd = getDay.AddDays(1);
                query = query.Where(x => x.StartTime < getDayEnd && x.EndTime > getDay);
            }

            // クエリを実行し、結果をリストで取得
            return await query.ToListAsync();
        }

        /// <summary>
        /// 指定された内容で会議室予約情報を更新します。
        /// </summary>
        public async Task<ConferenceRoomReservation> PutConferenceRoomReservationProcess(PutConferenceRoomReservation putConferenceRoomReservation)
        {
            // 指定IDの会議室予約情報を取得
            var conferenceRoomReservation = await _context.ConferenceRoomReservation.FindAsync(putConferenceRoomReservation.ConferenceRoomReservationId);
            if (conferenceRoomReservation == null)
            {
                throw new Exception("予約が見つかりません");
            }

            // 同じデバイス・時間帯で重複する予約がないかチェック
            var conferenceRoomReservationData = await GetConferenceRoomReservationProcess(new GetConferenceRoomReservation
            {
                DeviceId = conferenceRoomReservation.DeviceId,
                StartTime = putConferenceRoomReservation.StartTime,
                EndTime = putConferenceRoomReservation.EndTime
            });
            if (conferenceRoomReservationData.Count > 0)
            {
                throw new Exception("予約が重複しています");
            }

            // 各プロパティが指定されていれば更新
            if (putConferenceRoomReservation.ConferenceRoomReservationRequirement != null)
            {
                conferenceRoomReservation.ConferenceRoomReservationRequirement = putConferenceRoomReservation.ConferenceRoomReservationRequirement;
            }
            if (putConferenceRoomReservation.StartTime != null)
            {
                conferenceRoomReservation.StartTime = putConferenceRoomReservation.StartTime;
            }
            if (putConferenceRoomReservation.EndTime != null)
            {
                conferenceRoomReservation.EndTime = putConferenceRoomReservation.EndTime;
            }

            // 更新日時をセット
            conferenceRoomReservation.UpdateTime = DateTime.Now;

            // エンティティの状態を変更
            _context.Entry(conferenceRoomReservation).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return conferenceRoomReservation;
            }
            catch (DbUpdateConcurrencyException)
            {
                // 更新対象が存在しない場合の例外処理
                if (!ConferenceRoomReservationExists(conferenceRoomReservation.ConferenceRoomReservationId))
                {
                    throw new Exception("予約が見つかりません");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 新しい会議室予約情報を登録します。
        /// </summary>
        public async Task<ConferenceRoomReservation> PostConferenceRoomReservationProcess(PostConferenceRoomReservation conferenceRoomReservation)
        {
            try
            {
                // 同じデバイス・時間帯で重複する予約がないかチェック
                var conferenceRoomReservationData = await GetConferenceRoomReservationProcess(new GetConferenceRoomReservation
                {
                    DeviceId = conferenceRoomReservation.DeviceId,
                    StartTime = conferenceRoomReservation.StartTime,
                    EndTime = conferenceRoomReservation.EndTime
                });
                if (conferenceRoomReservationData.Count > 0)
                {
                    throw new Exception("予約が重複しています");
                }

                // 新しい会議室予約エンティティを作成
                var addconferenceRoomReservationData = new ConferenceRoomReservation
                {
                    ConferenceRoomReservationRequirement = conferenceRoomReservation.ConferenceRoomReservationRequirement,
                    DeviceId = conferenceRoomReservation.DeviceId,
                    StartTime = conferenceRoomReservation.StartTime,
                    EndTime = conferenceRoomReservation.EndTime,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                // DBに追加
                _context.ConferenceRoomReservation.Add(addconferenceRoomReservationData);
                await _context.SaveChangesAsync();
                return addconferenceRoomReservationData;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 指定されたIDの会議室予約情報を削除します。
        /// </summary>
        public async Task<bool> DeleteConferenceRoomReservationProcess(int id)
        {
            // 指定IDの会議室予約情報を取得
            var conferenceRoomReservation = await _context.ConferenceRoomReservation.FindAsync(id);
            if (conferenceRoomReservation == null)
            {
                throw new Exception("予約が見つかりません");
            }

            // 会議室予約情報を削除
            _context.ConferenceRoomReservation.Remove(conferenceRoomReservation);
            await _context.SaveChangesAsync();

            return true;
        }

        // 指定IDの会議室予約が存在するか確認
        private bool ConferenceRoomReservationExists(int id)
        {
            return _context.ConferenceRoomReservation.Any(e => e.ConferenceRoomReservationId == id);
        }
    }

    /// <summary>
    /// 会議室予約の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetConferenceRoomReservation
    {
        /// <summary>
        /// 会議室予約ID
        /// </summary>
        public int? ConferenceRoomReservationId { get; set; }
        /// <summary>
        /// 予約要件
        /// </summary>
        public string? ConferenceRoomReservationRequirement { get; set; }
        /// <summary>
        /// デバイスID
        /// </summary>
        public int? DeviceId { get; set; }
        /// <summary>
        /// 予約開始日時
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 予約終了日時
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 指定日の予約を取得するための日付
        /// </summary>
        public DateTime? GetDay { get; set; }
    }
    /// <summary>
    /// 会議室予約の新規登録時に使用するクラスです。
    /// </summary>
    public class PostConferenceRoomReservation
    {
        /// <summary>
        /// 予約要件
        /// </summary>
        public required string ConferenceRoomReservationRequirement { get; set; }
        /// <summary>
        /// デバイスID
        /// </summary>
        public required int DeviceId { get; set; }
        /// <summary>
        /// 予約開始日時
        /// </summary>
        public required DateTime StartTime { get; set; }
        /// <summary>
        /// 予約終了日時
        /// </summary>
        public required DateTime EndTime { get; set; }
    }
    /// <summary>
    /// 会議室予約の更新時に使用するクラスです。
    /// </summary>
    public class PutConferenceRoomReservation
    {
        /// <summary>
        /// 会議室予約ID
        /// </summary>
        public required int ConferenceRoomReservationId { get; set; }
        /// <summary>
        /// デバイスID
        /// </summary>
        public required int DeviceId { get; set; }
        /// <summary>
        /// 予約要件
        /// </summary>
        public string ConferenceRoomReservationRequirement { get; set; }
        /// <summary>
        /// 予約開始日時
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 予約終了日時
        /// </summary>
        public DateTime EndTime { get; set; }
    }

}
