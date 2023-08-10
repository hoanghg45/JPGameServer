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
        public ActionResult GameHistory()
        {
            return View();
        }
        public ActionResult DetailReport()
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

        [HttpGet]
        public JsonResult DataDetailReport(int page = 0,string From = "", string To = "", string cashier="",int shift=0,string type="",int paytype=0)
        {

           
            var dateFrom = new DateTime(2023, 8, 1);
            var dateTo = new DateTime(2030, 8, 10);
            if(!string.IsNullOrEmpty(From))
                dateFrom = DateTime.ParseExact(From, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(To))
                dateTo = DateTime.ParseExact(To, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //var dateFrom = new DateTime(2023, 8, 3);
            //var dateTo = new DateTime(2023, 8, 10);
          

            var records = db.MemberCardChargeRecords
                        .Where(r => r.ChargeDate >= dateFrom && r.ChargeDate <= dateTo)
                        .Where(r => r.ChargeDate.Value.Hour >= 9 && r.ChargeDate.Value.Hour <= 22)

                        .Select(r => new
                        {
                            r.RecordID,
                            Date = r.ChargeDate.Value,
                            Shift = (r.ChargeDate.Value.Hour >= 9 && r.ChargeDate.Value.Hour <= 15) ? 1 : 2,
                            r.Cashier,
                            Money = r.Money.Value,
                            Type = r.RecordType,
                            TypePayID = r.PayType.ID,
                            Typepay = r.PayType.Name,
                            RecordType = r.RecordType.Equals("Create")?"Tạo thẻ":"Nạp thẻ",


                        })
                        ;
            records = records.WhereIf(!string.IsNullOrEmpty(cashier), r => r.Cashier.Equals(cashier))
                             .WhereIf(!string.IsNullOrEmpty(type), r => r.Type.Equals(type)) 
                             .WhereIf(paytype != 0, r => r.TypePayID.Equals(paytype))
                             .WhereIf(shift != 0, r => r.Shift == shift);
            var c = records.ToList();
            // Lọc theo loại thẻ (type)


            // tìm kiếm 
          





            //Xử lí phân trang

            //Số dữ liệu trên 1 trang
            int pageSize = 10;
            page = (page > 0) ? page : 1;
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalBill = records.Count();
            float totalNumsize = (totalBill / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;

            records = records.OrderByDescending(d => d.Date).Skip(start).Take(pageSize);

            var fromto = PaginationExtension.FromTo(totalBill, (int)page, pageSize);

            int from = fromto.Item1;
            int to = fromto.Item2;
            return this.Json(
          new
          {
              data = records,
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
        [HttpGet]
        public JsonResult ReportGame(int page=0, string From ="2023-08-01", string To="2030-08-01",string game="-1",string card="-1")
        {
            var dateFrom = int.Parse(getDay(From)) + int.Parse(getMonth(From)) * 30 * int.Parse(getYear(From)); 
            var dateTo = int.Parse(getDay(To)) + int.Parse(getMonth(To)) * 30 * int.Parse(getYear(To)); 
            var data = db.ReportGameHistories.Select(a => new
            {
                a.Id,
                a.IdGame,
                a.SettingGame.Name,
                a.MemberCard.Code39,
                a.SettingGame.Price,
                a.CreateDate,
                a.Status,
            }).Where(x=>x.CreateDate.Value.Day+x.CreateDate.Value.Month*30*x.CreateDate.Value.Year>= dateFrom
                        && x.CreateDate.Value.Day + x.CreateDate.Value.Month * 30 * x.CreateDate.Value.Year<=dateTo);
            if (game != "-1")
            {
               data= data.Where(x => x.IdGame == game);
            }
            if(card != "-1")
            {
                data = data.Where(x => x.Code39 == card);
            }
            //Xử lí phân trang
            var z = data.ToList();
            //Số dữ liệu trên 1 trang
            int pageSize = 10;
            page = (page > 0) ? page : 1;
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalBill = data.Count();
            float totalNumsize = (totalBill / (float)pageSize);

            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            data = data.OrderByDescending(d => d.CreateDate).Skip(start).Take(pageSize);

            var fromto = PaginationExtension.FromTo(totalBill, (int)page, pageSize);

            int from = fromto.Item1;
            int to = fromto.Item2;
            return this.Json(
          new
          {
              data,
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
        [HttpGet]
        public JsonResult List()
        {
            var game = db.SettingGames.Select(a => new
            {
                a.Id,
                a.Name,
            });
            var card = db.MemberCards.Select(a => new
            {
                a.Code39,
            });
            return this.Json(
          new
          {
              game = game,
              card= card

          }
          , JsonRequestBehavior.AllowGet
          );
        }
        [HttpGet]
        public ActionResult ExportToExcelGameHistory(string From, string To, string game, string card)
        {
            From = From == "" ? "2023-08-01" : From;
            To = To == "" ? "2030-08-01" : To;
            game = game == "" ? "-1" : game;
            card = card == "" ? "-1" : card;
            var dateFrom = int.Parse(getDay(From)) + int.Parse(getMonth(From)) * 30 * int.Parse(getYear(From));
            var dateTo = int.Parse(getDay(To)) + int.Parse(getMonth(To)) * 30 * int.Parse(getYear(To));
            var data = db.ReportGameHistories.Select(a => new
            {
                a.Id,
                a.IdGame,
                a.SettingGame.Name,
                a.MemberCard.Code39,
                a.SettingGame.Price,
                a.CreateDate,
                a.Status,
            }).Where(x => x.CreateDate.Value.Day + x.CreateDate.Value.Month * 30 * x.CreateDate.Value.Year >= dateFrom
                        && x.CreateDate.Value.Day + x.CreateDate.Value.Month * 30 * x.CreateDate.Value.Year <= dateTo);
            if (game != "-1")
            {
                data = data.Where(x => x.IdGame == game);
            }
            if (card != "-1")
            {
                data = data.Where(x => x.Code39 == card);
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Thêm tiêu đề cho các cột
                worksheet.Cells[1, 1].Value = "Ngày";
                worksheet.Cells[1, 2].Value = "Máy Game";
                worksheet.Cells[1, 3].Value = "Thẻ";
                worksheet.Cells[1, 4].Value = "Tiền";
               
                // Thêm dữ liệu từ data vào các ô
                for (int i = 0; i < data.ToList().Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data.ToList()[i].CreateDate.Value.Date.ToString();
                    worksheet.Cells[i + 2, 2].Value = data.ToList()[i].Name;
                    worksheet.Cells[i + 2, 3].Value = data.ToList()[i].Code39;
                    worksheet.Cells[i + 2, 4].Value = data.ToList()[i].Price;
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

        [HttpGet]
        public ActionResult ExportDetailReport( string From = "", string To = "", string cashier = "", int shift = 0, string type = "", int paytype = 0)
        {


            var dateFrom = new DateTime(2023, 8, 1);
            var dateTo = new DateTime(2030, 8, 10);
            if (!string.IsNullOrEmpty(From))
                dateFrom = DateTime.ParseExact(From, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(To))
                dateTo = DateTime.ParseExact(To, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //var dateFrom = new DateTime(2023, 8, 3);
            //var dateTo = new DateTime(2023, 8, 10);


            var records = db.MemberCardChargeRecords
                        .Where(r => r.ChargeDate >= dateFrom && r.ChargeDate <= dateTo)
                        .Where(r => r.ChargeDate.Value.Hour >= 9 && r.ChargeDate.Value.Hour <= 22)

                        .Select(r => new
                        {
                            r.RecordID,
                            Date = r.ChargeDate.Value,
                            Shift = (r.ChargeDate.Value.Hour >= 9 && r.ChargeDate.Value.Hour <= 15) ? 1 : 2,
                            r.Cashier,
                            Money = r.Money.Value,
                            Type = r.RecordType,
                            TypePayID = r.PayType.ID,
                            Typepay = r.PayType.Name,
                            RecordType = r.RecordType.Equals("Create") ? "Tạo thẻ" : "Nạp thẻ",


                        })
                        ;
            records = records.WhereIf(!string.IsNullOrEmpty(cashier), r => r.Cashier.Equals(cashier))
                             .WhereIf(!string.IsNullOrEmpty(type), r => r.Type.Equals(type))
                             .WhereIf(paytype != 0, r => r.TypePayID.Equals(paytype))
                             .WhereIf(shift != 0, r => r.Shift == shift);
            var c = records.ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Thêm tiêu đề cho các cột
                worksheet.Cells[1, 1].Value = "Ngày";
                worksheet.Cells[1, 2].Value = "Mã giao dịch";
                
                worksheet.Cells[1, 3].Value = "Ca";
                worksheet.Cells[1,4 ].Value = "Quầy"; 
                worksheet.Cells[1,5].Value = "Tiền";
                worksheet.Cells[1,6].Value = "Loại thanh toán";
                worksheet.Cells[1,7].Value = "Tác vụ";

                // Thêm dữ liệu từ data vào các ô
                for (int i = 0; i < records.ToList().Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = records.ToList()[i].Date.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cells[i + 2, 2].Value = records.ToList()[i].RecordID;
                    worksheet.Cells[i + 2, 3].Value = records.ToList()[i].Shift;
                    worksheet.Cells[i + 2, 4].Value = records.ToList()[i].Cashier;
                    worksheet.Cells[i + 2,  5].Value = records.ToList()[i].Money.ToString("#,##0");
                    worksheet.Cells[i + 2, 6].Value = records.ToList()[i].Typepay;
                    worksheet.Cells[i + 2, 7].Value = records.ToList()[i].RecordType;
                    
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
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "detailreport-data.xlsx");
            }

        }


        public ActionResult ExportToExcel()
        {

            List<Report> data = new List<Report>();
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
                        Shift1 = shift1.Sum(s => s.Money).Value.ToString("#,##0"),
                        Shift2 = shift2.Sum(s => s.Money).Value.ToString("#,##0"),
                        Total = (shift1.Sum(s => s.Money).Value + shift2.Sum(s => s.Money).Value).ToString("#,##0")
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
        public string getDay(string time)
        {
            var day = time.Split('-')[2];
            return day;
        }
        public string getMonth(string time)
        {
            var month = time.Split('-')[1]; 
            return month;
        }
        public string getYear(string time)
        {
            var year = time.Split('-')[0];
            return year;
        }
    }
}