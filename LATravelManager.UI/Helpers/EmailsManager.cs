using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;

namespace LATravelManager.UI.Helpers
{
    public class EmailsManager
    {
        public EmailsManager()
        {
            PartnersMails = new List<PartnerMail>();
        }

        public void AddToMails(string mail, ReservationWrapper reservation, string filePath)
        {
            var partnerMail = PartnersMails.FirstOrDefault(p => p.EmailAdress == mail);
            if (partnerMail == null)
            {
                partnerMail = new PartnerMail { EmailAdress = mail };
                PartnersMails.Add(partnerMail);
            }
            partnerMail.Mails.Add(new MailWithFile { FilePath = filePath, Reservation = reservation });
        }

        private List<PartnerMail> PartnersMails { get; set; }


        public void SendAllEmailsToPartner(PartnerMail partnerMail)
        {
            try
            {
                if (partnerMail == null || partnerMail.Mails.Count == 0)
                {
                    return;
                }
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("sales@latravel.gr")
                };
                //Put the email where you want to send.

                mail.To.Add(partnerMail.EmailAdress);

                string going = string.Empty;
                string dates = string.Empty;

                foreach (var r in partnerMail.Mails)
                {
                    if (!going.Contains(r.Reservation.Booking.Excursion.Destinations[0].Name))
                    {
                        going += r.Reservation.Booking.Excursion.Destinations[0].Name + "-";

                    }
                    if (!going.Contains(r.Reservation.CheckIn.ToString("dd/MM")))
                    {
                        going += r.Reservation.CheckIn.ToString("dd/MM") + "/";
                    }
                }

                going = going.TrimEnd('-');
                dates = dates.TrimEnd('/');

                if (partnerMail.Mails.Count > 1)
                {
                    mail.Subject = $"Vouchers και Ενημερωτικά ";
                }
                else
                {
                    mail.Subject = $"Voucher και Ενημερωτικό ";
                }

                StringBuilder sbBody = new StringBuilder();

                sbBody.Append(mail.Subject);
                sbBody.Append(":");
                sbBody.Append(Environment.NewLine);
                foreach (var p in partnerMail.Mails)
                {
                    sbBody.Append($"{p.Reservation.Booking.Excursion.Name} {p.Reservation.Dates} / {p.Reservation.CustomersList[0]}");
                }


                mail.Body = sbBody.ToString();

                LinkedResource LinkedImage = new LinkedResource(@"Images\01_latravel_logo1.jpg")
                {
                    ContentId = "latravel",
                    ContentType = new ContentType(MediaTypeNames.Image.Jpeg)
                };

                LinkedResource LinkedImage1 = new LinkedResource(@"Images\gobansko.jpg")
                {
                    ContentId = "gotoskiathos",


                    ContentType = new ContentType(MediaTypeNames.Image.Jpeg)
                };

                LinkedResource LinkedImage2 = new LinkedResource(@"Images\gotoskiathos.jpg")
                {
                    ContentId = "gobansko",
                    ContentType = new ContentType(MediaTypeNames.Image.Jpeg)
                };
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString("<br/> Το παρόν email έχει σταλεί αυτόματα από την κεντρική εφαρμογή διαχείρισης κρατήσεων του LA Travel.<br/>" +
                            "Για οποιαδήποτε διευκρίνιση μπορείτε να επικοινωνήσετε μαζί μας στα τηλέφωνα:<br/>" +
                            "Thessaloniki: 2310260986 <br/>Larisa: 2410555689<br/> Athens: 2102841001 <br/>  <a href=\"https://www.latravel.gr/ \">  <img src=cid:latravel ></a><br/><a  href=\"https://www.gotoskiathos.com/ \"> <img src=cid:gotoskiathos ></a><a href=\"http://www.gobansko.com/ \"> <img src=cid:gobansko ></a>", null, "text/html");
                htmlView.LinkedResources.Add(LinkedImage);
                htmlView.LinkedResources.Add(LinkedImage1);
                htmlView.LinkedResources.Add(LinkedImage2);
                mail.AlternateViews.Add(htmlView);

                foreach (var mailf in partnerMail.Mails)
                {
                    if (File.Exists(mailf.FilePath))
                    {
                        mail.Attachments.Add(new Attachment(mailf.FilePath));
                    }
                }
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("latravel.sales@gmail.com", "!#sales#!"),
                    EnableSsl = true
                };
                client.Port = 587;
                client.Send(mail);
                mail.Dispose();
                client.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void SendEmail(ReservationWrapper reservationWrapper, string filePath)
        {
            try
            {
                string infoDir = string.Empty;
                string voucherDir = string.Empty;

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("sales@latravel.gr")
                };
                //Put the email where you want to send.

                mail.To.Add("achilleaskaragiannis@outlook.com");
                // mail.To.Add("sales@latravel.gr");
                //mail.To.Add("info@latravel.com");

                mail.Subject = $"Auto mail test";

                StringBuilder sbBody = new StringBuilder();



                mail.Body = sbBody.ToString();

                LinkedResource LinkedImage = new LinkedResource(@"Images\01_latravel_logo1.jpg")
                {
                    ContentId = "latravel",
                    //Added the patch for Thunderbird as suggested by Jorge
                    ContentType = new ContentType(MediaTypeNames.Image.Jpeg)
                };

                LinkedResource LinkedImage1 = new LinkedResource(@"Images\gobansko.jpg")
                {
                    ContentId = "gotoskiathos",
                    // Added the patch for Thunderbird as suggested by Jorge


                    ContentType = new ContentType(MediaTypeNames.Image.Jpeg)
                };

                LinkedResource LinkedImage2 = new LinkedResource(@"Images\gotoskiathos.jpg")
                {
                    ContentId = "gobansko",
                    //Added the patch for Thunderbird as suggested by Jorge
                    ContentType = new ContentType(MediaTypeNames.Image.Jpeg)
                };
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(" Το παρόν email έχει σταλεί αυτόματα από την κεντρική εφαρμογή διαχείρισης κρατήσεων του LA Travel.<br/>" +
                            "Για οποιαδήποτε διευκρίνιση μπορείτε να επικοινωνήσετε μαζί μας στα τηλέφωνα:<br/>" +
                            "Thessaloniki: 2310260986 <br/>Larisa: 2410555689<br/> Athens: 2102841001 <br/>  <a href=\"https://www.latravel.gr/ \">  <img src=cid:latravel ></a><br/><a  href=\"https://www.gotoskiathos.com/ \"> <img src=cid:gotoskiathos ></a><a href=\"http://www.gobansko.com/ \"> <img src=cid:gobansko ></a>", null, "text/html");
                htmlView.LinkedResources.Add(LinkedImage);
                htmlView.LinkedResources.Add(LinkedImage1);
                htmlView.LinkedResources.Add(LinkedImage2);
                mail.AlternateViews.Add(htmlView);

                Attachment attachment;
                //folderNameVouchers = CreateFolder(Date, @"\Vouchers\");
                //folderNameInfo = CreateFolder(Date, @"\Ενημερωτικά\");
                //DirectoryInfo di = new DirectoryInfo(folderNameVouchers);
                //foreach (FileInfo file in di.GetFiles())
                //    file.Delete();
                //di = new DirectoryInfo(folderNameInfo);
                //foreach (FileInfo file in di.GetFiles())
                //    file.Delete();
                //foreach (Reservation res in perPartnerReservations)
                //{
                //    voucherDir = PrintVoucher(res, out infoDir);
                //    //Your log file path
                //    if (File.Exists(infoDir) && File.Exists(voucherDir))
                //    {
                attachment = new Attachment("Sources/airports.txt");
                //        mail.Attachments.Add(attachment);
                //        attachment = new Attachment(voucherDir);
                //        mail.Attachments.Add(attachment);
                //    }
                //}
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("latravel.sales@gmail.com", "!#sales#!"),
                    EnableSsl = true
                };
                client.Port = 587;
                //SmtpServer.EnableSsl = true;
                // client.EnableSsl = true;
                client.Send(mail);
                mail.Dispose();
                attachment.Dispose();
                client.Dispose();
                // MessageBox.Show("The exception has been sent! :)");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }

    public class PartnerMail
    {
        public PartnerMail()
        {
            Mails = new List<MailWithFile>();
        }
        public string EmailAdress { get; set; }

        public List<MailWithFile> Mails { get; set; }

    }

    public class MailWithFile
    {
        public ReservationWrapper Reservation { get; set; }

        public string FilePath { get; set; }
    }
}
