using System.ComponentModel;

namespace KendoMvcDemo.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StudentNumber { get; set; }

        [DisplayName("Photo")]
        public string AvatarImageFileUrl { get; set; }
    }
}
