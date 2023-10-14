using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using AuthProject.ViewModels;

namespace AuthProject.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }

        [BsonElement("Age")]
        public string Age { get; set; }

        [BsonElement("AdminOfGroups")]
        public List<string> AdminOfGroups { get; set; }

        [BsonElement("MemberInGroups")]
        public List<string> MemberInGroups { get; set; }

        [BsonElement("Image")]
        public string Image { get; set; }

        [BsonElement("SentInvitations")]
        public List<string> SentInvitations { get; set; }

        [BsonElement("RecivedInvitations")]
        public List<string> RecivedInvitations { get; set; }

    }
}