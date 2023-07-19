using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JPGame.qa
{
    public partial class mcardsea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Khởi tạo đối tượng DBEntities
                using (DBEntities db = new DBEntities())
                {
                    // Thực hiện truy vấn hoặc tương tác với cơ sở dữ liệu
                    var memberCards = db.MemberCards.ToList();
                    // Ví dụ: Lấy danh sách tất cả khách hàng từ bảng Customers và gán vào một biến customers.
                    // Để hiển thị dữ liệu này trong file .aspx, bạn có thể sử dụng các điều khiển như Repeater, GridView, ...
                }
            }
        }
    }
}