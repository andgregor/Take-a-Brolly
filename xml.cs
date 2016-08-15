using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace uk.co.andgregor.TakeABrolly
{
    class weatherXML
    {
        //Call API URL and save http stream response as local XML file.
        public static int generateXML()
        {
            try
            {
                string strXMLName = appSettings.strXmlName;
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(appSettings.strApiUrl);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)updateRequest.GetResponse();
                Stream receiveStream = myHttpWebResponse.GetResponseStream();

                if (File.Exists(strXMLName))
                {
                    File.Delete(strXMLName);
                }
                if (receiveStream != null)
                {

                    using (Stream output = File.OpenWrite(strXMLName))
                    using (Stream input = receiveStream)
                    {
                        input.CopyTo(output);
                    }
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                debugLog.logError("Exception: " + ex.Message, "generateXML");
                return -1;
            }

        }

        //Parse stored XML and populate relevant variables
        public static void populateFromXML()
        {
            try
            {
                XElement weatherData = XElement.Load(appSettings.strXmlName);
                appSettings.strCity = weatherData.Element("location").Element("name").Value.ToString();
                string strTmpName = "";
                string strTmpVar = "";
                

                //Loop through forecasts
                foreach (XElement childEllement in weatherData.Descendants("time"))
                {
                    //Log time attributes
                    DateTime dtStart = DateTime.ParseExact(childEllement.Attribute("from").Value.ToString(), "yyyy-MM-ddTHH:mm:ss", null);
                    DateTime dtFinish = DateTime.ParseExact(childEllement.Attribute("to").Value.ToString(), "yyyy-MM-ddTHH:mm:ss", null);

                    //Loop through each forecast element(s)
                    foreach (XElement childEl in childEllement.Descendants())
                    {
                        if (childEl.Name.ToString() == "symbol")
                        {
                            if (childEl.Attribute("name") != null)
                            {
                                strTmpName = childEl.Attribute("name").Value.ToString();
                                strTmpVar = childEl.Attribute("var").Value.ToString();
                            }
                        }

                        //If rain found
                        if (childEl.Name.ToString() == "precipitation")
                        {
                            if (childEl.Attribute("value") != null)
                            {
                                //Populate temporary string array with data we are looking for
                                if (Convert.ToDouble(childEl.Attribute("value").Value) > appSettings.dblRainMin && (dtStart - DateTime.Now).TotalHours < appSettings.intTimescale)
                                {
                                    String[] arrTmpRain = new String[6];
                                    arrTmpRain[0] = dtStart.ToString();
                                    arrTmpRain[1] = dtFinish.ToString();
                                    arrTmpRain[2] = strTmpName;
                                    arrTmpRain[3] = strTmpVar;
                                    arrTmpRain[4] = childEl.Attribute("value").Value.ToString();
                                    arrTmpRain[5] = childEl.Attribute("type").Value.ToString();

                                    appSettings.arrRain.Add(arrTmpRain);
                                    //Array.Clear(arrTmpRain, 0, arrTmpRain.Length);
                                    

                                    if (appSettings.doDebug)
                                    {
                                        debugLog.LogEvent("Hours: " + (dtStart - DateTime.Now).TotalHours, "");
                                        debugLog.LogEvent("Threshold: " + Convert.ToDouble(childEl.Attribute("value").Value) + ">" + appSettings.dblRainMin, "");
                                        debugLog.LogEvent("From: " + dtStart.ToString(), "");
                                        debugLog.LogEvent("To: " + dtFinish.ToString(), "");
                                        debugLog.LogEvent("Value: " + childEl.Attribute("value").Value.ToString(), "");
                                        debugLog.LogEvent("Type: " + childEl.Attribute("type").Value.ToString(), "");
                                    }
                                }
                                
                            }
                        }
                        
                    }
                    
                }

                //Add 3 hourly data to overall rain array
                appSettings.intRainCount = appSettings.arrRain.Count;

                if (appSettings.doDebug)
                {
                    debugLog.LogEvent("City: " + appSettings.strCity, "");
                    debugLog.LogEvent("Rain Count: " + appSettings.arrRain.Count, "");
                    foreach(string[] strOut in appSettings.arrRain)
                    {
                        debugLog.LogEvent("Array: " + strOut[1], "");
                    }

                }
            }
            catch (Exception ex)
            {
                debugLog.logError("Exception: " + ex.Message, "populateFromXML");
            }
        }

        //Build API URL from settings and defaults
        public static void buildURL()
        {
            string strOut = appSettings.strBaseURL;
            strOut += "forecast?";
            strOut += "zip=" + appSettings.strPostCode.Replace(" ", "%20");
            strOut += "," + appSettings.strCountryCode;
            strOut += "&APPID=" + appSettings.strApiKey;
            strOut += "&mode=XML";
            if (appSettings.doDebug)
            {
                debugLog.LogEvent("URL Built: " + strOut, "");
            }
            appSettings.strApiUrl = strOut;
        }
    }
}
