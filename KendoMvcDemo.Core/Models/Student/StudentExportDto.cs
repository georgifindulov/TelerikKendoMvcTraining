namespace KendoMvcDemo.Core.Models.Student
{
    public class StudentExportDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StudentNumber { get; set; }

        public string AvatarImageFileName { get; set; }

        public byte[] AvatarImageData { get; set; }
    }
}
