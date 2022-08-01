using Newtonsoft.Json;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service;

public class OrderService : IOrderService
{
	private readonly HttpClient _httpClient;
	
	public async Task<IEnumerable<OrderDTO>> GetAll(string? userId)
	{
		var response = await _httpClient.GetAsync("/api/order");
		if (response.IsSuccessStatusCode)
		{
			var content = await response.Content.ReadAsStringAsync();
			var orders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(content);
		
			return orders;
		}

		return new List<OrderDTO>();
	}

	public async Task<OrderDTO> Get(int orderHeaderId)
	{
		var response = await _httpClient.GetAsync($"/api/order/{orderHeaderId}");
		var content = await response.Content.ReadAsStringAsync();
		
		if (response.IsSuccessStatusCode)
		{
			var order = JsonConvert.DeserializeObject<OrderDTO>(content);
			return order;
		}
		else
		{
			var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
			// we could do some kind of logging here
			throw new Exception(errorModel.ErrorMessage);
		}
	}
}