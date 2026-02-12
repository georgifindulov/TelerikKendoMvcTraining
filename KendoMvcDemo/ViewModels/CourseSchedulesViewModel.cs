using KendoMvcDemo.Core.Models.Course;

namespace KendoMvcDemo.ViewModels
{
    public class CourseSchedulesViewModel
    {
        public IEnumerable<CourseDto> Courses { get; set; } = [];
    }
}
