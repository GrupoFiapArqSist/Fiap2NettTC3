using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Order.Domain.Dtos.Event;
using Order.Domain.Interfaces.Integration;
using System.Net.Http.Headers;

namespace Order.Service.Integration;
public class EventIntegration : IEventIntegration
{
	private readonly IConfiguration _configuration;
	private readonly HttpClient _httpClient;

    public EventIntegration(IConfiguration configuration)
    {
        _configuration = configuration;
		_httpClient = new HttpClient();
	}

	public async Task<EventDto> GetEventById(int eventId, string accessToken)
	{
		var baseUrl = _configuration.GetSection("EventApi:UrlBase").Value;
		var url = @$"{baseUrl}{eventId}";
		_httpClient.BaseAddress = new Uri(url);
		_httpClient.DefaultRequestHeaders.Accept.Clear();
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		var response = await _httpClient.GetAsync(url);
		var jsonString = await response.Content.ReadAsStringAsync();
		var dto = JsonConvert.DeserializeObject<EventDto>(jsonString);
		return dto;
	}
}
