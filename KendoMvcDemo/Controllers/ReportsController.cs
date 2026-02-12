using Microsoft.AspNetCore.Mvc;

namespace KendoMvcDemo.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ReportName = "Students and Courses";
            ViewBag.ReportNameTrdp = "Report1_StudentsAndCourses.trdp";

            return View();
        }
    }
}
