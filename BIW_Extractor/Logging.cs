using System;
using System.Text;
using System.IO;


namespace BIW_Extractor
{
    public static class Logging
    {
    
        public static void CreateLogFile(string logFile){
            if(logFile != null && !File.Exists(logFile))
            File.Create(logFile);
        }

        public static void LogOperation(string operation, string url, string status, string logFile){

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{operation}({DateTime.Now}): {url} -- {status}");
            try
            {
                File.AppendAllText(logFile, sb.ToString());
            }
            catch(Exception e)
            {
            Console.WriteLine(e.Message);
            }
        }
    }
}

