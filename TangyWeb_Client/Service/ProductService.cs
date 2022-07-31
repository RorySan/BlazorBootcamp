using Newtonsoft.Json;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service;

public class ProductService : IProductService
{
	private readonly HttpClient _httpClient;
	private IConfiguration _configuration;
	private string BaseServerUrl;

	public ProductService(HttpClient httpClient, IConfiguration configuration)
	{
		_httpClient = httpClient;
		_configuration = configuration;
		// this will extract the server URL from appsettings.json in wwwroot
		BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
	}
	
	public async Task<IEnumerable<ProductDTO>> GetAll()
	{
		var response = await _httpClient.GetAsync("/api/product");
		if (response.IsSuccessStatusCode)
		{
			var content = await response.Content.ReadAsStringAsync();
			var products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(content);
			foreach (var productDto in products)
			{
				productDto.ImageUrl = BaseServerUrl + productDto.ImageUrl;
			}
			return products;
		}

		return new List<ProductDTO>();
	}

	public async Task<ProductDTO> Get(int productId)
	{
		var response = await _httpClient.GetAsync($"/api/product/{productId}");
		var content = await response.Content.ReadAsStringAsync();
		
		if (response.IsSuccessStatusCode)
		{
			var product = JsonConvert.DeserializeObject<ProductDTO>(content);
			// since images are stored in the server, we add the server url to the image URL
			product.ImageUrl = BaseServerUrl + product.ImageUrl;
			return product;
		}
		else
		{
			var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
			// we could do some kind of logging here
			throw new Exception(errorModel.ErrorMessage);
		}
	}
}