namespace KendoMvcDemo.Core.Entities
{
    public abstract class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AvatarImageFileName { get; set; }
    }
}
