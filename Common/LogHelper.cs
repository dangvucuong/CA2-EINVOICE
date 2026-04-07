using Common;
using Dapper;


namespace WebApp
{
    public static class LogHelper
    {


        public static void Write(string ActionName, string ControllerName, string Parameters, string Description)
        {
            var objUser = new SYS_User_Log();
            //if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session["User"] != null)
            //{
            //    objUser = System.Web.HttpContext.Current.Session["User"].Map<SYS_User_Log>();
            //}
            var IP = "";
            //if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null)
            //{
            //    IP = System.Web.HttpContext.Current.Request.UserHostAddress.MapStr();
            //}
            var param = new DynamicParameters();
            param.Add("@UserID", objUser.ID);
            param.Add("@UserName", objUser.UserName.ConvertToString());
            param.Add("@FullName", objUser.FullName.ConvertToString());
            param.Add("@LogTime", DateTime.Now);
            param.Add("@IP", IP);
            param.Add("@ActionName", ActionName);
            param.Add("@ControllerName", ControllerName);
            param.Add("@Parameters", Parameters);
            param.Add("@Description", Description);
            param.Add("@ID", null, System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue);
            Connection.ExcuteScalar("SYS_Log_Insert", param, "ID");
        }


    }
    public class SYS_User_Log
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public SYS_User_Log()
        {
            this.Email = string.Empty;
            this.Phone = string.Empty;
            this.UserName = string.Empty;
            this.FullName = string.Empty;
        }

    }
}
