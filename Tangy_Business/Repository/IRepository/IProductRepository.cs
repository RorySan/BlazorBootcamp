using Tangy_Models;

namespace Tangy_Business.Repository.IRepository;

public interface IProductRepository
{
	public Task<ProductDTO> Create(ProductDTO objDto);
	public Task<ProductDTO> Update(ProductDTO objDto);
	public Task<int> Delete(int id);

	public Task<ProductDTO> Get(int id);
	public Task<IEnumerable<ProductDTO>> GetAll();
}