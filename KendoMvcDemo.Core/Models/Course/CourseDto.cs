namespace KendoMvcDemo.Core.Models.Course
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime Start { get;set; }

        public DateTime? End { get; set; }
    }
}
