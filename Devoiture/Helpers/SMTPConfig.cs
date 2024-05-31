namespace Devoiture.Helpers
{
    public class SMTPConfig
    {
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool IsHtml { get; set; }
    }
}
