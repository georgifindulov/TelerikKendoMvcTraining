namespace KendoMvcDemo.Core.Entities
{
    public class Student : User
    {
        public string StudentNumber { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
