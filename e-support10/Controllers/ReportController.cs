using e_support10.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace e_support10.Controllers
{
    public class ReportController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async  Task<IActionResult> SalesReport(string data)
        {
            DateTime dataModifikuar;

            try
            {
                dataModifikuar = DateTime.ParseExact(data, "yyyy-MM-ddTHH:mm", null);
                dataModifikuar = new DateTime(dataModifikuar.Year, dataModifikuar.Month, 1, 0, 0, 0);
            }
            catch (Exception)
            {
                // return Json(new {a="Ka problem data", b=dataF,c=dataFillimit,d=dataFundit,g=dataM  });
                dataModifikuar = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
            }
            var applicationDbContext = _context.SalesReports.FromSqlInterpolated($"EXEC    [dbo].[MonthlySales] @data={dataModifikuar}");

            return View(await applicationDbContext.ToListAsync());


            
        }

        public async Task<IActionResult> SuppliesReport(string data)
        {
            DateTime dataModifikuar;

            try
            {
                dataModifikuar = DateTime.ParseExact(data, "yyyy-MM-ddTHH:mm", null);
                dataModifikuar = new DateTime(dataModifikuar.Year, dataModifikuar.Month, 1, 0, 0, 0);
            }
            catch (Exception)
            {
                // return Json(new {a="Ka problem data", b=dataF,c=dataFillimit,d=dataFundit,g=dataM  });
                dataModifikuar = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
            }
            var applicationDbContext = _context.SalesReports.FromSqlInterpolated($"EXEC    [dbo].[MonthlySupplies] @data={dataModifikuar}");

            return View(await applicationDbContext.ToListAsync());



        }
    }
}
