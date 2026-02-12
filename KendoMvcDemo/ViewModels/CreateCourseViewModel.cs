using Kendo.Mvc.Extensions;
using KendoMvcDemo.Core.Models.Teacher;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KendoMvcDemo.ViewModels
{
    public class CreateCourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a code")]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Section { get; set; }

        [Required]
        public DateOnly Start { get; set; }

        public DateOnly? End { get; set; }

        [DisplayName("Teachers")]
        [Required(ErrorMessage = "Please select at least one teacher")]
        public IEnumerable<int> SelectedTeacherIds { get; set; }

        public IEnumerable<TeacherDto> Teachers { get; set; } = [];

        [DisplayName("Upload image")]
        [Required(ErrorMessage = "Please upload an image with minimum size of 50KB and maximum size of 1MB")]
        public IFormFile ImageFile { get; set; }

        public virtual CourseViewModel ConvertToCourseViewModel(IEnumerable<TeacherDto> teachers) => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Section = Section,
            Start = Start.ToDateTime(),
            End = End?.ToDateTime(),
            SelectedTeacherIds = SelectedTeacherIds,
            Teachers = teachers
        };
    }
}
