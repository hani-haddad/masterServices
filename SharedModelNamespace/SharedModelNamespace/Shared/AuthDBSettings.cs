using System;
namespace SharedModelNamespace.Shared
{
    public interface IAuthDBSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class AuthDBSettings : IAuthDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
