namespace KendoMvcDemo.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Section { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public ICollection<CourseTeacher> Teachers { get; set; } = [];

        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
