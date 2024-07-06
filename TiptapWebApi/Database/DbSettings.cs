namespace TiptapWebApi.Database
{
    public class DbSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Database { get; set; } = null!;
    }
}
