using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace SocialMediaWeb
{
    public class ErrorLog
    {
        public void LogError(Exception exception)
        {
            string logDirectory = HttpContext.Current.Server.MapPath("~/Log"); 
            string logFileName = $"ErrorLog_{DateTime.Now.ToString("yyyyMMdd")}.txt"; 
            string logFilePath = Path.Combine(logDirectory, logFileName);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory); 
            }

            using (StreamWriter streamWriter = new StreamWriter(logFilePath, true)) 
            {
                streamWriter.WriteLine("========================================");
                streamWriter.WriteLine($"Timestamp: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                streamWriter.WriteLine($"Message: {exception.Message}");

                if (exception.InnerException != null)
                {
                    streamWriter.WriteLine($"Inner Exception Message: {exception.InnerException.Message}");
                    streamWriter.WriteLine($"Inner Exception Stack Trace: {exception.InnerException.StackTrace}");
                }

                streamWriter.WriteLine($"Stack Trace: {exception.StackTrace}");
                streamWriter.WriteLine("========================================");
            }
        }
    }
}