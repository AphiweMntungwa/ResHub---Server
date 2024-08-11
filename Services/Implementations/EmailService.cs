using ResHub.Services.Interfaces;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;

public class EmailService: IEmailService
{
    private readonly string _apiKey = "af588e4379465d6ef330bc1b494cd0a6-afce6020-bc74f9ce"; // Replace with your Mailgun API key
    private readonly string _domain = "sandbox3e973892847c4986925957120b7c657b.mailgun.org"; // Replace with your Mailgun domain

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var options = new RestClientOptions("https://api.mailgun.net/v3")
        {
            Authenticator = new HttpBasicAuthenticator("api", _apiKey),
            ThrowOnAnyError = true,
            Timeout = TimeSpan.FromSeconds(30)
        };

        var client = new RestClient(options);

        var request = new RestRequest($"{_domain}/messages", Method.Post);
        request.AddParameter("from", $"Excited User <mailgun@{_domain}>");
        request.AddParameter("to", toEmail);
        request.AddParameter("subject", subject);
        request.AddParameter("text", body);

        var response = await client.PostAsync(request);

        if (response.IsSuccessful)
        {
            Console.WriteLine("Email sent successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to send email. Status Code: {response.StatusCode}");
            Console.WriteLine(response.Content);
        }
    }
}
