using Event.Domain.Interfaces.Integration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Event.Service.Integration
{
    public class OrderIntegration : IOrderIntegration
    {
        private readonly IConfiguration _configuration;

        public OrderIntegration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> GetOrderActiveEvent(int eventId, string token)
        {
            using (var httpClient = new HttpClient())
            {
                var baseUrl = _configuration["Gateway:UrlBase"];
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response =
                            await httpClient.GetAsync($"{baseUrl}/api/order/get-order-active-event/{eventId}");

                if (response.StatusCode != (HttpStatusCode)StatusCodes.Status404NotFound)
                    return true;
                else
                    return false;
            }
        }
    }
}
