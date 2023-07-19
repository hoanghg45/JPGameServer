using System.Web.Mvc;

namespace JPGame.Areas.Admin.Extension
{
    public static class NotificationExtention
    {
        public class ResponseData
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }

        public static JsonResult GetJsonResponse(string status, string message)
        {
            ResponseData responseData = new ResponseData
            {
                Status = status,
                Message = message
            };

            return new JsonResult
            {
                Data = responseData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
