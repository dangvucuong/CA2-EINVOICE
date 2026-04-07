namespace WebApp
{
    public static class LogWriter
    {
        private static string m_exePath = string.Empty;
        public static void SetPath(string path)
        {
            m_exePath = path;
        }
        public static void Writer(string logMessage, string url, string userID)
        {
            LogWrite(logMessage, url, userID);
        }
        public static void LogWrite(string logMessage, string url, string userID)
        {
            try
            {
                var fileName = "log_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
                fileName = fileName + ".txt";
                string directoryPath = Path.GetDirectoryName(@"log\" + fileName);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (StreamWriter w = File.AppendText(@"log\" + fileName))
                {
                    Log(logMessage, w, url, userID);
                }
            }
            catch
            {
            }
        }

        public static void Log(string logMessage, TextWriter txtWriter, string url, string userID)
        {
            try
            {
                txtWriter.Write("\r\nTime: ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("User:{0}", userID);
                txtWriter.WriteLine("Url:{0}", url);
                txtWriter.WriteLine("Message:{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch
            {
            }
        }
    }
}