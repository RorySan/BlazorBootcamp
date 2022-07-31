using Tangy_Models;

namespace TangyWeb_Client.Service.IService;

public interface IProductService
{
	// this methods point to the API methods that we want to call
	public Task<IEnumerable<ProductDTO>> GetAll();
	public Task<ProductDTO> Get(int productId);

}