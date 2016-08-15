using System;
using System.IO;
using System.Net;

namespace uk.co.andgregor.TakeABrolly
{
    class debugLog
    {
        private static settingsIni iniReader = new settingsIni(appSettings.strCurrDir + "\\" + appSettings.settingsFile);


        public static void LogEvent(string strMessage, string strPrefix)
        {
            try
            {

                string localDir = Directory.GetCurrentDirectory();

                if (strPrefix.Length < 1) { strPrefix = "DEBUG"; }

                if (appSettings.writtenErrLog)
                {
                    if (!File.Exists(localDir + "/TakeABrolly.log"))
                    {
                        File.Create(localDir + "/TakeABrolly.log").Close();
                        LogEvent("Log File Missing, Creating.", "APPLICATION");
                    }

                    StreamWriter SW;
                    SW = File.AppendText(localDir + "/TakeABrolly.log");
                    SW.WriteLine(DateTime.Now.ToString() + ":" + strPrefix + " | " + strMessage);
                    SW.Close();
                }

                if (!appSettings.asService)
                {
                    Console.WriteLine(strPrefix + " | " + strMessage);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("FATAL: " + ex.Message);
            }

        }

        public static void logError(string strMessage, string strFunction)
        {
            try
            {

                appSettings.intErrCount = appSettings.intErrCount + 1;



                if (appSettings.emailErrLog && strFunction != "sendEmail")
                {
                    if (appSettings.intErrCount < 20)
                    {
                        sendEmail("andrew@andgregor.co.uk", appSettings.senderAddr, "An Error Occured in Take A Brolly, function: " + strFunction, strFunction + " generated the following error : " + strMessage, "");
                    }
                }

                if (appSettings.writtenErrLog || !(appSettings.asService))
                {
                    LogEvent(strMessage, "ERROR in " + strFunction);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FATAL: " + ex.Message);
            }

        }
        public static void sendEmail(string strRecipient, string strSender, string strSubject, string strMessage, string strAttachment)
        {
            try
            {
                string strServer = appSettings.smtpIP;
                string strAuth = appSettings.smtpPass;
                if (strRecipient.Length < 1)
                {
                    strRecipient = "andrew@andgregor.co.uk";
                }

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.To.Add(strRecipient);
                mailMessage.Subject = strSubject;
                mailMessage.From = new System.Net.Mail.MailAddress(strSender);
                mailMessage.Body = strMessage;
                //CHECK FOR HTML
                if (strMessage.Substring(0, 6) == "<html>")
                {
                    mailMessage.IsBodyHtml = true;
                }
                //IF ATTACHMENT DATA
                if (strAttachment.Length > 0)
                {
                    System.Net.Mail.Attachment mailAttach = new System.Net.Mail.Attachment(strAttachment);
                    mailMessage.Attachments.Add(mailAttach);
                }

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(strServer);

                if (strAuth.Length > 0)
                {
                    NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(strSender, strAuth);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = basicAuthenticationInfo;
                }
                smtp.Send(mailMessage);

            }
            catch (Exception ex)
            {
                logError("Exception: " + ex.Message, "sendEmail");
            }


        }

        public static void insertBranding()
        {
            debugLog.LogEvent("Name: Take A Brolly", "INFO");
            debugLog.LogEvent("Author: Andrew Gregory", "INFO");
            debugLog.LogEvent("Support: andrew@andgregor.co.uk", "INFO");
            debugLog.LogEvent("A Code Sample.", "INFO");
            debugLog.LogEvent("Updated: 08/2016", "INFO");
        }
    }
}
