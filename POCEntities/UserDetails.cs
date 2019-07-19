using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCEntities
{
    

    public class LanguageInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LanguageDescription { get; set; }
    }

    public class OfficerProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string OfficerId { get; set; }
        public string WindowsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleId { get; set; }
        public List<string> Specialization { get; set; }
        public List<string> LanguagesKnown { get; set; }
        public string Sex { get; set; }
        public bool IsRosterAdministrator { get; set; }
    }

    public class Duty
    {
        public string DutyName { get; set; }
        public string DutyDescription { get; set; }
        public bool IsCritical { get; set; }
    }

    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Duty> Duty { get; set; }
    }

    public class Specialization
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public string SpecializationDescription { get; set; }
    }
}
