using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Order.Domain.Dtos.Event;
using Order.Domain.Interfaces.Integration;
using System.Net.Http.Headers;

namespace Order.Service.Integration;
public class EventIntegration : IEventIntegration
{
	private readonly IConfiguration _configuration;

    public EventIntegration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<EventDto> GetEventById(int eventId, string accessToken)
	{
		using (var httpClient = new HttpClient())
		{
			var baseUrl = _configuration.GetSection("EventApi:UrlBase").Value;
			var url = @$"{baseUrl}{eventId}";
			httpClient.BaseAddress = new Uri(url);
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			var response = await httpClient.GetAsync(url);
			var jsonString = await response.Content.ReadAsStringAsync();
			var dto = JsonConvert.DeserializeObject<EventDto>(jsonString);
			return dto;
		}
	}
}
