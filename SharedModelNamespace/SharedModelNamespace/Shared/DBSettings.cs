using System;
namespace SharedModelNamespace.Shared
{
    public interface IDBSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class DBSettings : IDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

}

