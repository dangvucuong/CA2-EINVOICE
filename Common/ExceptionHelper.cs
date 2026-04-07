using Dapper;

namespace Common
{
    public static class ExceptionHelper
    {
        private static string m_exePath = string.Empty;

        public static void Write(string logMessage, string URL = "")
        {
            try
            {
                return;
                //var objUser = new SYS_User_Log();
                //if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["User"] != null)
                //{
                //    objUser = System.Web.HttpContext.Current.Session["User"].Map<SYS_User_Log>();
                //}
                var param = new DynamicParameters();
                param.Add("@ExceptionMessage", logMessage);
                param.Add("@CreateTime", DateTime.Now);
                param.Add("@URL", URL);
                param.Add("@UserName", string.Empty);
                Connection.Excute("Exception_Insert", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SaveLog(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SaveLog(this Exception ex, string URL = "")
        {
            var m = ex.Message;
            ExceptionHelper.Write(ex.Message, URL);
        }
    }

}
