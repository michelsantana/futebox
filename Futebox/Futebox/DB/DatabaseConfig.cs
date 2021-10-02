using Futebox.DB.Interfaces;

namespace Futebox.DB
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public string Connection { get; set; }
        public string ConnectionString() { return this.Connection; }
        public DatabaseConfig(string conn)
        {
            this.Connection = conn;
        }
    }
}
