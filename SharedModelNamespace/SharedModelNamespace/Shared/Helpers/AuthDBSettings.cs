using System;
namespace SharedModelNamespace.Shared.Helpers
{
    public interface IAuthDBSettings
    {
        string ConnectionString { get; set; }
        string UsersCollectionName { get; set; }
        string DatabaseName { get; set; }
    }
    public class AuthDBSettings : IAuthDBSettings
    {
        public string ConnectionString { get; set; }
        public string UsersCollectionName { get; set; }
        public string DatabaseName { get; set; }
    }
}
