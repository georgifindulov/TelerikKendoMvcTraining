using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoMvcDemo.Core.Documents;
using KendoMvcDemo.Core.Models.Student;
using KendoMvcDemo.Infrastructure.Database;
using KendoMvcDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace KendoMvcDemo.Controllers
{
    public class StudentsController : Controller
    {
        private readonly KaustDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;

        public StudentsController(KaustDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(int id = 1)
        {
            if (id < 1 || id > 2)
            {
                id = 1;
            }

            ViewBag.ActiveTabNumber = id;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListView()
        {
            IEnumerable<StudentViewModel> students = await db.Students
                .Select(x => new StudentViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    StudentNumber = x.StudentNumber,
                    AvatarImageFileUrl = x.AvatarImageFileName
                }).ToListAsync();

            foreach (StudentViewModel student in students)
            {
                student.AvatarImageFileUrl = $"{Request.Scheme}://{Request.Host}/images/{student.AvatarImageFileUrl ?? "default-avatar.jpg"}";
            }

            return PartialView("_List", students);
        }

        [HttpGet]
        public IActionResult SpreadsheetView()
        {
            return PartialView("_Spreadsheet");
        }

        [HttpGet]
        public async Task<JsonResult> LoadSpreadsheetData([DataSourceRequest] DataSourceRequest request)
        {
            List<StudentSpreadsheetViewModel> students = await db.Students
                .Select(x => new StudentSpreadsheetViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Number = x.StudentNumber,
                    Avatar = x.AvatarImageFileName
                }).ToListAsync();

            foreach (StudentSpreadsheetViewModel student in students)
            {
                student.Avatar = $"{Request.Scheme}://{Request.Host}/images/{student.Avatar ?? "default-avatar.jpg"}";
            }

            return Json(students.ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf([FromServices] IPdfDocumentGenerator<StudentExportDto> studentsPdfGenerator)
        {
            List<StudentExportDto> students = await db.Students
                .Select(x => new StudentExportDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StudentNumber = x.StudentNumber,
                    AvatarImageFileName = x.AvatarImageFileName
                }).ToListAsync();

            foreach (StudentExportDto student in students)
            {
                student.AvatarImageFileName ??= "default-avatar.jpg";

                string avatarFilePath = Path.Combine(hostEnvironment.WebRootPath, "images", student.AvatarImageFileName);

                student.AvatarImageData = System.IO.File.ReadAllBytes(avatarFilePath);
            }

            byte[] pdfData = studentsPdfGenerator.Export(students);

            return File(pdfData, "application/pdf", "Students.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadExcel([FromServices] IExcelDocumentGenerator<StudentExportDto> studentsExcelGenerator)
        {
            List<StudentExportDto> students = await db.Students
                .Select(x => new StudentExportDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StudentNumber = x.StudentNumber,
                    AvatarImageFileName = x.AvatarImageFileName
                }).ToListAsync();

            foreach (StudentExportDto student in students)
            {
                student.AvatarImageFileName ??= "default-avatar.jpg";

                string avatarFilePath = Path.Combine(hostEnvironment.WebRootPath, "images", student.AvatarImageFileName);

                student.AvatarImageData = System.IO.File.ReadAllBytes(avatarFilePath);
            }

            byte[] excelData = studentsExcelGenerator.Export(students);

            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");
        }


        [HttpGet]
        public async Task<IActionResult> DownloadWord([FromServices] IWordDocumentGenerator<StudentExportDto> studentsWordGenerator)
        {
            List<StudentExportDto> students = await db.Students
                .Select(x => new StudentExportDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StudentNumber = x.StudentNumber,
                    AvatarImageFileName = x.AvatarImageFileName
                }).ToListAsync();

            foreach (StudentExportDto student in students)
            {
                student.AvatarImageFileName ??= "default-avatar.jpg";

                string avatarFilePath = Path.Combine(hostEnvironment.WebRootPath, "images", student.AvatarImageFileName);

                student.AvatarImageData = System.IO.File.ReadAllBytes(avatarFilePath);
            }

            byte[] excelData = studentsWordGenerator.Export(students);

            return File(excelData, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Students.docx");
        }
    }
}
