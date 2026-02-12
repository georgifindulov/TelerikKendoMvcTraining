using KendoMvcDemo.Core.Documents;
using KendoMvcDemo.Infrastructure.Database;
using KendoMvcDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KendoMvcDemo.Controllers
{
    public class ConvertController : Controller
    {
        private readonly KaustDbContext db;
        private readonly IPdfDocumentGenerator pdfDocumentGenerator;

        public ConvertController(KaustDbContext db, IPdfDocumentGenerator pdfDocumentGenerator)
        {
            this.db = db;
            this.pdfDocumentGenerator = pdfDocumentGenerator;
        }

        [HttpGet]
        public IActionResult File()
        {
            FileConvertViewModel viewModel = new();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult File(FileConvertViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string fileName = Path.GetFileNameWithoutExtension(viewModel.ImageFile.FileName);
            string fileExtension = Path.GetExtension(viewModel.ImageFile.FileName);

            if (fileExtension != ".docx")
            {
                return View(viewModel);
            }

            byte[] pdfData = pdfDocumentGenerator.ExportDocxToPdf(viewModel.ImageFile.OpenReadStream());

            return File(pdfData, "application/pdf", $"{fileName}.pdf");
        }
    }
}
