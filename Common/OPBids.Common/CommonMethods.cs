using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static OPBids.Common.Constant;

namespace OPBids.Common
{
    public class CommonMethods
    {
        public static string SendEmail(string mailSubject, string mailBody, List<string> toList)
        {
            return SendEmail(mailSubject, mailBody, new Dictionary<string, System.IO.Stream>(), toList);
        }
        public static string SendEmail(string mailSubject, string mailBody, Dictionary<string, System.IO.Stream> attachmentList, List<string> toList)
        {
            try
            {
                var smtp = new SmtpClient()
                {
                    Host = AppSettings.Smtp,
                    Port = AppSettings.EmailPort,
                    EnableSsl = AppSettings.IsEmailSSL,
                    UseDefaultCredentials = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(AppSettings.EmailFrom, AppSettings.EmailPassword),

                };
                using (var mail = new MailMessage())
                {
                    mail.Subject = mailSubject;
                    mail.From = new MailAddress(AppSettings.EmailFrom, AppSettings.AppName);
                    mail.Priority = MailPriority.High;
                    if (AppSettings.IsTestEmail == true)
                    {
                        mail.To.Add(AppSettings.TestEmailAddress);
                    }
                    else
                    {
                        toList.ForEach(a => mail.To.Add(a));
                    }                                        
                    mail.Body = mailBody;
                    
                    if (attachmentList != null)
                    {
                        foreach (KeyValuePair<string, System.IO.Stream> mAttachment in attachmentList)
                        {
                            mail.Attachments.Add(new Attachment(mAttachment.Value, mAttachment.Key));
                        }
                    }
                    mail.IsBodyHtml = true;
                    smtp.Send(mail);
                }
                return "E-Mail successfully sent";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string SendEmailEmbedImage(string mailSubject, string mailBody, List<string> toList, string linkedresource, string contentId)
        {
            try
            {
                var smtp = new SmtpClient()
                {
                    Host = AppSettings.Smtp,
                    Port = AppSettings.EmailPort,
                    EnableSsl = AppSettings.IsEmailSSL,
                    UseDefaultCredentials = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(AppSettings.EmailFrom, AppSettings.EmailPassword),

                };
                using (var mail = new MailMessage())
                {
                    mail.Subject = mailSubject;
                    mail.From = new MailAddress(AppSettings.EmailFrom, AppSettings.AppName);
                    mail.Priority = MailPriority.High;
                    if (AppSettings.IsTestEmail == true)
                    {
                        mail.To.Add(AppSettings.TestEmailAddress);
                    }
                    else
                    {
                        toList.ForEach(a => mail.To.Add(a));
                    }
                    //mail.Body = mailBody;
                    LinkedResource logo = new LinkedResource(linkedresource);
                    logo.ContentId = contentId;

                    AlternateView av = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                    av.LinkedResources.Add(logo);

                    mail.AlternateViews.Add(av);
                    mail.IsBodyHtml = true;
                    smtp.Send(mail);
                }
                return "E-Mail successfully sent";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string getDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }

        }

        #region " Convert Number to Words "
        public static string ConvertToWords(decimal value)
        {
            string valStr = "";
            string endStr = "Pesos";
            string centStr = "";
            try
            {
                decimal whole = Math.Truncate(value);
                decimal fraction = value - whole;

                if (fraction > 0)
                {
                    if (fraction > 0)
                    {
                        centStr = string.Format(" and {0}/100", fraction.ToString(".00").Substring(1));
                    }
                }
                valStr = string.Format("{0} {1}{2}", ConvertWholeNumber(whole.ToString()).Trim(), endStr, centStr);
            }
            catch { }
            return valStr;
        }
        private static string ConvertWholeNumber(string Number)
        {
            string word = "";
            try
            {
                bool isDone = false;
                double dblAmt = (Convert.ToDouble(Number));
                
                if (dblAmt > 0)
                {   
                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping 
                    string place = "";//digit grouping name:hundres,thousand,etc... 
                    switch (numDigits)
                    {
                        case 1://ones' range 

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range 
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range 
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range 
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range 
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range 
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion... 
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {
                        //if transalation is not done, continue...
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }
                    }
                    //ignore digit grouping names 
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
               
            }
            catch { }
            return word.Trim();
        }

        private static string tens(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static string ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        #endregion


    }
}
