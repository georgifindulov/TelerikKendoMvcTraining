using System.Diagnostics;
using KendoMvcDemo.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using KendoMvcDemo.ViewModels;
using KendoMvcDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace KendoMvcDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly KaustDbContext db;

        public HomeController(KaustDbContext db)
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
                InstructorsCount = await db.Teachers.CountAsync()
            };

            return viewModel;
        }

        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest request)
        {
            var result = Enumerable.Range(1, 50).Select(i => new Customer
            {
                Id = i,
                CompanyName = "Company Name " + i,
                ContactName = "Contact Name " + i,
                ContactTitle = "Contact Title " + i,
                Country = "Country " + i
            });

            var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult);
        }
    }
}
