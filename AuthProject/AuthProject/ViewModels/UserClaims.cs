using Newtonsoft.Json;
using System.Collections.Generic;
namespace AuthProject.ViewModels
{
    public class UserClaims
    {
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Age")]
        public string Age { get; set; }

        [JsonProperty("AdminOfGroups")]
        public List<string> AdminOfGroups { get; set; }

        [JsonProperty("MemberInGroups")]
        public List<string> MemberInGroups { get; set; }

        [JsonProperty("Image")]
        public string Image { get; set; }

        [JsonProperty("SentInvitations")]
        public List<string> SentInvitations { get; set; }

        [JsonProperty("RecivedInvitations")]
        public List<string> RecivedInvitations { get; set; }
    }
}
