using KendoMvcDemo.Core.Models.Teacher;
using System.ComponentModel;

namespace KendoMvcDemo.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Section { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        [DisplayName("Teachers")]
        public IEnumerable<int> SelectedTeacherIds { get; set; } = [];

        public IEnumerable<TeacherDto> Teachers { get; set; } = [];
    }
}
