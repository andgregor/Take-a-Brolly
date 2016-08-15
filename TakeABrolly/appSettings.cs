using System;
using System.Collections;
using System.IO;


namespace uk.co.andgregor.TakeABrolly
{
    class appSettings
    {

        //Settings
        private static bool asservice;
        public static bool asService { get { return asservice; } set { asservice = value; } }

        private static string strapikey;
        public static string strApiKey { get { return strapikey; } set { strapikey = value; } }

        private static string strpostcode;
        public static string strPostCode { get { return strpostcode; } set { strpostcode = value; } }

        private static string strcountrycode;
        public static string strCountryCode { get { return strcountrycode; } set { strcountrycode = value; } }
        

        private static string strbaseurl;
        public static string strBaseURL { get { return strbaseurl; } set { strbaseurl = value; } }

        //Logging
        private static bool writtenerrlog;
        public static bool writtenErrLog { get { return writtenerrlog; } set { writtenerrlog = value; } }

        private static bool emailerrlog;
        public static bool emailErrLog { get { return emailerrlog; } set { emailerrlog = value; } }

        //Debug
        private static bool dodebug;
        public static bool doDebug { get { return dodebug; } set { dodebug = value; } }

        private static bool doupdate;
        public static bool doUpdate { get { return doupdate; } set { doupdate = value; } }

        //Reporting
        private static bool doemailreport;
        public static bool doEmailReport { get { return doemailreport; } set { doemailreport = value; } }

        private static string reportrecipient;
        public static string reportRecipient { get { return reportrecipient; } set { reportrecipient = value; } }


        //SMTP
        private static string smtpip;
        public static string smtpIP { get { return smtpip; } set { smtpip = value; } }

        private static string senderaddr;
        public static string senderAddr { get { return senderaddr; } set { senderaddr = value; } }

        private static string smtppass;
        public static string smtpPass { get { return smtppass; } set { smtppass = value; } }


        //Program

        private static string strcurrdir;
        public static string strCurrDir { get { strcurrdir = Directory.GetCurrentDirectory(); return strcurrdir; } set { strcurrdir = value; } }

        private static string settingsfile;
        public static string settingsFile { get { return "TakeABrolly.ini"; } set { settingsfile = value; } }

        private static int interrcount = 0;
        public static int intErrCount { get { return interrcount; } set { interrcount = value; } }

        //Variables

        private static string strapiurl = "Not Set.xml";
        public static string strApiUrl { get { return strapiurl; } set { strapiurl = value; } }

        private static string strxmlname = "Not Set.xml";
        public static string strXmlName { get { return strxmlname; } set { strxmlname = value; } }

        private static string strcity = "Not Set";
        public static string strCity { get { return strcity; } set { strcity = value; } }

        public static ArrayList arrRain = new ArrayList();

        private static double dblrainin = 0;
        public static double dblRainMin { get { return dblrainin; } set { dblrainin = value; } }

        private static int inttimescale = 10;
        public static int intTimescale { get { return inttimescale; } set { inttimescale = value; } }

        private static int intraincount = 0;
        public static int intRainCount { get { return intraincount; } set { intraincount = value; } }

        public static settingsIni iniReader = new settingsIni(strCurrDir + "\\" + settingsFile);


        //Populate settings from ini / other sources
        public static void loadSettings()
        {
            try {
                //Settings
                string strTmp;
                bool bolTmp = false;
                double dblTmp;
                int intTmp;

                if (!File.Exists(strCurrDir + "\\" + settingsFile))
                {
                    createSettings();
                    //settingsIni iniReader = new settingsIni(strCurrDir + "\\" + settingsFile);
                }

                //Debug
                bolTmp = false;
                if (Boolean.TryParse(iniReader.Read("doDebug", "DEBUG"), out bolTmp)) { bolTmp = Convert.ToBoolean(iniReader.Read("doDebug", "DEBUG")); }
                dodebug = bolTmp;

                //SMTP

                smtpip = iniReader.Read("ip", "SMTP");

                strTmp = iniReader.Read("sender", "SMTP");
                senderaddr = (strTmp.Length > 0) ? strTmp : "";

                smtppass = iniReader.Read("pass", "SMTP");

                if (doDebug) { debugLog.insertBranding(); }

                //Logging
                bolTmp = false;
                if (Boolean.TryParse(iniReader.Read("writeErrLog", "LOGGING"), out bolTmp)) { bolTmp = Convert.ToBoolean(iniReader.Read("writeErrLog", "LOGGING")); }
                writtenerrlog = bolTmp;

                bolTmp = false;
                if (Boolean.TryParse(iniReader.Read("emailErrLog", "LOGGING"), out bolTmp)) { bolTmp = Convert.ToBoolean(iniReader.Read("emailErrLog", "LOGGING")); }
                emailerrlog = bolTmp;

                //Settings
                if (doDebug) { debugLog.LogEvent("Read Settings", "");}

                bolTmp = true;
                if (Boolean.TryParse(iniReader.Read("doUpdate", "DEBUG"), out bolTmp)) { bolTmp = Convert.ToBoolean(iniReader.Read("doUpdate", "DEBUG")); }
                doupdate = bolTmp;

                bolTmp = false;
                if (Boolean.TryParse(iniReader.Read("service", "SETTINGS"), out bolTmp)){bolTmp = Convert.ToBoolean(iniReader.Read("service", "SETTINGS"));}
                asservice = bolTmp;

                strTmp = iniReader.Read("apikey", "SETTINGS");
                strapikey = (strTmp.Length > 0) ? strTmp : "";

                strTmp = iniReader.Read("postcode", "SETTINGS");
                strpostcode = (strTmp.Length > 0) ? strTmp : "NN16";

                strTmp = iniReader.Read("country", "SETTINGS");
                strcountrycode = (strTmp.Length > 0) ? strTmp : "GB";

                strTmp = iniReader.Read("baseURL", "SETTINGS");
                strbaseurl = (strTmp.Length > 0) ? strTmp : "http://api.openweathermap.org/data/2.5/";

                dblTmp = 0;
                if (Double.TryParse(iniReader.Read("rainmin", "SETTINGS"), out dblTmp)) { dblrainin = Convert.ToDouble(iniReader.Read("rainmin", "SETTINGS")); }

                intTmp = 0;
                if (Int32.TryParse(iniReader.Read("timescale", "SETTINGS"), out intTmp)) { intTimescale = Convert.ToInt32(iniReader.Read("timescale", "SETTINGS")); }


                //Reporting
                bolTmp = false;
                if (Boolean.TryParse(iniReader.Read("emailReport", "REPORTING"), out bolTmp)) { bolTmp = Convert.ToBoolean(iniReader.Read("emailReport", "REPORTING")); }
                doemailreport = bolTmp;

                strTmp = iniReader.Read("recipient", "REPORTING");
                reportrecipient = (strTmp.Length > 0) ? strTmp : "agregory@lsh.co.uk";

                if (doDebug) {
                    debugLog.LogEvent("Load Settings ini: " + strCurrDir + "\\" + settingsFile, "");
                    debugLog.LogEvent("Hide Console (service): " + asservice, "");
                    debugLog.LogEvent("Debug: " + doDebug, "");
                    debugLog.LogEvent("Update: " + doUpdate, "");
                    debugLog.LogEvent("Local Logging: " + writtenErrLog, "");
                    debugLog.LogEvent("Email Logging: " + emailErrLog, "");
                    debugLog.LogEvent("API Key: " + strApiKey, "");
                    debugLog.LogEvent("API URL: " + strBaseURL, "");
                    debugLog.LogEvent("Area: " + strPostCode, "");
                    debugLog.LogEvent("Country: " + strCountryCode, "");
                    debugLog.LogEvent("Threshold: " + dblRainMin, "");
                    debugLog.LogEvent("Timescale: " + intTimescale, "");
                    debugLog.LogEvent("Settings Loaded.", "");
                }


            }
            catch (Exception ex)
            {
                debugLog.logError("Exception: "+ex.Message, "loadSettings");
            }

        }

        //Create default settings if none exist
        private static void createSettings()
        {

            try
            {
                StreamWriter SW;
                SW = File.AppendText(strCurrDir + "/TakeABrolly.ini");
                SW.WriteLine("[SETTINGS]");
                SW.WriteLine("service=false");//Run with no console (as service / schedule)
                SW.WriteLine("apikey=");//openweathermap.org API key e989e4dc842f0644076bdd184d07d8a1
                SW.WriteLine("postcode=NN16");//Postcode of target location
                SW.WriteLine("country=GB");//Open Weather Country Code e.g. GB / US
                SW.WriteLine("baseURL=http://api.openweathermap.org/data/2.5/");//Open Weather API URL
                SW.WriteLine("rainmin=0.01");//Rain mm per 3 Hours threshold to activate.
                SW.WriteLine("timescale=10");//Hours from runtime to look for rain
                SW.WriteLine("");
                SW.WriteLine("[DEBUG]");
                SW.WriteLine("doDebug=true");//Additional debugging
                SW.WriteLine("doUpdate=true");//Update XML from API each runtime. False will use local copy of xml data.
                SW.WriteLine("");
                SW.WriteLine("[LOGGING]");
                SW.WriteLine("writeErrLog=true");//Write error / debug log to file
                SW.WriteLine("emailErrLog=true");//Email errors to developer
                SW.WriteLine("");
                SW.WriteLine("[REPORTING]");
                SW.WriteLine("emailReport=true");//Send email
                SW.WriteLine("recipient=andrew@andgregor.co.uk");//Recipient
                SW.WriteLine("");
                SW.WriteLine("[SMTP]");
                SW.WriteLine("ip=weatherapp.andgregor.co.uk");//Mail server
                SW.WriteLine("sender=auth@andgregor.co.uk");//From address
                SW.WriteLine("pass=");//Credentials (if required by server)
                SW.Close();

                debugLog.LogEvent("Settings Missing, Creating Defaults.", "APPLICATION");
                debugLog.LogEvent("Edit TakeABrolly.ini then restart application.", "APPLICATION");
            }
            catch(Exception ex)
            {
                debugLog.logError("Exception: " + ex.Message, "createSettings");
            }
 
        }

        
    }
}
