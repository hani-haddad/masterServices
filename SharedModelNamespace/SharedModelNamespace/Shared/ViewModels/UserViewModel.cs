using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SharedModelNamespace.Shared.ViewModels
{
    public class UserViewModel
    {
        [BsonElement("Id")]
        public string Id { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Phone")]
        public string Phone { get; set; }
    }
}
