using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KendoMvcDemo.ViewModels
{
    public class FileConvertViewModel
    {
        [DisplayName("Upload file")]
        [Required(ErrorMessage = "Please upload a PDF file")]
        public IFormFile ImageFile { get; set; }
    }
}
