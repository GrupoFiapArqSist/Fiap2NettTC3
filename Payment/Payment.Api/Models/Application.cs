public class Application
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string WebhookUrl { get; set; }
    public string ApiKey { get; set; }

    public Application(string username, string password, string webhookUrl)
    {
        Username = username;
        Password = password;
        WebhookUrl = webhookUrl;
    }
}