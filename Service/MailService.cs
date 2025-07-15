using MailKit.Net.Smtp;
using MailKit.Security;
using MF2024_API.Models;
using MimeKit;

namespace MF2024_API.Service
{
    public class MailService : Interfaces.MailInterfase
    {
        public Task<bool> SendMailReservation(MailModelTemplateReservstion mailModelTemplateReservstion)
        {
            try
            {
                var context = new Mf2024apiDbContext();
                var Reservation = context.Reservations.Where(x => x.ReservationId == mailModelTemplateReservstion.ReservationID).FirstOrDefault();
                if (Reservation == null) throw new Exception("予約が存在しません");


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Test", "c3260@oic.jp"));
                message.To.Add(new MailboxAddress(Reservation.ReservationName, Reservation.ReservationEmail));
                message.Subject = mailModelTemplateReservstion.Subject;

                var builder = new BodyBuilder();
                //HTMLファイルを読み込む
                //builder.HtmlBody　HTMLファイル設定
                //htmlファイルに変数を埋め込む(image,name,)

                var QR = Convert.ToBase64String(Reservation.ReservationQrcode);
                var html = File.ReadAllText(@"Service/ReservstionTemplate.html");
                html = html.
                    Replace("{{ Name }}", Reservation.ReservationName).
                    Replace("{{ ID }}", Reservation.ReservationId.ToString()).
                    Replace("{{ Date }}", Reservation.ReservationDate.ToString("yyyy/MM/dd")).
                    Replace("{{ Number }}", Reservation.ReservationNumberOfPersons.ToString()).
                    Replace("{{ Content }}", Reservation.ReservationRequirement).
                    Replace("{{code}}", Reservation.ReservationCode).
                    Replace("{{QR}}", QR)
                    ;
                builder.HtmlBody = html;

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("sandbox.smtp.mailtrap.io", 587, SecureSocketOptions.StartTls);//SMTPサーバー設定
                    client.Authenticate("81dcc0fc588976", "78732ab19a1e33");//メールアドレスとパスワード アプリパスワードではできない　
                    client.Send(message);//メール送信
                    client.Disconnect(true);//切断
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
