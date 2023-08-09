using JPGame.Areas.Admin.Extension;
using NinjaNye.SearchExtensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JPGame.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        private DBEntities db = new DBEntities();
        // GET: Admin/Report
        public ActionResult Index()
        {
            return View();
        }
        public class Report
        {
            public DateTime Date;
            public string Shift1;
            //public float Revenue1;
            //public string Cashier1;
            public string Shift2;
            //public float Revenue2;
            //public string Cashier2;
            public string Total;
            

        }

        [HttpGet]
        public JsonResult DataTable(int page = 0,string From = "", string To = "")
        {

            List<Report> data = new List<Report>();
            var dateFrom = new DateTime(2023, 8, 1);
            var dateTo = new DateTime(2030, 8, 10);
            if(!string.IsNullOrEmpty(From))
             dateFrom = DateTime.ParseExact(From, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(To))
                dateTo = DateTime.ParseExact(To, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //var dateFrom = new DateTime(2023, 8, 3);
            //var dateTo = new DateTime(2023, 8, 10);
          
            var records = db.MemberCardChargeRecords.ToList();
            TimeSpan duration = dateTo - dateFrom;
            int numberOfDays = duration.Days;
            for (int i = 0; i <= numberOfDays; i++)
            {
                
                var date = dateFrom.AddDays(i);
                var a = records.ToList();
                // Nếu lịch sử nạp tiền có trong khoảng
                var record = records.Where(r => r.ChargeDate >= date && r.ChargeDate < date.AddHours(23).AddMinutes(59));
                if (record.Any())
                {
                    var shift1 = record.Where(r => r.ChargeDate.Value.Hour >= 9 && r.ChargeDate.Value.Hour <= 15);
                    var shift2 = record.Where(r => r.ChargeDate.Value.Hour >= 15 && r.ChargeDate.Value.Hour <= 23);


                    var report = new Report
                    {
                        Date = date,
                        Shift1 = shift1.Sum(s => s.Money).ToString(),
                        Shift2 = shift2.Sum(s => s.Money).ToString(),
                        Total = (shift1.Sum(s => s.Money).Value+ shift2.Sum(s => s.Money).Value).ToString()
                    };
                    data.Add(report);
                }
                
            }
            var c = data.ToList();
            // Lọc theo loại thẻ (type)


            // tìm kiếm 
          





            //Xử lí phân trang

            //Số dữ liệu trên 1 trang
            int pageSize = 10;
            page = (page > 0) ? page : 1;
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalBill = data.Count();
            float totalNumsize = (totalBill / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            IEnumerable<Report> filteredReports = data;
            filteredReports = filteredReports.OrderByDescending(d => d.Date).Skip(start).Take(pageSize);

            var fromto = PaginationExtension.FromTo(totalBill, (int)page, pageSize);

            int from = fromto.Item1;
            int to = fromto.Item2;
            return this.Json(
          new
          {
              data =filteredReports,
              pageCurrent = page,
              numSize,
              total = totalBill,
              size = pageSize,
              from,
              to

          }
          , JsonRequestBehavior.AllowGet
          );
        }
        public ActionResult ExportToExcel()
        {
           
             List < Report > data = new List<Report>();
            var dateFrom = new DateTime(2023, 8, 1);
            var dateTo = new DateTime(2030, 8, 10);
          
            //var dateFrom = new DateTime(2023, 8, 3);
            //var dateTo = new DateTime(2023, 8, 10);

            var records = db.MemberCardChargeRecords.ToList();
            TimeSpan duration = dateTo - dateFrom;
            int numberOfDays = duration.Days;
            for (int i = 0; i <= numberOfDays; i++)
            {

                var date = dateFrom.AddDays(i);
                var a = records.ToList();
                // Nếu lịch sử nạp tiền có trong khoảng
                var record = records.Where(r => r.ChargeDate >= date && r.ChargeDate < date.AddHours(23).AddMinutes(59));
                if (record.Any())
                {
                    var shift1 = record.Where(r => r.ChargeDate.Value.Hour >= 9 && r.ChargeDate.Value.Hour <= 15);
                    var shift2 = record.Where(r => r.ChargeDate.Value.Hour >= 15 && r.ChargeDate.Value.Hour <= 23);


                    var report = new Report
                    {
                        Date = date,
                        Shift1 = shift1.Sum(s => s.Money).ToString(),
                        Shift2 = shift2.Sum(s => s.Money).ToString(),
                        Total = (shift1.Sum(s => s.Money).Value + shift2.Sum(s => s.Money).Value).ToString()
                    };
                    data.Add(report);
                }

            } // Lấy dữ liệu từ cơ sở dữ liệu
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Thêm tiêu đề cho các cột
                worksheet.Cells[1, 1].Value = "Ngày";
                worksheet.Cells[1, 2].Value = "Doanh số ca 1";
                worksheet.Cells[1, 3].Value = "Doanh số ca 2";
                worksheet.Cells[1, 4].Value = "Tổng doanh số ngày";
               
                // Thêm dữ liệu từ data vào các ô
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].Date.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + 2, 2].Value = data[i].Shift1;
                    worksheet.Cells[i + 2, 3].Value = data[i].Shift2;
                    worksheet.Cells[i + 2, 4].Value = data[i].Total;
                }
                var headerCells = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns];
                worksheet.View.FreezePanes(2, 1);
                // Set their text to bold, italic and underline.
                headerCells.Style.Font.Bold = true;
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                worksheet.Cells["A:AZ"].AutoFitColumns();
                var range = worksheet.Cells[worksheet.Dimension.Address];
                range.AutoFilter = true;
                ///Setting thêm cho sheet detail
                //Select only the header cells
                // Lưu package thành file Excel
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report-data.xlsx");
            }
        }
    }
}