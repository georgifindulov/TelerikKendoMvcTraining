namespace KendoMvcDemo.Core.Entities
{
    public class Teacher : User
    {
        public ICollection<CourseTeacher> Courses { get; set; } = [];
    }
}
