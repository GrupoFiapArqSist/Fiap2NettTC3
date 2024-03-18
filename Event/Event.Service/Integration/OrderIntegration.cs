using Event.Domain.Interfaces.Integration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Event.Service.Integration;

public class OrderIntegration : IOrderIntegration
{
    private readonly IConfiguration _configuration;

    public OrderIntegration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> ExistsOrderByEvent(int eventId, string token)
    {
        using (var httpClient = new HttpClient())
        {
            var baseUrl = _configuration["Gateway:UrlBase"];
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response =
                        await httpClient.GetAsync($"{baseUrl}/api/order/exists-order-by-event/{eventId}");

            if (response.StatusCode == (HttpStatusCode)StatusCodes.Status200OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Convert.ToBoolean(result);
            }

            return true;
        }
    }
}
