using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MF2024_API.Models;
using MF2024_API.Interfaces;
using QRCoder;
using MF2024_API.Service;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace MF2024_API.Method
{

    public class Reservations
    {
        private readonly Mf2024apiDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly AESInterfaces _aesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Reservations(Mf2024apiDbContext context, ITokenService tokenService, AESInterfaces aESInterfaces, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _tokenService = tokenService;
            _aesService = aESInterfaces;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 指定された条件で予約情報を検索します。
        /// </summary>
        public async Task<List<Reservation>> GetReservationProcess(GetReservation getReservation)
        {
            try
            {
                // 予約情報のクエリを初期化
                var query = _context.Reservations.AsQueryable();

                // 各検索条件が指定されていればクエリに追加
                if (getReservation.ReservationId != null)
                    query = query.Where(x => x.ReservationId == getReservation.ReservationId);
                if (getReservation.ReservationName != null)
                    query = query.Where(x => x.ReservationName == getReservation.ReservationName);
                if (getReservation.ReservationNameKana != null)
                    query = query.Where(x => x.ReservationNameKana == getReservation.ReservationNameKana);
                if (getReservation.ReservationNumberOfPersonsStart != null)
                    query = query.Where(x => x.ReservationNumberOfPersons >= getReservation.ReservationNumberOfPersonsStart);
                if (getReservation.ReservationNumberOfPersonsEnd != null)
                    query = query.Where(x => x.ReservationNumberOfPersons <= getReservation.ReservationNumberOfPersonsEnd);
                if (getReservation.ReservationRequirement != null)
                    query = query.Where(x => x.ReservationRequirement == getReservation.ReservationRequirement);
                if (getReservation.ReservationCompanyName != null)
                    query = query.Where(x => x.ReservationCompanyName == getReservation.ReservationCompanyName);
                if (getReservation.ReservationCompanyNameKana != null)
                    query = query.Where(x => x.ReservationCompanyNameKana == getReservation.ReservationCompanyNameKana);
                if (getReservation.ReservationCompanyPosition != null)
                    query = query.Where(x => x.ReservationCompanyPosition == getReservation.ReservationCompanyPosition);
                if (getReservation.ReservationDateStart != null)
                    query = query.Where(x => x.ReservationDate >= getReservation.ReservationDateStart);
                if (getReservation.ReservationDateEnd != null)
                    query = query.Where(x => x.ReservationDate <= getReservation.ReservationDateEnd);
                if (getReservation.ReservationState != null)
                    query = query.Where(x => x.ReservationState == getReservation.ReservationState);
                if (getReservation.ReservationReception != null)
                    query = query.Where(x => x.ReservationReception == getReservation.ReservationReception);
                if (getReservation.Token != null)
                    query = query.Where(x => x.Token == getReservation.Token);
                if (getReservation.ReservationQrcode != null)
                    query = query.Where(x => x.ReservationQrcode == getReservation.ReservationQrcode);
                if (getReservation.ReservationCode != null)
                    query = query.Where(x => x.ReservationCode == getReservation.ReservationCode);
                if (getReservation.ReservationEmail != null)
                    query = query.Where(x => x.ReservationEmail == getReservation.ReservationEmail);
                if (getReservation.ReservationPhoneNumber != null)
                    query = query.Where(x => x.ReservationPhoneNumber == getReservation.ReservationPhoneNumber);
                if (getReservation.ReservationAddUserID != null)
                    query = query.Where(x => x.ReservationAddUserID == getReservation.ReservationAddUserID);
                if (getReservation.ReservationAddTimeStart != null)
                    query = query.Where(x => x.ReservationAddTime >= getReservation.ReservationAddTimeStart);
                if (getReservation.ReservationAddTimeEnd != null)
                    query = query.Where(x => x.ReservationAddTime <= getReservation.ReservationAddTimeEnd);
                if (getReservation.ReservationUpdateUserID != null)
                    query = query.Where(x => x.ReservationUpdateUserID == getReservation.ReservationUpdateUserID);
                if (getReservation.ReservationUpDateTimeStart != null)
                    query = query.Where(x => x.ReservationUpDateTime >= getReservation.ReservationUpDateTimeStart);
                if (getReservation.ReservationUpDateTimeEnd != null)
                    query = query.Where(x => x.ReservationUpDateTime <= getReservation.ReservationUpDateTimeEnd);
                if (getReservation.SectionId != null)
                    query = query.Where(x => x.SectionId == getReservation.SectionId);

                // クエリを実行し、結果をリストで取得
                var reservations = await query.ToListAsync();
                return reservations;
            }
            catch (Exception e)
            {
                // 例外発生時はそのまま再スロー
                throw e;
            }
        }

        /// <summary>
        /// 指定された内容で予約情報を更新します。
        /// </summary>
        public async Task<Reservation> PutReservationProcess(PutReservation putReservation)
        {
            // 指定IDの予約情報を取得
            var reservation = await _context.Reservations.SingleAsync(x => x.ReservationId == putReservation.ReservationId);
            if (reservation == null)
                throw new Exception("Reservationが見つかりません");

            // 各プロパティが指定されていれば更新
            if (putReservation.ReservationName != null)
                reservation.ReservationName = putReservation.ReservationName;
            if (putReservation.ReservationNameKana != null)
                reservation.ReservationNameKana = putReservation.ReservationNameKana;
            if (putReservation.ReservationNumberOfPersons != null)
                reservation.ReservationNumberOfPersons = (int)putReservation.ReservationNumberOfPersons;
            if (putReservation.ReservationRequirement != null)
                reservation.ReservationRequirement = putReservation.ReservationRequirement;
            if (putReservation.ReservationCompanyName != null)
                reservation.ReservationCompanyName = putReservation.ReservationCompanyName;
            if (putReservation.ReservationCompanyNameKana != null)
                reservation.ReservationCompanyNameKana = putReservation.ReservationCompanyNameKana;
            if (putReservation.ReservationCompanyPosition != null)
                reservation.ReservationCompanyPosition = putReservation.ReservationCompanyPosition;
            if (putReservation.ReservationDate != null)
                reservation.ReservationDate = (DateTime)putReservation.ReservationDate;
            if (putReservation.ReservationState != null)
                reservation.ReservationState = (int)putReservation.ReservationState;
            if (putReservation.ReservationEmail != null)
                reservation.ReservationEmail = putReservation.ReservationEmail;
            if (putReservation.ReservationPhoneNumber != null)
                reservation.ReservationPhoneNumber = putReservation.ReservationPhoneNumber;
            if (putReservation.ReservationReception != null)
                reservation.ReservationReception = (int)putReservation.ReservationReception;
            if (putReservation.SectionId != null)
                reservation.SectionId = (int)putReservation.SectionId;

            // ユーザーIDを取得し、更新者情報をセット
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");
            reservation.ReservationUpdateUserID = UserID;
            reservation.ReservationUpDateTime = DateTime.Now;

            // エンティティの状態を変更
            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                // DBに保存
                await _context.SaveChangesAsync();
                return reservation;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // 更新対象が存在しない場合の例外処理
                if (!ReservationExists(reservation.ReservationId))
                    throw new Exception("Reservationが見つかりません");
                else
                    throw e;
            }
        }

        /// <summary>
        /// 新しい予約情報を登録します。
        /// </summary>
        public async Task<Reservation> PostReservationProses(PostReservation postReservation)
        {
            // ランダムな予約コードを生成（英字のみ）
            var code = new string(Guid.NewGuid().ToString("N").Where(c => !char.IsDigit(c)).ToArray());

            // 予約コードをAES暗号化
            var decoded = await _aesService.Encrypt(code);

            // QRコードを生成
            var QRGenerator = new QRCodeGenerator();
            var QRData = QRGenerator.CreateQrCode(decoded.ToString(), QRCodeGenerator.ECCLevel.Q);
            var QRCode = new PngByteQRCode(QRData);
            var QRCodeImage = QRCode.GetGraphic(10);

            // ユーザーIDを取得
            var user = _httpContextAccessor.HttpContext?.User;
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null)
                throw new Exception("ユーザーが見つかりません");

            // Sectionの存在確認
            var section = await _context.Sections.SingleAsync(x => x.SectionId == postReservation.SectionId);
            if (section == null)
                throw new Exception("Sectionが見つかりません");

            // 新しい予約エンティティを作成
            var reservation = new Reservation
            {
                ReservationName = postReservation.ReservationName,
                ReservationNameKana = postReservation.ReservationNameKana,
                ReservationNumberOfPersons = postReservation.ReservationNumberOfPersons,
                ReservationRequirement = postReservation.ReservationRequirement,
                SectionId = postReservation.SectionId,
                ReservationDate = postReservation.ReservationDate,
                ReservationState = postReservation.ReservationState,
                ReservationEmail = postReservation.ReservationEmail,
                ReservationPhoneNumber = postReservation.ReservationPhoneNumber,
                ReservationQrcode = QRCodeImage,
                ReservationCode = code,
                ReservationAddUserID = UserID,
                ReservationAddTime = DateTime.Now,
                ReservationUpdateUserID = UserID,
                ReservationUpDateTime = DateTime.Now
            };

            // 法人予約の場合は会社情報もセット
            if (postReservation.ReservationState == 1)
            {
                reservation.ReservationCompanyName = postReservation.ReservationCompanyName;
                reservation.ReservationCompanyNameKana = postReservation.ReservationCompanyNameKana;
                reservation.ReservationCompanyPosition = postReservation.ReservationCompanyPosition;
            }

            // DBに追加
            _context.Reservations.Add(reservation);
            try
            {
                await _context.SaveChangesAsync();

                // トークンを作成し、予約情報にセット
                var token = _tokenService.CreateTokenReservation(reservation.ReservationCode);
                reservation.Token = token;
                _context.Entry(reservation).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return reservation;
            }
            catch (DbUpdateException)
            {
                // 既に存在する場合の例外処理
                if (ReservationExists(reservation.ReservationId))
                    throw new Exception("Reservationが重複しています");
                else
                    throw;
            }
        }

        /// <summary>
        /// 指定されたIDの予約情報を削除します。
        /// </summary>
        public async Task<bool> DeleteReservationProcess(int id)
        {
            // 指定IDの予約情報を取得
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                throw new Exception("Reservationが見つかりません");

            // 予約情報を削除
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        // 指定IDの予約が存在するか確認
        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }
    }
    /// <summary>
    /// 予約情報の新規登録時に使用するクラスです。
    /// </summary>
    public class PostReservation
    {
        /// <summary>
        /// 予約者名
        /// </summary>
        public required string ReservationName { get; set; }
        /// <summary>
        /// 予約者名(カナ)
        /// </summary>
        public string? ReservationNameKana { get; set; }
        /// <summary>
        /// 予約人数
        /// </summary>
        public required int ReservationNumberOfPersons { get; set; }
        /// <summary>
        /// 要件
        /// </summary>
        public required string ReservationRequirement { get; set; }
        /// <summary>
        /// sectionID
        /// </summary>
        public int SectionId { get; set; }
        /// <summary>
        /// 会社名
        /// </summary>
        public string? ReservationCompanyName { get; set; }
        /// <summary>
        /// 会社名(カナ)
        /// </summary>
        public string? ReservationCompanyNameKana { get; set; }
        /// <summary>
        /// 役職
        /// </summary>
        public string? ReservationCompanyPosition { get; set; }
        /// <summary>
        /// 予約日時
        /// </summary>
        public required DateTime ReservationDate { get; set; }
        /// <summary>
        /// ステータス（0:個人 1:法人 2:キャンセル）
        /// </summary>
        public required int ReservationState { get; set; }
        /// <summary>
        /// 予約タイプ（0:web予約 1:受付予約作成）
        /// </summary>
        public int ReservationType { get; set; }
        /// <summary>
        /// メールアドレス
        /// </summary>
        public required string ReservationEmail { get; set; }
        /// <summary>
        /// 電話番号
        /// </summary>
        public required string ReservationPhoneNumber { get; set; }
    }
    /// <summary>
    /// 予約情報の検索条件を指定するためのクラスです。
    /// </summary>
    public class GetReservation
    {
        /// <summary>
        /// 予約ID
        /// </summary>
        public int? ReservationId { get; set; }
        /// <summary>
        /// 予約者名
        /// </summary>
        public string? ReservationName { get; set; }
        /// <summary>
        /// 予約者名(カナ)
        /// </summary>
        public string? ReservationNameKana { get; set; }
        /// <summary>
        /// 予約人数（下限）
        /// </summary>
        public int? ReservationNumberOfPersonsStart { get; set; }
        /// <summary>
        /// 予約人数（上限）
        /// </summary>
        public int? ReservationNumberOfPersonsEnd { get; set; }
        /// <summary>
        /// 要件
        /// </summary>
        public string? ReservationRequirement { get; set; }
        /// <summary>
        /// sectionID
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// 会社名
        /// </summary>
        public string? ReservationCompanyName { get; set; }
        /// <summary>
        /// 会社名(カナ)
        /// </summary>
        public string? ReservationCompanyNameKana { get; set; }
        /// <summary>
        /// 役職
        /// </summary>
        public string? ReservationCompanyPosition { get; set; }
        /// <summary>
        /// 予約日時（開始）
        /// </summary>
        public DateTime? ReservationDateStart { get; set; }
        /// <summary>
        /// 予約日時（終了）
        /// </summary>
        public DateTime? ReservationDateEnd { get; set; }
        /// <summary>
        /// ステータス（0:個人 1:法人 2:キャンセル）
        /// </summary>
        public int? ReservationState { get; set; }
        /// <summary>
        /// 受付済みか（0:未受付 1:受付済み）
        /// </summary>
        public int? ReservationReception { get; set; }
        /// <summary>
        /// 予約タイプ（0:web予約 1:受付予約作成）
        /// </summary>
        public int? ReservationType { get; set; }
        /// <summary>
        /// トークン（web予約用）
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// QRコード
        /// </summary>
        public byte[]? ReservationQrcode { get; set; }
        /// <summary>
        /// 予約コード
        /// </summary>
        public string? ReservationCode { get; set; }
        /// <summary>
        /// メールアドレス
        /// </summary>
        public string? ReservationEmail { get; set; }
        /// <summary>
        /// 電話番号
        /// </summary>
        public string? ReservationPhoneNumber { get; set; }
        /// <summary>
        /// 追加ユーザーID
        /// </summary>
        public string? ReservationAddUserID { get; set; }
        /// <summary>
        /// 追加日時（開始）
        /// </summary>
        public DateTime? ReservationAddTimeStart { get; set; }
        /// <summary>
        /// 追加日時（終了）
        /// </summary>
        public DateTime? ReservationAddTimeEnd { get; set; }
        /// <summary>
        /// 更新ユーザーID
        /// </summary>
        public string? ReservationUpdateUserID { get; set; }
        /// <summary>
        /// 更新日時（開始）
        /// </summary>
        public DateTime? ReservationUpDateTimeStart { get; set; }
        /// <summary>
        /// 更新日時（終了）
        /// </summary>
        public DateTime? ReservationUpDateTimeEnd { get; set; }
    }
    /// <summary>
    /// 予約情報の更新時に使用するクラスです。
    /// </summary>
    public class PutReservation
    {
        /// <summary>
        /// 予約ID
        /// </summary>
        public int ReservationId { get; set; }
        /// <summary>
        /// 予約者名
        /// </summary>
        public string? ReservationName { get; set; }
        /// <summary>
        /// 予約者名(カナ)
        /// </summary>
        public string? ReservationNameKana { get; set; }
        /// <summary>
        /// 予約人数
        /// </summary>
        public int? ReservationNumberOfPersons { get; set; }
        /// <summary>
        /// 要件
        /// </summary>
        public string? ReservationRequirement { get; set; }
        /// <summary>
        /// sectionID
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// 会社名
        /// </summary>
        public string? ReservationCompanyName { get; set; }
        /// <summary>
        /// 会社名(カナ)
        /// </summary>
        public string? ReservationCompanyNameKana { get; set; }
        /// <summary>
        /// 役職
        /// </summary>
        public string? ReservationCompanyPosition { get; set; }
        /// <summary>
        /// 予約日時
        /// </summary>
        public DateTime? ReservationDate { get; set; }
        /// <summary>
        /// ステータス（0:個人 1:法人 2:キャンセル）
        /// </summary>
        public int? ReservationState { get; set; }
        /// <summary>
        /// 受付済みか（0:未受付 1:受付済み）
        /// </summary>
        public int? ReservationReception { get; set; }
        /// <summary>
        /// メールアドレス
        /// </summary>
        public string? ReservationEmail { get; set; }
        /// <summary>
        /// 電話番号
        /// </summary>
        public string? ReservationPhoneNumber { get; set; }
    }
}
