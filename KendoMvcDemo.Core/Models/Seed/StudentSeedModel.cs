using System.Text.Json.Serialization;

namespace KendoMvcDemo.Core.Models.Seed
{
    public class StudentSeedModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        public string Name => $"{FirstName} {LastName}".Trim();

        [JsonPropertyName("studentNumber")]
        public string StudentNumber { get; set; }
    }
}
