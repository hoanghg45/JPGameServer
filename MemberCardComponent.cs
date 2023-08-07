using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JPGame
{
    public class MemberCardComponent
    {
        //Thay đổi mỗi khi quét thẻ (bảng Livecards) 
        string ReaderID;
        public void RegisterNotification(string readerID)
        {
            ReaderID = readerID;
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT [ID]
                                      ,[CardID] 
                                      ,[ScanAt]
                                      ,[ReaderID]
                                  FROM [dbo].[LiveCards] WHERE [ReaderID] = @ReaderID";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@ReaderID", readerID);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        private void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                //SqlDependency sqlDep = sender as SqlDependency;
                //sqlDep.OnChange -= RequestsqlDep_OnChange;

                //from here we will send notification message to client
                var membercardHub = GlobalHost.ConnectionManager.GetHubContext<MembercardHub>();
                membercardHub.Clients.All.notify("cardscanned");
                //re-register notification
                RegisterNotification(ReaderID);
            }
        }

    
}
}