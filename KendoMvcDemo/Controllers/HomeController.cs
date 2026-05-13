using System.Diagnostics;
using KendoMvcDemo.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using KendoMvcDemo.ViewModels;
using KendoMvcDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using KendoMvcDemo.Core.Documents;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KendoMvcDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly UniDbContext db;

        public HomeController(UniDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HomeViewModel viewModel = await BuildDashboardInfoAsync();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DashboardInfo()
        {
            HomeViewModel viewModel = await BuildDashboardInfoAsync();

            await Task.Delay(1000); // Simulate some delay for demo purposes

            return PartialView("_DashboardInfo", viewModel);
        }

        private async Task<HomeViewModel> BuildDashboardInfoAsync()
        {
            HomeViewModel viewModel = new()
            {
                CoursesCount = await db.Courses.CountAsync(),
                StudentsCount = await db.Students.CountAsync(),
                TeachersCount = await db.Teachers.CountAsync()
            };

            return viewModel;
        }

        [HttpGet]
        public IActionResult ExportDynamic([FromServices] IExcelDocumentGenerator excelDocumentGenerator)
        {
            using SqlConnection connection = new(db.Database.GetConnectionString());
            using SqlDataAdapter adapter = new(
                @"SELECT c.Name, COUNT(*) AS [Number Of Enrolled Students] FROM Courses c
                JOIN Enrollments e ON c.Id = e.CourseId
                GROUP BY c.Name", 
                connection);

            DataTable dataTable = new();
            adapter.Fill(dataTable);

            byte[] excelData = excelDocumentGenerator.Export(dataTable);

            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelDynamicExport.xlsx");
        }
    }
}
