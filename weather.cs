using System;
using System.Runtime.InteropServices;

namespace uk.co.andgregor.TakeABrolly
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            try
            {
                if (appSettings.doDebug)
                {
                    debugLog.LogEvent("Init Application", "");
                }

                var handle = GetConsoleWindow();

                //Load Settings
                appSettings.loadSettings();

                //Hide Console ?
                if (appSettings.asService) { ShowWindow(handle, SW_HIDE); } else { ShowWindow(handle, SW_SHOW); }

                //Build API String
                weatherXML.buildURL();

                //Add pause into console to review settings
                if (appSettings.doDebug && !appSettings.asService)
                {
                    Console.WriteLine("Press any key to continue.");
                    Console.WriteLine("Press ctrl + c to exit.");
                    Console.ReadKey();
                }

                //Set XML File Name
                appSettings.strXmlName = appSettings.strPostCode + ".xml";

                //Generate XML - Result for future usage
                int intResult = 0;
                if (appSettings.doUpdate)
                {
                    intResult = weatherXML.generateXML();
                }

                if (appSettings.doDebug)
                {
                    debugLog.LogEvent("XML Generated with result: " + intResult, "");
                }

                //Populate common variables from parsed XML
                weatherXML.populateFromXML();

                //Email forecast if any - Take Your Brolly.
                reportReults();


                //Add pause at end for debug mode
                if (appSettings.doDebug)
                {
                    if (!appSettings.asService)
                    {
                        Console.WriteLine("Press any key to exit.");
                        Console.ReadKey();
                    }

                }

            }
            catch(Exception ex)
            {
                debugLog.logError("Exception: " + ex.Message, "Main");
            }
        }

        //Final function - Format email and send if rain predicted.
        private static void reportReults()
        {
            try
            {
                string strEmail = "<html>";
                string strSubject = "No need for a brolly today. | Take a Brolly App";
                if (appSettings.intRainCount > 0)
                {
                    strSubject = "Take a Brolly today. | Take a Brolly App";
                    strEmail += "<h2>Take a Brolly</h2>";
                    strEmail += "<br />";
                    strEmail += "For <b>" + appSettings.strCity + "</b>";
                    strEmail += "<br />";

                    foreach (string[] strOut in appSettings.arrRain)
                    {
                        strEmail += "Rain: " + strOut[0];
                        strEmail += " | " + strOut[2];
                        strEmail += " (" + strOut[4] + "mm).";
                        strEmail += "  <img src=\"http://openweathermap.org/img/w/" + strOut[3] + ".png\" />";
                        strEmail += "<br />";
                        
                    }
                    strEmail += "<br />";

                    strEmail += "powered by andGregor LTD Take a Brolly App.";
                    strEmail += "</html>";
                    
                }
                else
                {
                    strSubject = "No Brolly required today. | Take a Brolly App";
                    strEmail += "<h2>Leave the Brolly</h2>";
                    strEmail += "<br />";
                    strEmail += "For <b>" + appSettings.strCity + "</b>";
                    strEmail += "<br />";

                    foreach (string[] strOut in appSettings.arrRain)
                    {
                        strEmail += "Rain: " + strOut[0];
                        strEmail += " | " + strOut[2];
                        strEmail += " (" + strOut[4] + "mm).";
                        strEmail += "  <img src=\"http://openweathermap.org/img/w/" + strOut[3] + ".png\" />";
                        strEmail += "<br />";

                    }
                    strEmail += "<br />";
                    strEmail += "<a href=\""+appSettings.strApiUrl+"\">API Link</a>";
                    strEmail += "<br />";
                    strEmail += "No rain reported.<br />";
                    strEmail += "powered by andGregor LTD Take a Brolly App.";
                    strEmail += "</html>";
                }

                debugLog.sendEmail(appSettings.reportRecipient, appSettings.senderAddr, strSubject, strEmail, "");


            }
            catch(Exception ex)
            {
                debugLog.logError("Exception: " + ex.Message, "reportReults");
            }
        }

    }
}
