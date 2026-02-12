using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoMvcDemo.Infrastructure.Database;
using KendoMvcDemo.ViewModels;
using KendoMvcDemo.ViewModels.Charts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KendoMvcDemo.Controllers
{
    public class ChartsController : Controller
    {
        private readonly KaustDbContext db;

        public ChartsController(KaustDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> TopFiveBestCourses()
        {
            return Json(await GenerateTopFiveCoursesChartData());
        }

        private async Task<IEnumerable<TopCoursesChartViewModel>> GenerateTopFiveCoursesChartData()
        {
            Dictionary<int, string> courses = await db.Courses
                .Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToDictionaryAsync(x => x.Id, x => x.Name);

            var data = await db.Enrollments
                .GroupBy(x => x.CourseId)
                .OrderByDescending(x => x.Count())
                .Take(5)
                .Select(x => new
                {
                    CourseId = x.Key,
                    StudentsCount = x.Count()
                }).ToListAsync();

            List<TopCoursesChartViewModel> topFiveBestCoursesData = data
                .Select(x => new TopCoursesChartViewModel
                {
                    CourseName = courses[x.CourseId],
                    StudentsCount = x.StudentsCount
                }).ToList();

            return topFiveBestCoursesData;
        }
    }
}
