namespace KendoMvcDemo.Core.Models.Seed
{
    public class CourseSeedModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Section { get; set; }

        public IEnumerable<TeacherSeedModel> Teachers { get; set; } = [];

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
