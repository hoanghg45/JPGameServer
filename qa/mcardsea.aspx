<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/qa/mcardsea.aspx.cs" Inherits="JPGame.qa.mcardsea" %>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/json";

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        dynamic data = new System.Dynamic.ExpandoObject();
        // parameter receive from URL
        data.cardid = Request.QueryString["cardid"];
        data.mjihao = Request.QueryString["mjihao"];
        data.cjihao = Request.QueryString["cjihao"];
        data.status = Convert.ToInt32(Request.QueryString["status"]);
        data.time = Convert.ToInt32(Request.QueryString["time"]);
        data.output = 0;

        // logic code
        int time = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        int time_min = time - 60;
        int time_max = time + 60;

        dynamic res = new System.Dynamic.ExpandoObject();
        if (string.IsNullOrEmpty(data.cardid) || data.status == 0 || data.time == 0 || string.IsNullOrEmpty(data.mjihao) || string.IsNullOrEmpty(data.cjihao))
        {
            // Parameter judgment
            data.status = 0;
            res.code = 1000;
            res.message = "false";
        }
        else
        {
            data.status = 1;
            res.code = 0;
            res.message = "success";
        }

        res.data = new[] { data };
        //// output code
        //string json_data = serializer.Serialize(res);
        //string filePath = Server.MapPath("~/data.txt");

            //using (StreamWriter writer = new StreamWriter(filePath, true))
            //{
            //    writer.WriteLine(json_data);
            //}
            
            //Response.Write(json_data);

    }
</script>

