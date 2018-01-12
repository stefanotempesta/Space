using SocialSharing.Models;
using SocialSharing.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialSharing.Controllers
{
    public class SocialController : Controller
    {
        [ChildActionOnly]
        public ActionResult ShareButtons(int id)
        {
            using (var db = new DatabaseContext())
            {
                return PartialView(new
                {
                    Article = db.Articles.Find(id),
                    SecurityToken = Request.Url.Host.Encrypt(EncryptionKey, EncryptionIV)
                }.ToDynamic());
            }
        }

        [HttpPost]
        public async Task<HttpStatusCodeResult> Audit(int modelId, string socialName)
        {
            string securityToken = Request.Headers["securityToken"];
            if (string.IsNullOrEmpty(securityToken))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string phrase = securityToken.Decrypt(EncryptionKey, EncryptionIV);
            if (phrase != Request.Url.Host)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var entry = new AuditTrailEntry
            {
                TimeStamp = DateTime.Now,
                Browser = Request.Browser.Browser,
                BrowserVersion = Request.Browser.Version,
                BrowserMajorVersion = Request.Browser.MajorVersion,
                BrowserIsMobileDevice = Request.Browser.IsMobileDevice,
                BrowserPlatform = Request.Browser.Platform,
                UrlReferrer = Request.UrlReferrer?.ToString(),
                UserAgent = Request.UserAgent,
                UserHostAddress = Request.UserHostAddress,
                SocialName = socialName,
                ModelId = modelId
            };

            using (var db = new DatabaseContext())
            {
                db.AuditTrail.Add(entry);
                await db.SaveChangesAsync();
            }

            await entry.GeolocateIpAddress(new IpInfoGeolocator(GeolocationApiUrl));

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ContentResult ShareByEmail(FormCollection values)
        {
            string emailAddress = values["email_address"];
            string emailSubject = values["email_subject"];
            string emailBody = values["email_body"];

            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress("<FROM EMAIL ADDRESS>"),
                    Subject = emailSubject,
                    Body = emailBody
                };
                message.To.Add(new MailAddress(emailAddress));

                SmtpClient client = new SmtpClient
                {
                    Host = SmtpServer,
                    Credentials = new NetworkCredential(SmtpUser, SmtpPassword)
                };

                client.Send(message);

                return Content("<div class='alert alert-success'>Thanks for sharing this post.</div>", "text/html");
            }
            catch (Exception ex)
            {
                return Content($"<div class='alert alert-danger'>{ex.Message}</div>", "text/html");
            }
        }

        public static string EncryptionKey => System.Configuration.ConfigurationManager.AppSettings["EncryptionKey"];

        public static string EncryptionIV => System.Configuration.ConfigurationManager.AppSettings["EncryptionIV"];

        public static string SmtpServer => System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];

        public static string SmtpUser => System.Configuration.ConfigurationManager.AppSettings["SmtpUser"];

        public static string SmtpPassword => System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];

        public static string GeolocationApiUrl => System.Configuration.ConfigurationManager.AppSettings["GeolocationApiUrl"];
    }
}